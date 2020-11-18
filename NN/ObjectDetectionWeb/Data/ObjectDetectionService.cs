using AISysML.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ObjectDetectionWeb.Data
{
    public class ObjectDetectionService
    {
        private readonly SharedVideoSource _capture;
        private readonly IOptions<ModelEnvConfiguration> _config;
        private CancellationToken token;

        public bool Recognition { get; set; }

        public bool IsEnabled => Recognition && DetectionResult != default;

        public ModelOutput DetectionResult;
        private DateTime _lastChangeModel;

        public ObjectDetectionService(SharedVideoSource capture,
            IOptions<ModelEnvConfiguration> config)
        {
            _capture = capture;
            _config = config;
            Initialize();
            ConsumeModel.Predict(_config.Value.ModelPath, new ModelInput());
        }

        public void Initialize()
        {
            var mp = _config.Value.ModelPath;
            var currentTimeStamp = File.GetLastWriteTime(mp);
            if (ConsumeModel.Instance == default ||
                _lastChangeModel - currentTimeStamp > TimeSpan.FromMinutes(1))
                ConsumeModel.CreatePredictionEngine(mp);
            _lastChangeModel = currentTimeStamp;
        }

        public async IAsyncEnumerable<byte[]> GetStream()
        {
            var mp = _config.Value.ModelPath;
            Initialize();
            _capture.StartCameraCapture();
            while (true)
            {
                var mat = _capture.Current;
                if (mat == default || mat.Empty()) continue;
                var bytes = mat.ToBytes();

                if (Recognition)
                {
                    var predictionResult = ConsumeModel.Predict(mp,
                        new ModelInput
                        {
                            Image = bytes
                        });
                    Console.WriteLine(
                        $"{predictionResult.Prediction} : {predictionResult.Score}");
                    DetectionResult = predictionResult;
                }
                yield return bytes;
                await Task.Delay(100, token);
            }
        }
    }
}