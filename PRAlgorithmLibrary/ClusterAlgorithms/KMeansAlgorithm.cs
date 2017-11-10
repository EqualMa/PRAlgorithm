using System;
using System.Collections.Generic;
using System.Text;
using static PRAlgorithmLibrary.ClusterAlgorithms.DistanceOperations;

namespace PRAlgorithmLibrary.ClusterAlgorithms
{
    class KMeansAlgorithm : IClusterAlgorithm
    {
        public int K { get; set; }

        public List<List<Vector>> Cluster(Vector[] vectors)
        {
            List<Vector> centers = new List<Vector>();

            //任选K个初始聚类中心
            for (int i = 0; i < K; i++)
            {
                centers.Add(vectors[i]);
            }

            //分配所有样本


        }

        public void SetParameter(string paraName, decimal paraValue)
        {
            throw new NotImplementedException();
        }
    }
}
