using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary.ClusterAlgorithms
{
    class HierarchicalClusteringAlgorithm : IClusterAlgorithm
    {
        public List<List<Vector>> Cluster(Vector[] vectors)
        {
            List<List<Vector>> result = new List<List<Vector>>(vectors.Length);

            for (int i = 0; i < vectors.Length; i++)
            {
                result.Add(new List<Vector>() { vectors[i] });
            }

            while (true)
            {
                //计算距离最短的两类
                decimal minD = GetMinDistance(result, out int vl1, out int vl2);

                //合并这两类
                result[vl1].AddRange(result[vl2]);
                result.RemoveAt(vl2);

                //判断是否结束
                if (JudgeEnd(minD, T))
                    break;
            }

            return result;
        }

        private decimal GetMinDistance(List<List<Vector>> result, out int vl1, out int vl2)
        {
            decimal minD = decimal.MaxValue;
            int mini = -1;
            int minj = -1;

            for (int i = 0; i < result.Count; i++)
            {
                for (int j = i + 1; j < result.Count; j++)
                {
                    decimal d = FuncToCalculateDistanceBetweenClusters(result[i], result[j]);

                    if (d < minD)
                    {
                        minD = d;
                        mini = i;
                        minj = j;
                    }
                }

            }

            vl1 = mini;
            vl2 = minj;
            return minD;
        }

        private static bool JudgeEnd(decimal minD, decimal t)
        {
            //当minD超过距离阈值T时，停止
            return minD >= t;
        }

        public void SetParameter(string paraName, decimal paraValue)
        {
            if (paraName == ParaName_T)
                T = paraValue;
            else throw new ParameterNotExistException(paraName);
        }

        /// <summary>
        /// 计算聚类间距离的函数，默认是使用最短距离法。
        /// </summary>
        public Func<List<Vector>, List<Vector>, decimal> FuncToCalculateDistanceBetweenClusters { get; set; }
            = CalculateDistanceBetweenClustersUsingMin;

        public const string ParaName_T = "T";

        /// <summary>
        /// 距离阈值
        /// </summary>
        public decimal T { get; set; }

        /// <summary>
        /// 使用最短距离法计算两个聚类间的距离
        /// </summary>
        /// <param name="vectors1"></param>
        /// <param name="vectors2"></param>
        /// <returns></returns>
        public static decimal CalculateDistanceBetweenClustersUsingMin(List<Vector> vectors1, List<Vector> vectors2)
        {
            if (vectors1.Count == 1)
            {
                if (vectors2.Count == 1)
                {
                    return vectors1[0].CalculateDistance(vectors2[0]);
                }
                else
                {
                    decimal d1 = CalculateDistanceBetweenClustersUsingMin(vectors1, vectors2.GetRange(0, 1));
                    decimal d2 = CalculateDistanceBetweenClustersUsingMin(vectors1, vectors2.GetRange(1, vectors2.Count - 1));
                    return d1 > d2 ? d2 : d1;
                }
            }
            else
            {
                if (vectors2.Count == 1)
                {
                    return CalculateDistanceBetweenClustersUsingMin(vectors2, vectors1);
                }
                else
                {
                    decimal d1 = CalculateDistanceBetweenClustersUsingMin(vectors1.GetRange(0, 1), vectors2);
                    decimal d2 = CalculateDistanceBetweenClustersUsingMin(vectors1.GetRange(1, vectors1.Count - 1), vectors2);
                    return d1 > d2 ? d2 : d1;
                }
            }
        }
    }
}
