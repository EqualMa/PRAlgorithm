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

            List<List<Vector>> clusters = null;

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
                    Vector newCenter = DistanceOperations.CalculateCenter(clusters[i]);

                    if (!newCenter.ValueEquals(centers[i]))
                    {
                        centers[i] = newCenter;
                        end = false;
                    }
                }
            }

            return clusters;
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
