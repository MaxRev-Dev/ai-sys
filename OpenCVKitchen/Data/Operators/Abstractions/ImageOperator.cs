using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators.Abstractions
{
    public abstract class ImageOperator
    {
        private string _name;
        public bool Enabled { get; set; } = true;
        public int Priority { get; } = 0;
        public string Name => _name ??= GetType().Name;

        public virtual Mat Preview(Mat frame)
        {
            return frame;
        }
    }
}