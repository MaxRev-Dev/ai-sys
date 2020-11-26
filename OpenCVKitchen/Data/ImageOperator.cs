using System.Collections.Generic;
using OpenCvSharp;

namespace OpenCVKitchen.Data
{
    public class ImageOperator
    {
        public float Alfa { get; set; } = 1;
        public float Beta { get; set; } = 0;

        public byte[] ChannelR(Mat mat_s)
        {
            var ret = new Mat(mat_s.Size(), MatType.CV_8U);
            Cv2.ExtractChannel(mat_s, ret, 0);
            return ret.ToBytes();
        }

        public byte[] ChannelG(Mat mat_s)
        {
            return mat_s.ExtractChannel(1).ToBytes();
        }

        public byte[] ChannelB(Mat mat_s)
        {
            var ret = new Mat(mat_s.Size(), MatType.CV_8UC1);
            Cv2.ExtractChannel(mat_s, ret, 2);
            return ret.ToBytes();
        }

        public byte[] WarpPerspective(Mat mat_s)
        {
            var ret = new Mat(mat_s.Size(), MatType.CV_8UC1);
             
            var target = new List<Point2f>
            {
                new Point(0, 0),
                new Point(mat_s.Width, 0),
                new Point(mat_s.Width, mat_s.Height),
                new Point(0, mat_s.Height)
            };

            Mat trans = Cv2.GetPerspectiveTransform(corners, target);

            Mat src = new Mat(bitmap.getHeight(), bitmap.getWidth(), MatType.CV_8SC1);
             
            Mat dst = new Mat(bitmap.getHeight(), bitmap.getWidth(), MatType.CV_8SC1 );

            Cv2.WarpPerspective(src, dst, trans, dst.Size());

            return ret.ToBytes();
        }





        public byte[] HistogramAuto(Mat mat)
        {
            using Mat hist = new Mat();
            int[] hdims = { 256 };
            Rangef[] ranges = { new Rangef(0, 256), };
            Cv2.CalcHist(
                new[] { mat },
                new[] { 0 },
                null,
                hist,
                1,
                hdims,
                ranges);

            const int Width = 260, Height = 200;
            double minVal, maxVal;
            Cv2.MinMaxLoc(hist, out minVal, out maxVal);

            Scalar color = Scalar.All(100);
            using Mat hist1 = hist * (maxVal != 0 ? Height / maxVal : 0.0);
            using Mat render = new Mat(new Size(Width, Height), MatType.CV_8UC3, Scalar.All(255));
            for (int j = 0; j < hdims[0]; ++j)
            {
                int binW = (int)((double)Width / hdims[0]);
                render.Rectangle(
                    new Point(j * binW, render.Rows - (int)hist1.Get<float>(j)),
                    new Point((j + 1) * binW, render.Rows),
                    color,
                    -1);
            }
            return render.ToBytes();
        }
        public byte[] EqualizeHist(Mat mat_s)
        {
            return mat_s.CvtColor(ColorConversionCodes.BGR2GRAY)
                .EqualizeHist().ToBytes();
        }

        public byte[] NormHistGrayscale(Mat mat_s)
        {
            return NormHistRGB(mat_s.CvtColor(ColorConversionCodes.BGR2GRAY));
        }


        public byte[] NormHistRGB(Mat mat_s)
        {
            var p = mat_s;
            var mat = p;
            var alfa = Alfa;
            var beta = Beta;
            var ret = new Mat(p.Size(), p.Type());
            for (int x = 0; x < mat.Rows; x++)
            {
                for (int y = 0; y < mat.Cols; y++)
                {
                    for (int h = 0; h < mat.Channels(); h++)
                    {
                        var c = p.At<Vec3b>(x, y)[h];
                        ret.At<Vec3b>(x, y)[h] = (byte)(alfa * c + beta);
                    }
                }
            }

            return ret.ToBytes();
        }

        public byte[] HistogramManual(Mat mat_s)
        {
            var hdims = 256;
            const int Width = 260, Height = 200;
            var mat = mat_s.CvtColor(ColorConversionCodes.BGR2GRAY);
            var a = Alfa; // brightness
            var b = Beta; //contrast

            Mat hist = Mat.Zeros(256, 1, mat.Type());
            mat.ConvertTo(hist, mat.Type(), a, b);
            hist = hist.Normalize(normType: NormTypes.MinMax);

            for (int x = 0; x < mat.Rows; x++)
            {
                for (int y = 0; y < mat.Cols; y++)
                {
                    var c = (int)mat.At<byte>(x, y);
                    hist.At<int>(c) += 1;
                }
            }

            Scalar color = Scalar.All(100);
            using Mat render = new Mat(new Size(Width, Height), MatType.CV_8UC3, Scalar.All(255));
            int binW = (int)((double)Width / hdims);
            for (int j = 0; j < hdims; ++j)
            {
                render.Rectangle(
                    new Point(j * binW, render.Rows - (int)hist.Get<byte>(j)),
                    new Point((j + 1) * binW, render.Rows),
                    color,
                    -1);
            }
            return render.ToBytes();
        }
    }
}