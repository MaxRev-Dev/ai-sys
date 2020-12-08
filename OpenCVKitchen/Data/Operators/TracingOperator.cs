using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class TracingOperator : ImageOperator
    {
        public Mat Preview(Mat frame)
        {
            if (ksize % 2 != 1)
                ksize++;

            var gray = frame.CvtColor(ColorConversionCodes.BGR2GRAY);
            var blur = gray
                .GaussianBlur(new Size(ksize, ksize), 0);
            var grX = blur.Sobel(MatType.CV_16S, 1, 0, ksize, scale, delta);
            var grY = blur.Sobel(MatType.CV_16S, 0, 1, ksize, scale, delta);
            var absGradX = grX.ConvertScaleAbs();
            var absGradY = grY.ConvertScaleAbs();
            var grad = new Mat();
            Cv2.AddWeighted(absGradX, 0.5, absGradY, 0.5, 0, grad);
            
            var edges = gray.Canny(100, 200);
            return edges;
        }

        public double delta { get; set; }

        public double scale { get; set; } = 1;

        public int ksize { get; set; } = 3;
    }
}