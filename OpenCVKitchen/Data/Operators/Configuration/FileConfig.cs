namespace OpenCVKitchen.Data.Operators.Configuration
{
    public class FileConfig
    {
        public string HaarCascade { get; } =
            "haarcascade_frontalface_default.xml";

        public string LbpCascade { get; } =
            "lbpcascade_frontalface.xml";
    }
}