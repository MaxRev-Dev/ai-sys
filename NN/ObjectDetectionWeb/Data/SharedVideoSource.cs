using System;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ObjectDetectionWeb.Data
{
    public class SharedVideoSource
    {
        private VideoCapture _capture;
        private CancellationTokenSource _cameraTcs;
        private CancellationToken _token;

        private volatile Mat _current;
        private volatile bool _isActive;

        public void StartCameraCapture()
        {
            if (!_isActive)
            {
                _cameraTcs = new CancellationTokenSource();
                _token = _cameraTcs.Token;
                Task.Run(async () => await Retrieve(_token), _token);
            }
        }

        public Mat Current => _current;
        public bool Enabled => _isActive;

        public void StopCameraCapture() =>
            _cameraTcs?.Cancel();

        public async Task Retrieve(CancellationToken token = default)
        {
            if (_isActive)
                return;
            _isActive = true;
            _capture ??= new VideoCapture();

            _capture.Open(0, VideoCaptureAPIs.DSHOW);

            if (!_capture.IsOpened())
            {
                return;
            }

            try
            {
                var lts =
                    CancellationTokenSource.CreateLinkedTokenSource(token,
                        _token);

                var ltsToken = lts.Token;
                while (!ltsToken.IsCancellationRequested)
                {
                    var mat = _capture.RetrieveMat().Flip(FlipMode.Y);
                    _current = mat;
                    await Task.Delay(1, ltsToken);
                }
            }
            catch (TaskCanceledException){}
            finally
            {
                _capture.Release();
                _isActive = false;
            }
        }
    }
}