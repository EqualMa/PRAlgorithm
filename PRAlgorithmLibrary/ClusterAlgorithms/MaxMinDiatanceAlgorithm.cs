using System;
using System.Collections.Generic;
using System.Text;

using static PRAlgorithmLibrary.ClusterAlgorithms.DistanceOperations;

namespace PRAlgorithmLibrary.ClusterAlgorithms
{
    //最大最小距离聚类算法
    class MaxMinDiatanceAlgorithm : IClusterAlgorithm
    {
        public List<List<Vector>> Cluster(Vector[] vectors)
        {
            if (vectors.Length == 0) return new List<List<Vector>>();

            List<Vector> centers = new List<Vector>
            {
                vectors[0],

                vectors[GetMostDistantVectorIndex(vectors[0], vectors)]
            };

            decimal Z = centers[0].CalculateDistance(centers[1]);
            int[] minDCenterIndexes;
            while (true)
            {
                decimal[] minDs = GetMinDs(centers, vectors, out minDCenterIndexes);

                int maxDInMinDsIndex = GetMaxValueIndex(minDs);

                if (JudgeEnd(minDs[maxDInMinDsIndex], Z, Theta))
                    break;
            }

            return Sort(minDCenterIndexes, vectors, centers.Count);
        }

        public void SetParameter(string paraName, decimal paraValue = 0.5M)
        {
            if (paraName == ParaName_theta)
                Theta = paraValue;
            else throw new ParameterNotExistException(paraName);
        }

        public const string ParaName_theta = "theta";

        public decimal Theta { get; set; } = 0.5M;




        private static bool JudgeEnd(decimal maxD, decimal z, decimal theta)
        {
            //所有最小距离中的最大距离小于阈值则结束
            return maxD < z * theta;
        }




    }
}
