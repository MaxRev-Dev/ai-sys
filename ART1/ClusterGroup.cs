using MaxRev.Helpers;

namespace ART1
{
    public class ClusterGroup
    {
        public bool[] Prototype;

        public ClusterGroup(FeatureVector fv)
        {
            Prototype = fv.Features;
        }

        public void AssignAnd(bool[] otherVector)
        {
            Prototype = Prototype.BitwiseAnd(otherVector);
        }
    }
}