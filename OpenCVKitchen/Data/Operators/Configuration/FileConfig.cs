namespace OpenCVKitchen.Data.Operators.Configuration
{
    public class FileConfig
    {
        public string HaarCascade { get; set; } =
            "haarcascade_frontalface_default.xml";

        public string LbpCascade { get; set; } =
            "lbpcascade_frontalface.xml";
    }
}