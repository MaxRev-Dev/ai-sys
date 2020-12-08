using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class MotionCaptureOperator : ImageOperator
    {
        public int Size { get; set; } = 10;

        private int iLowH = 170;
        private int iHighH = 179;
        private int iLowS = 150;
        private int iHighS = 255;
        private int iLowV = 60;
        private int iHighV = 255;

        private double iLastX = -1;
        private double iLastY = -1;
        private Mat imgLines;
        private Size _frameSize;
        private readonly object _gateLines = new object();

        public Mat Preview(Mat frame)
        { 
            var hsv = frame.CvtColor(ColorConversionCodes.BGR2HSV);

            var dst = new Mat();
            Cv2.InRange(hsv, new Scalar(iLowH, iLowS, iLowV),
                new Scalar(iHighH, iHighS, iHighV), dst);

            Morph(dst, MorphTypes.Open);
            Morph(dst, MorphTypes.Close);

            var mom = Cv2.Moments(dst);

            if (mom.M00 > 10000)
            {
                var posX = mom.M10 / mom.M00;
                var posY = mom.M01 / mom.M00;

                lock (_gateLines)
                {
                    if (imgLines == null)
                    {
                        _frameSize = frame.Size();
                        imgLines = Mat.Zeros(_frameSize, MatType.CV_8UC3)
                            .ToMat();
                    }

                    if (iLastX >= 0 && iLastY >= 0 && posX >= 0 && posY >= 0)
                    {
                        Cv2.Line(imgLines, new Point(posX, posY),
                            new Point(iLastX, iLastY), Scalar.AliceBlue, 2);
                    }

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
                imgLines = default;
        }

        private void Morph(Mat frame, MorphTypes morphTypes)
        {
            frame.MorphologyEx(morphTypes,
                Cv2.GetStructuringElement(MorphShapes.Ellipse,
                    new Size(Size, Size)));
        }
    }
}