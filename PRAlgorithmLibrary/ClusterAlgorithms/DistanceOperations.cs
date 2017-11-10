using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary.ClusterAlgorithms
{
    static class DistanceOperations
    {
        /// <summary>
        /// 得到向量v与centers中所有中心间最小的距离，并将距离最小的中心的索引输出到minDIndex中
        /// </summary>
        /// <param name="centers">所有中心点</param>
        /// <param name="v"></param>
        /// <param name="minDIndex"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 得到vectors中所有向量分别与centers中所有中心间最小的距离，
        /// 并将距离最小的中心的索引按顺序输出到indexes数组中
        /// </summary>
        /// <param name="centers">所有中心点</param>
        /// <param name="vectors"></param>
        /// <param name="indexes"></param>
        /// <returns></returns>
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

        public static decimal CalculateAverageDistance(Vector center, List<Vector> list)
        {


            decimal d = 0;
            foreach (var v in list)
            {
                d += center.CalculateDistance(v);
            }

            d /= list.Count;
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

        /// <summary>
        /// 把vectors中所有向量分到centers.Count个类中，采用最短距离原则
        /// </summary>
        /// <param name="centers"></param>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public static List<List<Vector>> Sort(List<Vector> centers, Vector[] vectors)
        {

            GetMinDs(centers, vectors, out int[] indexes);

            return Sort(indexes, vectors, centers.Count);
        }

        /// <summary>
        /// 已知要分为centerCount个类，vectors[i]距离第indexes[i]个中心点最近，根据以上条件生成聚类集合
        /// </summary>
        /// <param name="indexes"></param>
        /// <param name="vectors"></param>
        /// <param name="centerCount"></param>
        /// <returns></returns>
        public static List<List<Vector>> Sort(int[] indexes, Vector[] vectors, int centerCount)
        {
            List<List<Vector>> clusters = new List<List<Vector>>(centerCount);

            for (int i = 0; i < centerCount; i++)
            {
                clusters.Add(new List<Vector>());
            }

            for (int i = 0; i < indexes.Length; i++)
            {
                clusters[indexes[i]].Add(vectors[i]);
            }

            return clusters;
        }

        public static Vector CalculateCenter(List<Vector> vectors)
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

    }
}
