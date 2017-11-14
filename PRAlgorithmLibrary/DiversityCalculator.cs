using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary
{
    public class DiversityCalculator
    {
        /// <summary>
        /// 计算一个数据对象和另一个数据对象的相异性
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        public static decimal CalculateDiversity(Attribute[] attribute, object[] data1, object[] data2)
        {
            if (!(attribute.Length == data1.Length && attribute.Length == data2.Length))
                throw new Exception("Attribute count and data count does not match.");

            decimal DSum = 0;//分子 ：（指示符 x 相异性） 的 和
            decimal FSum = 0;//分母：指示符的和
            for (int i = 0; i < attribute.Length; i++)
            {
                bool f = true;
                //计算指示符
                if (attribute[i] is BinaryAttribute ba && ba.IsAsymmetry)
                {
                    //如果f是非对称的二元属性，且data1=data2=0，则指示符为0
                    if (ba.GetValue(data1[i].ToString()) == 0 && ba.GetValue(data2[i].ToString()) == 0)
                        f = false;
                }
                //如果 data1 或者 data2 缺失
                else if (data1[i] == null || data2[i] == null)
                    f = false;

                //计算 指示符 x 相异性
                if (f)
                {
                    DSum += CalculateDiversity(attribute[i], data1[i], data2[i]);
                    FSum++;
                }

            }

            if (DSum == 0) return 0;
            return DSum / FSum;
        }

        /// <summary>
        /// 计算单个数据和单个数据的相异性
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        public static decimal CalculateDiversity(Attribute attribute, object data1, object data2)
        {
            return attribute.CalculateDiversity(data1, data2);
        }

        public static decimal[,] CalculateDiversityMatrix(DataMatrix dataMatrix)
        {
            decimal[,] diversities;

            int count = dataMatrix.DataRows.Count;
            diversities = new decimal[count, count];

            for (int i = 0; i < count; i++)
            {
                diversities[i, i] = 0;

                for (int j = i + 1; j < count; j++)
                {
                    decimal d = CalculateDiversity(dataMatrix.Attributes, dataMatrix[i].Data, dataMatrix[j].Data);
                    diversities[i, j] = d;
                    diversities[j, i] = d;
                }
            }

            return diversities;
        }

    }
}
