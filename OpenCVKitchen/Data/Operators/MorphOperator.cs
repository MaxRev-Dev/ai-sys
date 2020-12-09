using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class MorphOperator : ImageOperator
    {
        public int Size { get; set; } = 5;
        public MorphShapes Shape { get; set; } = MorphShapes.Cross;

        public override Mat Preview(Mat frame)
        {
            return frame;
        }

        public Mat Close(Mat mat_s)
        {
            Mat ret = GetMorph(mat_s, MorphTypes.Close);
            return ret;
        }

        public Mat Open(Mat mat_s)
        {
            Mat ret = GetMorph(mat_s, MorphTypes.Open);
            return ret;
        }

        public Mat Erode(Mat mat_s)
        {
            Mat ret = GetMorph(mat_s, MorphTypes.Erode);
            return ret;
        }

        public Mat Dilate(Mat mat_s)
        {
            Mat ret = GetMorph(mat_s, MorphTypes.Dilate);
            return ret;
        }

        private Mat GetMorph(Mat matS, MorphTypes mtype)
        {
            return matS.MorphologyEx(mtype,
                Cv2.GetStructuringElement(Shape,
                    new Size(Size, Size)));
        }
    }
}