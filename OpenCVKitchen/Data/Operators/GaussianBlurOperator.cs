using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class GaussianBlurOperator : ImageOperator
    {
        public int Size { get; set; } = 5;

        public BorderTypes BorderType { get; set; }

        public double SigY { get; set; }

        public double SigX { get; set; } = 1;

        public override Mat Preview(Mat mat_s)
        {
            if (Size % 2 != 1)
                Size++;
            Mat ret = mat_s.GaussianBlur(
                new Size(Size, Size), SigX, SigY, BorderType);
            return ret;
        }
    }
}