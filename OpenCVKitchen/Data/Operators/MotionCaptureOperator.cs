using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class MotionCaptureOperator : ImageOperator
    {
        private readonly object _gateLines = new object();
        private readonly int iHighH = 179;
        private readonly int iHighS = 255;
        private readonly int iHighV = 255;

        private readonly int iLowH = 170;
        private readonly int iLowS = 150;
        private readonly int iLowV = 60;

        private double iLastX = -1;
        private double iLastY = -1;
        private Mat imgLines;
        public int Size { get; set; } = 10;

        public override Mat Preview(Mat frame)
        {
            Mat hsv = frame.CvtColor(ColorConversionCodes.BGR2HSV);

            var dst = new Mat();
            Cv2.InRange(hsv, new Scalar(iLowH, iLowS, iLowV),
                new Scalar(iHighH, iHighS, iHighV), dst);

            Morph(dst, MorphTypes.Open);
            Morph(dst, MorphTypes.Close);

            Moments mom = Cv2.Moments(dst);

            if (mom.M00 > 10000)
            {
                var posX = mom.M10 / mom.M00;
                var posY = mom.M01 / mom.M00;

                lock (_gateLines)
                {
                    imgLines ??= Mat.Zeros(frame.Size(), MatType.CV_8UC3)
                        .ToMat();

                    if (iLastX >= 0 && iLastY >= 0 && posX >= 0 && posY >= 0)
                        Cv2.Line(imgLines, new Point(posX, posY),
                            new Point(iLastX, iLastY), Scalar.AliceBlue, 2);

                    iLastX = posX;
                    iLastY = posY;

                    frame += imgLines;
                }
            }

            return frame;
        }

        [OperatorControl]
        public void Reset()
        {
            // just to ensure it won't be kicked
            lock (_gateLines)
            {
                imgLines = default;
            }
        }

        private void Morph(Mat frame, MorphTypes morphTypes)
        {
            frame.MorphologyEx(morphTypes,
                Cv2.GetStructuringElement(MorphShapes.Ellipse,
                    new Size(Size, Size)));
        }
    }
}