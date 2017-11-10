using System;
using System.Collections.Generic;
using System.Text;

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



        private List<List<Vector>> Sort(int[] indexes, Vector[] vectors, int centerCount)
        {
            List<List<Vector>> result = new List<List<Vector>>(centerCount);

            for (int i = 0; i < result.Count; i++)
            {
                result[i] = new List<Vector>();
            }

            for (int i = 0; i < indexes.Length; i++)
            {
                result[indexes[i]].Add(vectors[i]);
            }

            return result;
        }


        private decimal GetMinDistance(List<Vector> centers, Vector v, out int minDIndex)
        {
            decimal minD = decimal.MaxValue;
            int index = -1;
            for (int i = 0; i < centers.Count; i++)
            {
                decimal d = v.CalculateDistance(centers[i]);
                if (d < minD) { minD = d; index = i; };
            }
            minDIndex = index;
            return minD;
        }

        private static bool JudgeEnd(decimal maxD, decimal z, decimal theta)
        {
            //所有最小距离中的最大距离小于阈值则结束
            return maxD < z * theta;
        }

        private int GetMaxValueIndex(decimal[] minDs)
        {
            if (minDs.Length == 0) return -1;

            int index = 0;
            decimal maxValue = minDs[0];
            for (int i = 1; i < minDs.Length; i++)
            {
                if (maxValue < minDs[i])
                {
                    index = i;
                    maxValue = minDs[i];
                }
            }

            return index;
        }

        private decimal[] GetMinDs(List<Vector> centers, Vector[] vectors, out int[] indexes)
        {
            indexes = new int[vectors.Length];
            decimal[] result = new decimal[vectors.Length];

            for (int i = 0; i < vectors.Length; i++)
            {
                result[i] = GetMinDistance(centers, vectors[i], out indexes[i]);
            }

            return result;
        }

        private int GetMostDistantVectorIndex(Vector center, Vector[] vectors)
        {
            int index = -1;
            decimal distance = 0;
            for (int i = 0; i < vectors.Length; i++)
            {
                if (vectors[i] == center) continue;

                decimal di = vectors[i].CalculateDistance(center);

                if (di > distance) { distance = di; index = i; }
            }

            return index;
        }


    }
}
