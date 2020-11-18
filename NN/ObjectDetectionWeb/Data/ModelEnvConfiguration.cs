namespace ObjectDetectionWeb.Data
{
    public class ModelEnvConfiguration
    {
        public string ModelPath { get; set; }
        public string DatasetPath { get; set; }
        public string TestDatasetPath { get; set; }
        public bool EnableDetection { get; set; }
    }
}