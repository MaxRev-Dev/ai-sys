using System.Collections.Generic;
using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class TransformOperator : ImageOperator
    {
        public float TLX { get; set; }
        public float TLY { get; set; }

        public float TRX { get; set; }
        public float TRY { get; set; }

        public float BLX { get; set; }
        public float BLY { get; set; }

        public float BRX { get; set; }
        public float BRY { get; set; }

        public override Mat Preview(Mat frame)
        {
            var target = new List<Point2f>
            {
                new Point(0, 0),
                new Point(frame.Width, 0),
                new Point(frame.Width, frame.Height),
                new Point(0, frame.Height)
            };
            var corners = new List<Point2f>
            {
                new Point(0 + TLX, 0 + TLY),
                new Point(frame.Width + TRX, 0 + TRY),
                new Point(frame.Width + BRX, frame.Height + BRY),
                new Point(0 + BLX, frame.Height + BLY)
            };

            Mat trans = Cv2.GetPerspectiveTransform(corners, target);

            var dst = new Mat(frame.Size(), frame.Type());

            Cv2.WarpPerspective(frame, dst, trans, dst.Size());

            return dst;
        }
    }
}