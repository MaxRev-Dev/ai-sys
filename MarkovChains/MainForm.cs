using MaxRev.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Timers;
using System.Web;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace MarkovChains
{
    public partial class MainForm : Form
    {
        private readonly Timer _heartBeat = new Timer();
        private bool _redrawRequired = true;
        private readonly MarkovChain _chain;
        private string newsLink = "https://api-news.nuwee.maxrev.pp.ua/api/news?html=0&offset=0,100";
        private ExpandoObject cachedJsonNews;
        private int _resultLength;

        public MainForm()
        {
            InitializeComponent();
            _heartBeat.Elapsed += OnHeartBeat;
            _heartBeat.Interval = 50;
            _heartBeat.Start();
            _chain = new MarkovChain();
            Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GenerateText();
        }

        private void OnHeartBeat(object sender, ElapsedEventArgs e)
        {
            if (!_redrawRequired) return;
            lock (_heartBeat)
            {
                if (!_redrawRequired) return;
                this.Invoke(new Action(DoRedraw));
                _redrawRequired = false;
            }
        }

        private void DoRedraw()
        {
            if (string.IsNullOrEmpty(richTextBox1.Text)) return;
            var d = _chain.Learn(richTextBox1.Text, (int)ngramInput.Value);
            richTextBox2.Text = _chain.GenerateString(d, _resultLength, false);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            GenerateText();
        }

        private void GenerateText()
        {
            richTextBox1.Text = HelperExtensions.GetRandomText(100);
        }

        private void richTextBox1_TextChanged(object sender, System.EventArgs e)
        {
            _redrawRequired = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FetchNews();
        }

        private async void FetchNews()
        {
            using var httpClient = new HttpClient();
            using var result = await httpClient.GetAsync(newsLink);

            var sb = new StringBuilder();
            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStreamAsync();
                cachedJsonNews = await JsonSerializer.DeserializeAsync<ExpandoObject>(json);

            }

            if (cachedJsonNews == default) return;
            var res = ((IDictionary<string, object>)cachedJsonNews)["response"];
            foreach (var raw in ((JsonElement)res).GetProperty("item")
                .EnumerateArray().Select(item =>
                    item.GetProperty("detailed").GetProperty("content")
                ).Select(text => text.ToString()))
            {
                var txt = HttpUtility.HtmlDecode(raw).Replace("\r\n", " ")
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
                foreach (var s in txt)
                    sb.Append(s + " ");
            }
            richTextBox1.Text = sb.ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _resultLength = (int)numericUpDown1.Value;
            _redrawRequired = true;
        }
    }
}
