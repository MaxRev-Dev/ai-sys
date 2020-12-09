using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class BackFillOperator : ImageOperator
    {
        /// <inheritdoc />
        public override Mat Preview(Mat frame)
        {
            var dst = frame.Clone(); 
            for (int yf = 0; yf < dst.Rows; yf++)
            {
                for (int xf = 0; xf < dst.Cols; xf++)
                {
                    var value = dst.At<Vec3b>(yf, xf); 
                    if (value[0] > b1l &&
                        value[1] > b2l &&
                        value[2] > b3l &&
                        value[0] < b1h &&
                        value[1] < b2h &&
                        value[2] < b3h)
                    {
                        Cv2.FloodFill(dst, new Point(xf, yf), new Scalar(200), out Rect _);
                    }
                }
            }

            return dst;
        }


        public int b1l { get; set; } = 130;
        public int b2l { get; set; } = 130;
        public int b3l { get; set; } = 130;
        public int b1h { get; set; } = 250;
        public int b2h { get; set; } = 250;
        public int b3h { get; set; } = 250;
    }
}