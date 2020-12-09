using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class HistogramOperator : ImageOperator
    {
        public float Alfa { get; set; } = 1;
        public float Beta { get; set; } = 0;

        public override Mat Preview(Mat frame)
        {
            return frame;
        }

        public Mat HistogramAuto(Mat mat)
        {
            int Width = mat.Cols, Height = mat.Rows;
            using var hist = new Mat();
            int[] hdims = {256};
            Rangef[] ranges = {new Rangef(0, 256)};
            Cv2.CalcHist(
                new[] {mat},
                new[] {0},
                null,
                hist,
                1,
                hdims,
                ranges);

            double minVal, maxVal;
            Cv2.MinMaxLoc(hist, out minVal, out maxVal);

            Scalar color = Scalar.All(100);
            using Mat hist1 = hist * (maxVal != 0 ? Height / maxVal : 0.0);
            var render = new Mat(new Size(Width, Height), MatType.CV_8UC3,
                Scalar.All(255));
            for (var j = 0; j < hdims[0]; ++j)
            {
                var binW = (int) ((double) Width / hdims[0]);
                render.Rectangle(
                    new Point(j * binW,
                        render.Rows - (int) hist1.Get<float>(j)),
                    new Point((j + 1) * binW, render.Rows),
                    color,
                    -1);
            }

            return render;
        }

        public Mat EqualizeHist(Mat mat_s)
        {
            return mat_s.CvtColor(ColorConversionCodes.BGR2GRAY)
                .EqualizeHist();
        }

        public Mat NormHistGrayscale(Mat mat_s)
        {
            return NormHistRGB(mat_s.CvtColor(ColorConversionCodes.BGR2GRAY));
        }


        public Mat NormHistRGB(Mat mat_s)
        {
            Mat p = mat_s;
            Mat mat = p;
            var alfa = Alfa;
            var beta = Beta;
            var ret = new Mat(p.Size(), p.Type());
            for (var x = 0; x < mat.Rows; x++)
            for (var y = 0; y < mat.Cols; y++)
            for (var h = 0; h < mat.Channels(); h++)
            {
                var c = p.At<Vec3b>(x, y)[h];
                ret.At<Vec3b>(x, y)[h] = (byte) (alfa * c + beta);
            }

            return ret;
        }

        public Mat HistogramManual(Mat mat_s)
        {
            Mat mat = mat_s.CvtColor(ColorConversionCodes.BGR2GRAY);
            int Width = mat.Cols, Height = mat.Rows;
            var a = Alfa; // brightness
            var b = Beta; //contrast

            Mat hist = Mat.Zeros(256, 1, mat.Type());
            mat.ConvertTo(hist, mat.Type(), a, b);
            hist = hist.Normalize(normType: NormTypes.MinMax);

            for (var x = 0; x < mat.Rows; x++)
            for (var y = 0; y < mat.Cols; y++)
            {
                var c = (int) mat.At<byte>(x, y);
                hist.At<int>(c) += 1;
            }

            Scalar color = Scalar.All(100);
            var hdims = 256;
            var render = new Mat(new Size(Width, Height), MatType.CV_8UC3,
                Scalar.All(255));
            var binW = (int) ((double) Width / hdims);
            for (var j = 0; j < hdims; ++j)
                render.Rectangle(
                    new Point(j * binW, render.Rows - hist.Get<byte>(j)),
                    new Point((j + 1) * binW, render.Rows),
                    color,
                    -1);
            return render;
        }
    }
}