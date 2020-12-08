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

        public List<Point2f> Corners { get; set; }

        public Mat WarpPerspective(Mat mat_s)
        {
            var target = new List<Point2f>
            {
                new Point(0, 0),
                new Point(mat_s.Width, 0),
                new Point(mat_s.Width, mat_s.Height),
                new Point(0, mat_s.Height)
            };
            var corners = Corners ?? new List<Point2f>
            {
                new Point(0 + TLX, 0 + TLY),
                new Point(mat_s.Width + TRX, 0 + TRY),
                new Point(mat_s.Width + BRX, mat_s.Height + BRY),
                new Point(0 + BLX, mat_s.Height + BLY)
            };

            Mat trans = Cv2.GetPerspectiveTransform(corners, target);

            var dst = new Mat(mat_s.Size(), mat_s.Type());

            Cv2.WarpPerspective(mat_s, dst, trans, dst.Size());

            return dst;
        }
    }
}