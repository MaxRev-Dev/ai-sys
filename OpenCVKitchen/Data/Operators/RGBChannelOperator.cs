using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class RGBChannelOperator : ImageOperator
    {
        public override Mat Preview(Mat frame)
        {
            return frame;
        }

        public Mat ChannelR(Mat mat_s)
        {
            Mat g = mat_s.ExtractChannel(2);
            var ret = new Mat(mat_s.Size(), mat_s.Type());
            Cv2.Merge(new Mat[]
            {
                Mat.Zeros(mat_s.Size(), MatType.CV_8UC1),
                Mat.Zeros(mat_s.Size(), MatType.CV_8UC1),
                g
            }, ret);
            return ret;
        }

        public Mat ChannelG(Mat mat_s)
        {
            Mat g = mat_s.ExtractChannel(1);
            var ret = new Mat(mat_s.Size(), mat_s.Type());
            Cv2.Merge(new Mat[]
            {
                Mat.Zeros(mat_s.Size(), MatType.CV_8UC1),
                g,
                Mat.Zeros(mat_s.Size(), MatType.CV_8UC1)
            }, ret);
            return ret;
        }

        public Mat ChannelB(Mat mat_s)
        {
            Mat g = mat_s.ExtractChannel(0);
            var ret = new Mat(mat_s.Size(), mat_s.Type());
            Cv2.Merge(new Mat[]
            {
                g,
                Mat.Zeros(mat_s.Size(), MatType.CV_8UC1),
                Mat.Zeros(mat_s.Size(), MatType.CV_8UC1)
            }, ret);
            return ret;
        }
    }
}