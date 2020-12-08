using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class MedianBlurOperator : ImageOperator
    {
        public int Size { get; set; } = 5;

        public Mat MedianBlur(Mat mat_s)
        {
            return mat_s.MedianBlur(Size);
        }
    }
}