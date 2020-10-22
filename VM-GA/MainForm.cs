using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using VM_GA.Abstractions;
using Timer = System.Timers.Timer;

namespace VM_GA
{
    public partial class MainForm : Form
    {
        private readonly Timer _heartBeat = new Timer();
        private bool _redrawRequired = true;

        public MainForm()
        {
            InitializeComponent();
            chart1 = new Chart { Dock = DockStyle.Fill };
            splitContainer1.Panel2.Controls.Add(chart1);
            splitContainer1.Panel1.Controls.Add(chart1);
            _heartBeat.Elapsed += OnHeartBeat;
            _heartBeat.Interval = 100;
            _heartBeat.Start();

        }

        private IGAInfo _info;
        private Chart chart1;
        private DataTable table;
        private Series _currentSeries;
        private Series _currentSeries2;
        private CancellationToken cancellation;
        private CancellationTokenSource cancellationSource;

        private volatile int w;
        private GeneticAlgorithm ga;
        private int gen;
        private Stopwatch st;
        private int _inputThrottle = 500;

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateToken();
            table = new DataTable();
            table.Columns.Add("Generation");
            table.Columns.Add("World");
            chart1.DataSource = table;
            var chartArea1 = new ChartArea();
            var legend1 = new Legend();
            chartArea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea1.BackColor = System.Drawing.Color.Gainsboro;
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new System.Drawing.Point(3, 3);
            chart1.Name = "chart1";
            chart1.Palette = ChartColorPalette.Excel;
            chart1.Size = new System.Drawing.Size(797, 561);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";

            chart1.PerformLayout();
            CreateGA();
        }

        private void CreateGA()
        {
            ga = new GeneticAlgorithm();
            pxInput.Text = ga.CrossProbability.ToString(CultureInfo.InvariantCulture);
            pmInput.Text = ga.MutationProbability.ToString(CultureInfo.InvariantCulture);
            chromosomesCount.Text = ga.ChromosomeCount.ToString(CultureInfo.InvariantCulture);
            generationsCount.Text = ga.MaxGenerations.ToString(CultureInfo.InvariantCulture);

            ga.NewLoop += info =>
            {
                _info = info;
                _redrawRequired = true;
            };
            ga.NewGeneration += info =>
            {
                var rs = w < info.OperatorWorld;
                w = info.OperatorWorld;
                gen = info.Generation;
                if (rs)
                {
                    Invoke(new Action(() =>
                    {
                        _currentSeries = new Series
                        {
                            ChartType = SeriesChartType.Line,
                            Name = "MAX G:" + _info.OperatorWorld,
                            BorderWidth = 3
                        };
                        _currentSeries2 = new Series
                        {
                            ChartType = SeriesChartType.Line,
                            Name = "AVG G:" + _info.OperatorWorld,
                            BorderWidth = 3
                        };
                        chart1.Series.Add(_currentSeries);
                        chart1.Series.Add(_currentSeries2);
                    }));
                }
                var currentGenerations = info.Generation;
                if (currentGenerations < 2 || currentGenerations % 1000 == 0)
                {

                    lock (table)
                    {
                        if (!IsDisposed)
                            Invoke(new Action(() =>
                            {
                                _currentSeries.Points.AddXY(_info.Generation, _info.MaxFitness);
                                _currentSeries2.Points.AddXY(_info.Generation, _info.AverageFitness);
                                var sb = new StringBuilder();
                                sb.AppendLine("Processing...");
                                sb.AppendLine($"Crossovers: {info.Crossovers}");
                                sb.AppendLine($"Mutations: {info.Mutations}");
                                sb.AppendLine($"Results: {info.Results.Count}");
                                sb.AppendLine(currentTimeString());
                                sb.AppendLine(getResults());
                                richTextBox1.Text = sb.ToString();
                            }));
                    }

                    _redrawRequired = true;
                }
            };
        }

        private string getResults()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("---- Results ----");
            sb.AppendLine($"Solutions: {_info.Results.Count}");
            sb.AppendLine();
            foreach (var result in _info.Results)
            {
                sb.AppendLine($"Tests passed: {result.PassedTests}");
                sb.AppendLine($"Fitness: {result.Chromosome.Fitness}");
                sb.AppendLine($"Operators count: {result.Chromosome.Result.Length}");
                sb.AppendLine($"Operators: {string.Join(" ", result.Chromosome.Result)}");
                sb.AppendLine("------");
            }
            return sb.ToString();
        }

        private string currentTimeString()
        {
            return $"Elapsed: {st.Elapsed.Seconds} s {st.Elapsed.Milliseconds} ms";
        }

        private void CreateToken()
        {
            cancellationSource?.Cancel();
            cancellationSource = new CancellationTokenSource();
            cancellation = cancellationSource.Token;
        } 
         
        private async void StartProcessing()
        {
            _redrawRequired = true;
            w = 0;
            richTextBox1.Text = "Warming up...";
            chart1.Series.Clear();
            await Task.Run(() =>
            {
                st = Stopwatch.StartNew();
                Invoke(new Action(() => { richTextBox1.Text = "Processing..."; }));
                var sb = new StringBuilder();
                if (ga.Process(cancellation))
                {
                    st.Stop();
                    sb.AppendLine(cancellation.IsCancellationRequested ? "Cancelled" : "Finished");
                    sb.AppendLine($"Generation (world {_info.OperatorWorld}): {gen}"); 
                    sb.AppendLine(getResults());
                }
                else
                {
                    st.Stop();
                    sb.AppendLine("True answer was not found");
                }

                sb.AppendLine(currentTimeString());

                Invoke(new Action(() => { runBtn.Enabled = true; richTextBox1.Text = sb.ToString(); }));
            }, cancellation);
        }

        private void OnHeartBeat(object sender, ElapsedEventArgs e)
        {
            if (!_redrawRequired) return;
            lock (_heartBeat)
            {
                if (!_redrawRequired) return;
                if (!IsDisposed)
                    Invoke(new Action(DoRedraw));
                _redrawRequired = false;
            }
        }

        private void DoRedraw()
        {
            chart1.Invalidate();
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            CreateToken();
            StartProcessing();
            runBtn.Enabled = false;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            runBtn.Enabled = true;
            cancellationSource.Cancel();
        }

        private async void pxInput_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(_inputThrottle);
            if (!ga.Busy && float.TryParse(pxInput.Text, out var res) && res >= 0.0 && res <= 1.0)
            {
                ga.CrossProbability = res;
            }
            else
            {
                pxInput.Text = ga.CrossProbability.ToString(CultureInfo.InvariantCulture);
            }
        }

        private async void pmInput_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(_inputThrottle);
            if (!ga.Busy && float.TryParse(pmInput.Text, out var res) && res >= 0.0 && res <= 1.0)
            {
                ga.MutationProbability = res;
            }
            else
            {
                pmInput.Text = ga.MutationProbability.ToString(CultureInfo.InvariantCulture);
            }
        }

        private async void generationsCount_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(_inputThrottle);
            if (!ga.Busy && int.TryParse(generationsCount.Text, out var res) && res > 2)
            {
                ga.MaxGenerations = res;
            }
            else
            {
                generationsCount.Text = ga.MaxGenerations.ToString(CultureInfo.InvariantCulture);
            }
        }

        private async void textBox1_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(_inputThrottle);
            if (!ga.Busy && int.TryParse(chromosomesCount.Text, out var res) && res > 2)
            {
                ga.ChromosomeCount = res;
            }
            else
            {
                chromosomesCount.Text = ga.ChromosomeCount.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
