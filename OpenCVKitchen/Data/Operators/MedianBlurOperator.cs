using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class MedianBlurOperator : ImageOperator
    {
        public int Size { get; set; } = 5;

        public override Mat Preview(Mat frame)
        {
            if (Size % 2 != 1)
                Size++;
            return frame.MedianBlur(Size);
        }
    }
}