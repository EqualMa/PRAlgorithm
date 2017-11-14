using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary.ClusterAlgorithms
{
    class ISODATAAlgorithm : IClusterAlgorithm
    {
        public int Nc { get; set; }
        public int Theta_N { get; set; }
        public decimal Theta_S { get; set; }
        public int K { get; set; }
        public decimal SplitK { get; set; }
        public decimal Theta_C { get; set; }
        public int I { get; set; }

        public List<string> Parameters =>
            new List<string>() { ParaName_Nc,ParaName_Theta_N, ParaName_Theta_S,
            ParaName_K,ParaName_SplitK,ParaName_Theta_C,ParaName_I};

        public void SetParameter(string paraName, decimal paraValue)
        {
            if (paraName == ParaName_Nc)
                Nc = decimal.ToInt32(paraValue);
            else if (paraName == ParaName_Theta_N)
                Theta_N = decimal.ToInt32(paraValue);
            else if (paraName == ParaName_Theta_S)
                Theta_S = paraValue;
            else if (paraName == ParaName_K)
                K = decimal.ToInt32(paraValue);
            else if (paraName == ParaName_SplitK)
                SplitK = paraValue;
            else if (paraName == ParaName_Theta_C)
                Theta_C = paraValue;
            else if (paraName == ParaName_I)
                I = decimal.ToInt32(paraValue);
            else throw new ParameterNotExistException(paraName);
        }
        public decimal GetParameterValue(string paraName)
        {
            if (paraName == ParaName_Nc)
                return Nc;
            else if (paraName == ParaName_Theta_N)
                return Theta_N;
            else if (paraName == ParaName_Theta_S)
                return Theta_S;
            else if (paraName == ParaName_K)
                return K;
            else if (paraName == ParaName_SplitK)
                return SplitK;
            else if (paraName == ParaName_Theta_C)
                return Theta_C;
            else if (paraName == ParaName_I)
                return I;
            else throw new ParameterNotExistException(paraName);
        }

        public const string ParaName_Nc = "Nc";
        public const string ParaName_Theta_N = "Theta_N";
        public const string ParaName_Theta_S = "Theta_S";
        public const string ParaName_K = "K";
        public const string ParaName_SplitK = "SplitK";
        public const string ParaName_Theta_C = "Theta_C";
        public const string ParaName_I = "I";

        public List<List<Vector>> Cluster(Vector[] vectors)
        {
            int N = vectors.Length;

            List<Vector> centers = new List<Vector>(Nc);

            List<List<Vector>> clusters = null;

            //1 预选Nc个聚类中心
            for (int i = 0; i < Nc; i++)
            {
                centers.Add(vectors[i]);
            }

            int times = 1;
            bool end2_14 = false;

            while (!end2_14)
            {
                bool end2_10 = false;

                while (!end2_10)
                {
                    end2_10 = true;

                    //2 分配所有样本
                    clusters = DistanceOperations.Sort(centers, vectors);

                    //3 取消样本数太少的类
                    clusters.RemoveAll(new Predicate<List<Vector>>((vs) => vs.Count < Theta_N));
                    Nc = clusters.Count;

                    //4 修正各聚类中心值
                    centers.Clear();
                    for (int i = 0; i < Nc; i++)
                    {
                        centers.Add(DistanceOperations.CalculateCenter(clusters[i]));
                    }

                    //5 计算类内平均距离
                    decimal[] averageDistances = new decimal[Nc];
                    for (int i = 0; i < Nc; i++)
                    {
                        averageDistances[i] = DistanceOperations.CalculateAverageDistance(centers[i], clusters[i]);
                    }

                    //6 计算全部样本的总体平均距离
                    decimal averageDistanceOfAll = 0;
                    for (int i = 0; i < Nc; i++)
                    {
                        averageDistanceOfAll += averageDistances[i] * clusters[i].Count;
                    }
                    averageDistanceOfAll /= N;



                    const int NextIs8 = 8;
                    const int NextIs11 = 11;

                    int nextStep;//下一步

                    //7 
                    //如果迭代已达I次，置Theta_C=0，跳到第11步
                    if (times >= I)
                    {
                        Theta_C = 0;
                        nextStep = NextIs11;
                    }
                    //如果Nc<=K/2，进入第八步
                    if (Nc <= K / 2)
                        nextStep = NextIs8;
                    //如果迭代次数是偶数，或Nc>=2K，跳到11；否则进入第八步。
                    if (times % 2 == 0 || Nc >= 2 * K)
                        nextStep = NextIs11;
                    else nextStep = NextIs8;

                    if (nextStep == NextIs11)
                        break;// End While 2-10

                    //8 计算每个聚类中样本的标准差向量
                    Vector[] ss = new Vector[Nc];
                    for (int i = 0; i < Nc; i++)
                    {
                        ss[i] = DistanceOperations.CalculateS(centers[i], clusters[i]);
                    }

                    //9 求每个标准差向量的最大分量对应的的维数
                    int[] sMaxDimentions = new int[Nc];
                    for (int i = 0; i < Nc; i++)
                    {
                        sMaxDimentions[i] = DistanceOperations.GetMaxValueIndex(ss[i].Numbers);
                    }

                    //10 判断是否分裂
                    for (int i = 0; i < Nc; i++)
                    {
                        if (ss[i][sMaxDimentions[i]] > Theta_S)
                        {
                            if ((averageDistances[i] > averageDistanceOfAll
                                 && clusters[i].Count > 2 * (Theta_N + 1))
                                 ||
                                 (Nc <= K / 2))
                            {
                                //分裂
                                Vector oldCenter = centers[i];

                                decimal delta = ss[i][sMaxDimentions[i]] * SplitK;
                                Vector newCenter1 = oldCenter.CloneVector();
                                newCenter1.Numbers[sMaxDimentions[i]] += delta;
                                Vector newCenter2 = oldCenter.CloneVector();
                                newCenter2.Numbers[sMaxDimentions[i]] -= delta;

                                centers[i] = newCenter1;
                                centers.Add(newCenter2);

                                Nc = centers.Count;

                                times++;
                                end2_10 = false;
                            }

                        }
                    }

                }//End While 2-10

                //11 计算所有聚类中心间的距离，
                //  (i , j) 对应 i*Nc+j ; n 对应 ( n/Nc , n%Nc )
                List<KeyValuePair<int, decimal>> ds = DistanceOperations.CalculateAllDistances(centers);

                //12 将小于Theta_C的距离升序排序
                ds.RemoveAll((kv) => { return kv.Value >= Theta_C; });
                ds.Sort((kv1, kv2) => { return decimal.Compare(kv1.Value, kv2.Value); });

                //13 合并
                List<Vector> newCenters = new List<Vector>();
                List<List<Vector>> newClusters = new List<List<Vector>>();
                foreach (var kv in ds)
                {
                    int i = kv.Key / Nc;
                    int j = kv.Key % Nc;

                    int Ni = clusters[i].Count;
                    int Nj = clusters[j].Count;
                    Vector newCenter = (centers[i] * Ni + centers[j] * Nj) / (Ni + Nj);

                    List<Vector> newCluster = new List<Vector>(Ni + Nj);
                    newCluster.AddRange(clusters[i]);
                    newCluster.AddRange(clusters[j]);

                    newCenters.Add(newCenter);
                    newClusters.Add(newCluster);

                    centers[i] = null;
                    centers[j] = null;

                    clusters[i] = null;
                    clusters[j] = null;
                }
                centers.RemoveAll((v) => v == null);
                clusters.RemoveAll((vl) => vl == null);
                Nc = clusters.Count;


                //14
                if (times >= I)
                {
                    end2_14 = true;
                }
                else
                {
                    times++;
                    end2_14 = false;
                }

            }// End While 2-14

            return clusters;
        }


    }


}

