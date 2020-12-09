using System.Linq;
using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class ContoursOperator : ImageOperator
    {
        public double LTr { get; set; } = 127;
        public double HTr { get; set; } = 255;

        public ContourApproximationModes ContourApproximationMode { get; set; }
            = ContourApproximationModes.ApproxSimple;

        public RetrievalModes RetrievalMode { get; set; }
            = RetrievalModes.Tree;

        public int Size { get; set; } = 3;

        public MorphShapes MorphShape { get; set; } = MorphShapes.Cross;

        public MorphTypes MorphType { get; set; } = MorphTypes.Dilate;

        public int MaxLevel { get; set; } = 1;

        public override Mat Preview(Mat frame)
        {
            Mat gray = frame.CvtColor(ColorConversionCodes.BGR2GRAY);
            var thres = gray.Threshold(LTr, HTr, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            var dilate = thres.MorphologyEx(MorphType,
                Cv2.GetStructuringElement(MorphShape, new Size(Size, Size)));
            var contours = dilate
                .FindContoursAsArray(RetrievalMode, ContourApproximationMode);
            var fst = contours.OrderByDescending(x => x.Length).Take(1);
            frame.DrawContours(fst, -1, Scalar.Red, 3,
                LineTypes.Link8, default, MaxLevel);
            return frame;
        }

    }
}