﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary
{
    public class Vector
    {
        public Vector(int dimension)
        {
            Dimension = dimension;
            Numbers = new decimal[dimension];
        }

        public int Dimension { get; }
        public decimal[] Numbers { get; }

        public decimal this[int dimension]
        {
            get { return Numbers[dimension]; }
            set { Numbers[dimension] = value; }
        }

        /// <summary>
        /// 计算两个向量间的欧氏距离
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static decimal CalculateDistance(Vector v1, Vector v2)
        {
            CheckDimentionMatch(v1, v2);

            if (v1 == v2) return 0;

            if (v1.Dimension == 1)
                return v1[0] > v2[0] ? v1[0] - v2[0] : v2[0] - v1[0];


            decimal result = 0;

            for (int i = 0; i < v1.Dimension; i++)
            {
                decimal d = v1[i] - v2[i];
                result += decimal.Multiply(d, d);
            }

            return new decimal(Math.Sqrt(decimal.ToDouble(result)));

        }

        public bool ValueEquals(Vector vector)
        {
            if (this == vector) return true;

            try
            {
                int d = CheckDimentionMatch(this, vector);

                for (int i = 0; i < d; i++)
                {
                    if (this[i] != vector[i])
                        return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static int CheckDimentionMatch(Vector v1, Vector v2)
        {
            if (v1.Dimension != v2.Dimension)
                throw new Exception("Vectors are not matched in dimention!");

            return v1.Dimension;
        }

        public decimal CalculateDistance(Vector v2)
        {
            return CalculateDistance(this, v2);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            int d = CheckDimentionMatch(v1, v2);

            Vector r = new Vector(d);

            for (int i = 0; i < d; i++)
            {
                r[i] = v1[i] + v2[i];
            }

            return r;
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            int d = CheckDimentionMatch(v1, v2);

            Vector r = new Vector(d);

            for (int i = 0; i < d; i++)
            {
                r[i] = v1[i] - v2[i];
            }

            return r;
        }

        public static Vector operator *(Vector v, decimal n)
        {
            int d = v.Dimension;

            Vector r = new Vector(d);

            for (int i = 0; i < d; i++)
            {
                r[i] = v[i] * n;
            }

            return r;
        }
        public static Vector operator /(Vector v, decimal n)
        {
            int d = v.Dimension;

            Vector r = new Vector(d);

            for (int i = 0; i < d; i++)
            {
                r[i] = v[i] / n;
            }

            return r;
        }

        public static Vector GetZeroVector(int dimension)
        {
            Vector r = new Vector(dimension);
            for (int i = 0; i < dimension; i++)
            {
                r[dimension] = 0;
            }

            return r;
        }

        public Vector CloneVector()
        {
            Vector newV = new Vector(Dimension);

            for (int i = 0; i < Dimension; i++)
            {
                newV[i] = Numbers[i];
            }

            return newV;
        }

        /// <summary>
        /// 把"d1, d2, ..., dn"字符串转换成Vector对象
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector FromString(string s)
        {
            var ds = s.Split(',');
            Vector r = new Vector(ds.Length);

            for (int i = 0; i < ds.Length; i++)
            {
                r[i] = decimal.Parse(ds[i].Trim());
            }

            return r;
        }

        public static List<Vector> FromStrings(string[] inputs)
        {
            List<Vector> vlist = new List<Vector>(inputs.Length);

            foreach (var v in inputs)
            {
                string vt = v.Trim();
                if (!string.IsNullOrEmpty(vt))
                {
                    vlist.Add(PRAlgorithmLibrary.Vector.FromString(vt));
                }
            }

            return vlist;
        }

        public override string ToString()
        {
            if (Dimension == 1)
                return "( " + Numbers[0] + " )";

            StringBuilder builder = new StringBuilder();

            builder.Append("( ");
            for (int i = 0; i < Numbers.Length - 1; i++)
            {
                builder.AppendFormat("{0}, ", Numbers[i]);
            }

            builder.AppendFormat("{0} )", Numbers[Numbers.Length - 1]);

            return builder.ToString();
        }
    }
}
