using OpenCvSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenCVKitchen.Data
{
    public class ImageService
    {
        private readonly SharedVideoSource _capture;

        public ImageService(SharedVideoSource capture)
        {
            _capture = capture;
        }

        public async IAsyncEnumerable<Mat> GetStream()
        {
            _capture.StartCameraCapture();
            while (true)
            {
                using var mat = _capture.Current;
                if (mat == default || mat.IsDisposed || mat.Empty()) continue;
                yield return mat;
                await Task.Delay(100);
            }
        }
    }
}