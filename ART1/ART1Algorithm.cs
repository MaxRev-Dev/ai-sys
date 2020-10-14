using System;
using System.Collections.Generic;
using System.Linq;
using MaxRev.Helpers;

namespace ART1
{
    public class ART1Algorithm
    {
        /// <summary>
        /// Similarity
        /// </summary>
        public double Beta { get; set; } = 0.8;

        /// <summary>
        /// Attentiveness
        /// </summary>
        public double P { get; set; } = 0.3;

        /// <summary>
        /// To stop after 
        /// </summary>
        public double MaxIterations { get; set; } = 1000;


        public (List<FeatureVector> vectors, List<ClusterGroup> clusters)
            Process(IEnumerable<bool[]> inputCustomers)
        {
            var i = 0;
            var vectors = inputCustomers.Select(customer => new FeatureVector(i++, customer)).ToList();
            var clusters = new List<ClusterGroup>();
            CreateClusters(vectors, clusters);
            MakeRecommendations(vectors);
            return (vectors, clusters);
        }

        #region ART1 impl

        private void CreateClusters(List<FeatureVector> Vectors, IList<ClusterGroup> Clusters)
        {
            var foundPrototype = false;
            var currentIter = 0;

            while (!foundPrototype)
            {
                foundPrototype = true;

                foreach (var featureVector in Vectors)
                {
                    foreach (var cluster in Clusters)
                    {
                        if (cluster == featureVector.Cluster)
                            continue;
                        if (!SimilarityTest(cluster.Prototype, featureVector.Features))
                            continue;
                        if (!AttentionTest(cluster.Prototype, featureVector.Features))
                            continue;

                        var oldCluster = featureVector.Cluster;
                        featureVector.Cluster = cluster;

                        if (oldCluster != null)
                            HandleOldCluster(Vectors, Clusters, oldCluster);

                        var newVectors = Vectors.FindAll(c => c.Cluster == cluster);
                        if (newVectors.Count > 0)
                            cluster.Prototype = newVectors[0].Features;

                        foreach (var v in newVectors)
                            cluster.AssignAnd(v.Features);

                        foundPrototype = false;
                        break;
                    }

                    if (featureVector.Cluster != null)
                        continue;

                    var newCluster = new ClusterGroup(featureVector);
                    Clusters.Add(newCluster);
                    featureVector.Cluster = newCluster;
                    foundPrototype = false;
                }
                currentIter++;
                if (currentIter > MaxIterations)
                    break;
            }
        }

        private static void HandleOldCluster(List<FeatureVector> vectors,
            ICollection<ClusterGroup> clusters, ClusterGroup oldCluster)
        {
            var oldCustomers = vectors.FindAll(c => c.Cluster == oldCluster);
            if (oldCustomers.Count == 0)
                clusters.Remove(oldCluster);

            if (oldCustomers.Count > 0)
                oldCluster.Prototype = oldCustomers[0].Features;

            foreach (var c in oldCustomers)
            {
                oldCluster.AssignAnd(c.Features);
            }
        }

        private static void MakeRecommendations(List<FeatureVector> vectors)
        {
            foreach (var vector in vectors)
            {
                foreach (var clusterMember in vectors.FindAll(cm =>
                    cm != vector && cm.Cluster == vector.Cluster))
                {
                    for (var f = 0; f < vector.Features.Length; f++)
                    {
                        if (!vector.Features[f])
                            vector.Recommendation[f] += clusterMember.Features[f] ? 1 : 0;
                    }
                }
            }
        }

        #endregion

        #region Tests

        private bool SimilarityTest(bool[] prototype, bool[] next)
        {
            if (next.Length != prototype.Length)
                throw HelperExtensions.VectorSizeDiffers;

            var left = Count(next.BitwiseAnd(prototype)) / (Beta + Count(prototype));
            var right = Count(next) / (Beta + prototype.Length);

            return left > right;
        }

        private bool AttentionTest(bool[] prototype, bool[] next)
        {
            if (next.Length != prototype.Length)
                throw HelperExtensions.VectorSizeDiffers;

            var test =
                Count(next.BitwiseAnd(prototype)) /
                Count(next);
            return test < P;
        }

        #endregion

        private int Count(IEnumerable<bool> proto)
        {
            return proto.Count(x => x);
        }
    }
}