using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class VideoRecorderOperator : ImageOperator
    {
        public enum RecorderEffect
        {
            Morph,
            Blur
        }

        private readonly IHostEnvironment _env;
        private readonly SharedVideoSource _sharedVideoSource;
        private Mat _currentFrame;
        private Size _dsize;
        private bool _isRecording;
        private string _tmp;
        private VideoWriter _writer;

        public VideoRecorderOperator(IHostEnvironment env,
            SharedVideoSource sharedVideoSource)
        {
            _env = env;
            _sharedVideoSource = sharedVideoSource;
        }


        public float Fps { get; set; } = 40;
        public double SigmaX { get; set; } = 10;
        public double SigmaY { get; set; } = 10;
        public RecorderEffect Effect { get; set; } = RecorderEffect.Blur;
        public MorphTypes MorphType { get; set; } = MorphTypes.Open;
        public MorphShapes MorphShape { get; set; } = MorphShapes.Cross;
        public int StartSize { get; set; } = 10;
        public int EndSize { get; set; } = 100;

        private void Initialize()
        {
            if (_isRecording) return;

            var tmpFolder = Path.Combine(_env.ContentRootPath, "Temp");
            Directory.CreateDirectory(tmpFolder);
            _tmp = Path.Combine(tmpFolder, $"capture-{DateTime.Now.Ticks}.avi");

            VideoCapture c = _sharedVideoSource.Capture;
            _dsize = new Size(c.FrameWidth, c.FrameHeight);
            _writer = new VideoWriter(_tmp, VideoCaptureAPIs.FFMPEG,
                FourCC.IYUV, 10, _dsize);

            _isRecording = true;
        }

        public override Mat Preview(Mat frame)
        {
            if (_currentFrame == default)
                return frame;
            return _currentFrame.Clone();
        }

        [OperatorControl]
        public async Task RecordVideo(IAsyncEnumerable<Mat> sequence)
        {
            if (_isRecording)
                return;
            Initialize();

            uint frames = 0;
            var _currentSizeR = StartSize;
            var _currentSize = new Size(_currentSizeR, _currentSizeR);
            using (_writer)
            {
                await foreach (Mat frame in sequence)
                {
                    if (frame.Empty() ||
                        !_isRecording)
                        break;

                    if (_writer.IsDisposed)
                        Initialize();

                    frames++;

                    var secs = frames / 10;
                    if (secs % 1 == 0)
                    {
                        if (StartSize > EndSize)
                        {
                            _currentSizeR--;
                            if (_currentSizeR % 2 != 1)
                                _currentSizeR--;
                        }
                        else
                        {
                            _currentSizeR++;
                            if (_currentSizeR % 2 != 1)
                                _currentSizeR++;
                        }

                        _currentSize = new Size(_currentSizeR, _currentSizeR);
                    }

                    _currentFrame = ApplyOperator(frame, _currentSize);

                    _writer.Write(_currentFrame);

                    await Task.Delay((int) (1000f / Fps));
                    if (StartSize > EndSize && _currentSizeR >= StartSize ||
                        StartSize < EndSize && _currentSizeR >= EndSize)
                        break;
                }
            }

            _currentFrame = default;
        }

        [OperatorControl]
        public void StopRecording()
        {
            _isRecording = false;
        }

        private Mat ApplyOperator(Mat frame, Size _currentSize)
        {
            try
            {
                return Effect switch
                {
                    RecorderEffect.Blur =>
                        frame.GaussianBlur(_currentSize, SigmaX, SigmaY),
                    RecorderEffect.Morph =>
                        frame.MorphologyEx(MorphType,
                            Cv2.GetStructuringElement(MorphShape,
                                _currentSize)),
                    _ => frame
                };
            }
            catch
            {
                // just ignore
            }

            return frame;
        }
    }
}