using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary.ClusterAlgorithms
{
    static class DistanceOperations
    {
        public static decimal GetMinDistance(List<Vector> centers, Vector v, out int minDIndex)
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


        public static int GetMaxValueIndex(decimal[] minDs)
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

        public static decimal[] GetMinDs(List<Vector> centers, Vector[] vectors, out int[] indexes)
        {
            indexes = new int[vectors.Length];
            decimal[] result = new decimal[vectors.Length];

            for (int i = 0; i < vectors.Length; i++)
            {
                result[i] = GetMinDistance(centers, vectors[i], out indexes[i]);
            }

            return result;
        }

        public static int GetMostDistantVectorIndex(Vector center, Vector[] vectors)
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
