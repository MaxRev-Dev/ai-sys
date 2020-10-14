using MaxRev.Extensions.Binary;
using MaxRev.Extensions.Matrix;
using MaxRev.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace ART1
{
    public partial class MainForm : Form
    {
        private bool[][] _vectorInputs;
        private string[] _items;
        private (List<FeatureVector> vectors, List<ClusterGroup> clusters)? _resultPair;
        private readonly StringBuilder _output = new StringBuilder();
        private readonly Timer _heartBeat = new Timer();
        private bool _redrawRequired = true;
        private float _betaValue;
        private float _roValue;

        public MainForm()
        {
            InitializeComponent();
            _heartBeat.Elapsed += OnHeartBeat;
            _heartBeat.Interval = 50;
            _heartBeat.Start();
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
            ShowResults();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GenerateRandom();
        }


        private void ShowResults()
        {
            if (_vectorInputs == default)
            {
                return;
            }

            _betaValue = (float)betaInput.Value;
            _roValue = (float)roInput.Value;
            var algorithm = new ART1Algorithm
            {
                Beta = _betaValue,
                P = _roValue,
                MaxIterations = 1000
            };
            _resultPair = algorithm.Process(_vectorInputs);
            _output.Clear();
            var (vectors, clusters) = _resultPair.Value;
            PrepareClusters(vectors, clusters);
            PrepareRecommendations(vectors, _items);
            richTextBox2.Text = _output.ToString();
        }


        private void PrepareClusters(List<FeatureVector> vectors, List<ClusterGroup> clusters)
        {
            foreach (var cluster in clusters)
            {
                _output.AppendLine($"Prototype: {vTos(cluster.Prototype)}");
                foreach (var customer in vectors.FindAll(c => c.Cluster == cluster))
                {
                    _output.AppendLine($"Client {customer.Index + 1}: {vTos(customer.Features)}");
                }
                _output.AppendLine();
            }

        }

        public void PrepareRecommendations(IEnumerable<FeatureVector> vectors, string[] labels)
        {
            foreach (var customer in vectors)
            {
                _output.AppendLine($"Client {customer.Index + 1}: {vTos(customer.Recommendation)}");

                if (customer.Recommendation.Sum() != 0)
                {
                    var index = customer.Recommendation.ToList().IndexOf(customer.Recommendation.Max());
                    _output.AppendLine($"Recommended: {(index >= labels.Length ? "[null]" : labels[index])}");
                }
                else
                {
                    _output.AppendLine("-");
                }
            }
        }

        private string vTos<T>(T[] item)
        {
            var builder = new StringBuilder();

            foreach (var i in item)
            {
                builder.AppendFormat("{0,2}", (!Equals(i, default(T)) ? 1 : 0));
            }

            return builder.ToString();
        }

        private void GenerateRandomClick(object sender, EventArgs e)
        {
            GenerateRandom();
            ShowResults();
        }

        private void GenerateRandom()
        {
            _redrawRequired = true;
            var r = (int)clientsCountInput.Value;
            _items = HelperExtensions.LoremIpsum.Split(new[] { ' ', '\n' }, r + 1).Take(r)
                .Select((x, i) => x.Trim(',', '.')).ToArray();

            _vectorInputs = RandomExtensions.Default
                .MatrixRandomI((int)clientsCountInput.Value, 0, 2)
                .Cast<int, bool>()
                .Convert();
            UpdateInputControls();
        }

        private void UpdateInputControls()
        {
            if (_items != default)
            {
                itemsInputText.Text = string.Join(" ", _items);
            }
            if (_vectorInputs != default)
            {
                vInputsText.Text = string.Join("\n",
                    _vectorInputs.Select(vTos));
            }
        }

        private void GetVectorInputs()
        {
            try
            {
                var raw = vInputsText.Text;
                var lines = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x =>
                        x.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse).Select(v => v == 1).ToArray());
                _vectorInputs = lines.ToArray();
                if (_vectorInputs.Any())
                    _items = _items.Take(_vectorInputs[0].Length).ToArray();
            }
            catch (FormatException)
            {
            }
        }

        private void vInputsText_TextChanged(object sender, EventArgs e)
        {
            GetVectorInputs();
            _redrawRequired = true;
        }

        private void itemsInputText_TextChanged(object sender, EventArgs e)
        {
            GetItemsInput();
            _redrawRequired = true;
        }

        private void GetItemsInput()
        {
            var raw = itemsInputText.Text;
            var lines = raw.Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            _items = lines.ToArray();
        }

        private void clientsCountInput_ValueChanged(object sender, EventArgs e)
        {
            GenerateRandom();
        }

        private void betaInput_ValueChanged(object sender, EventArgs e)
        {
            _redrawRequired = true;
        }

        private void roInput_ValueChanged(object sender, EventArgs e)
        {
            _redrawRequired = true;
        }
    }
}
