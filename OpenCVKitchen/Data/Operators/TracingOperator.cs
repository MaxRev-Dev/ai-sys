using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class TracingOperator : ImageOperator
    {
        public double LTr { get; set; } = 100;
        public double HTr { get; set; } = 200;

        public double delta { get; set; }

        public double scale { get; set; } = 1;

        public int ksize { get; set; } = 3;

        public override Mat Preview(Mat frame)
        {
            if (ksize % 2 != 1)
                ksize++;

            Mat gray = frame.CvtColor(ColorConversionCodes.BGR2GRAY);
            Mat blur = gray
                .GaussianBlur(new Size(ksize, ksize), 0);
            Mat grX = blur.Sobel(MatType.CV_16S, 1, 0, ksize, scale, delta);
            Mat grY = blur.Sobel(MatType.CV_16S, 0, 1, ksize, scale, delta);
            Mat absGradX = grX.ConvertScaleAbs();
            Mat absGradY = grY.ConvertScaleAbs();
            var grad = new Mat();
            Cv2.AddWeighted(absGradX, 0.5, absGradY, 0.5, 0, grad);

            Mat edges = grad.Canny(LTr, HTr);
            return edges;
        }

        public Mat Sobel(Mat frame)
        {
            Mat gray = frame.CvtColor(ColorConversionCodes.BGR2GRAY);
            Mat blur = gray
                .GaussianBlur(new Size(ksize, ksize), 0);
            return blur.Sobel(MatType.CV_16S, 1, 0, ksize, scale, delta);
        }

        public Mat Canny(Mat frame)
        {
            Mat gray = frame.CvtColor(ColorConversionCodes.BGR2GRAY);
            return gray.Canny(LTr, HTr, ksize);
        }
    }
}