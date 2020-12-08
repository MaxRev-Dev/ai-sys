using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;

namespace OpenCVKitchen.Data
{
    public class SharedVideoSource
    {
        private CancellationTokenSource _cameraTcs;
        private VideoCapture _capture;

        private volatile Mat _current;
        private volatile bool _isActive;
        private CancellationToken _token;

        public Mat Current => _current;
        public bool Enabled => _isActive;
        public VideoCapture Capture => _capture;

        public void StartCameraCapture()
        {
            if (!_isActive)
            {
                _cameraTcs = new CancellationTokenSource();
                _token = _cameraTcs.Token;
                Task.Run(async () => await Retrieve(_token), _token);
            }
        }

        public void StopCameraCapture()
        {
            _cameraTcs?.Cancel();
        }

        public async Task Retrieve(CancellationToken token = default)
        {
            if (_isActive)
                return;
            _isActive = true;
            _capture ??= new VideoCapture();

            _capture.Open(0, VideoCaptureAPIs.DSHOW);

            if (!_capture.IsOpened()) return;

            try
            {
                var lts =
                    CancellationTokenSource.CreateLinkedTokenSource(token,
                        _token);

                CancellationToken ltsToken = lts.Token;
                while (!ltsToken.IsCancellationRequested)
                {
                    using Mat mat = _capture.RetrieveMat().Flip(FlipMode.Y);
                    _current = mat.Clone();
                    await Task.Delay(1, ltsToken);
                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                _capture.Release();
                _isActive = false;
            }
        }
    }
}