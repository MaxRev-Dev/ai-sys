namespace ART1
{
    public class FeatureVector
    {
        public FeatureVector(int index, bool[] features)
        {
            Index = index;
            Features = features;
            Recommendation = new int[features.Length];
        }

        public int Index { get; }
        public bool[] Features { get; }
        public int[] Recommendation { get; }
        public ClusterGroup Cluster { get; set; }
    }
}