using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary.ClusterAlgorithms
{
    class KMeansAlgorithm : IClusterAlgorithm
    {

        public List<List<Vector>> Cluster(Vector[] vectors)
        {
            List<Vector> centers = new List<Vector>();

            List<List<Vector>> clusters = new List<List<Vector>>(K);

            for (int i = 0; i < K; i++)
            {
                clusters.Add(new List<Vector>());
            }

            //任选K个初始聚类中心
            for (int i = 0; i < K; i++)
            {
                centers.Add(vectors[i]);
            }



            bool end = false;
            while (!end)
            {
                //分配所有样本
                clusters = DistanceOperations.Sort(centers, vectors);

                end = true;

                //计算新聚类中心
                for (int i = 0; i < centers.Count; i++)
                {
                    Vector newCenter = CalculateCenter(clusters[i]);

                    if (!newCenter.ValueEquals(centers[i]))
                    {
                        centers[i] = newCenter;
                        end = false;
                    }
                }
            }

            return clusters;
        }


        private Vector CalculateCenter(List<Vector> vectors)
        {
            if (vectors.Count == 0) return null;
            if (vectors.Count == 1) return vectors[0];

            int d = vectors[0].Dimension;

            Vector center = Vector.GetZeroVector(d);


            foreach (var vector in vectors)
            {
                center += vector;
            }

            center /= vectors.Count;

            return center;
        }

        public void SetParameter(string paraName, decimal paraValue = 0.5M)
        {
            if (paraName == ParaName_K)
                K = decimal.ToInt32(paraValue);
            else throw new ParameterNotExistException(paraName);
        }

        public const string ParaName_K = "K";

        public int K { get; set; }

    }
}
