using Microsoft.Extensions.Options;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ObjectDetectionWeb.Data
{
    public class TrainDataService
    {
        private readonly SharedVideoSource _videoSource;
        private readonly IOptions<ModelEnvConfiguration> _config;
        private readonly ModelBuilder _modelBuilder;
        private readonly LogQueue _logQueue;
        public bool IsBusy { get; private set; }
        public bool ModelChanged { get; private set; }
        public Dictionary<string, string> Saved { get; private set; }
            = new Dictionary<string, string>();

        public TrainDataService(SharedVideoSource videoSource,
            IOptions<ModelEnvConfiguration> config,
            ModelBuilder modelBuilder, LogQueue logQueue)
        {
            _videoSource = videoSource;
            _config = config;
            _modelBuilder = modelBuilder;
            _logQueue = logQueue;
        }

        public async Task RecordSequence(string currentRpsKey, bool testData, Action savedOne)
        {
            _videoSource.StartCameraCapture();
            Saved.Clear();
            RecordEnabled = true;
            while (RecordEnabled)
            { 
                var mat = _videoSource.Current;
                var targetFolder = !testData ? _config.Value.DatasetPath : _config.Value.TestDatasetPath;
                var path = Path.Combine(targetFolder,
                    (currentRpsKey.Contains("me") ? "" : "me-") + currentRpsKey.ToLower());
                Directory.CreateDirectory(path);
                var file = Path.Combine(path,
                    $"{currentRpsKey}_{IdGenerator.GetBase62(7)}.png");
                mat.SaveImage(file);
                mat.Resize(new Size(200, 200));
                Saved[file] = Convert.ToBase64String(mat.ToBytes());
                if (Saved.Count > 5)
                    Saved = new Dictionary<string, string>(Saved.TakeLast(5).ToArray());
                savedOne?.Invoke();
                if (!RecordEnabled) return;
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public bool RecordEnabled { get; set; }
        public LogQueue Log2 => _logQueue;

        public IEnumerable<string> Keys => Enum.GetNames(typeof(RpsKeys))
            .Concat(new[]
            {
                "me",
                "nothing",
                "<custom>"
            }).Concat(UserValues);

        public List<string> UserValues { get; set; } = new List<string>();

        public async Task TrainModelAsync(Action stateChanged = default)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                _videoSource.StopCameraCapture();
                await Task.Run(() =>
                {
                    _modelBuilder.CreateModel(_config.Value.ModelPath,
                        _config.Value.DatasetPath,
                        _config.Value.TestDatasetPath, onLog: (log) =>
                        {
                            while (_logQueue.Count > 10)
                            {
                                _logQueue.Dequeue();
                            }
                            stateChanged?.Invoke();
                        });
                    ModelChanged = true;
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async IAsyncEnumerable<byte[]> GetStream()
        {
            while (_videoSource.Enabled)
            {
                yield return _videoSource.Current.ToBytes();
                await Task.Delay(10);
            }
        }
    }
}
