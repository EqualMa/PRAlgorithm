using System;
using System.Collections.Generic;

namespace PRAlgorithmLibrary
{
    public abstract class Attribute
    {
        public Attribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        /// <summary>
        /// 计算x和y的相异性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public abstract decimal CalculateDiversity(object x, object y);
    }

    /// <summary>
    /// 序数属性
    /// </summary>
    public class OrdinalAttribute : Attribute
    {
        private Dictionary<string, int> ordinalValues;
        public OrdinalAttribute(string name, params string[] ascendingSequence) : base(name) { }
        public void SetAscendingSequence(params string[] ascendingSequence)
        {

            ordinalValues = new Dictionary<string, int>();
            for (int i = 0; i < ascendingSequence.Length; i++)
            {
                ordinalValues[ascendingSequence[i]] = i;
            }
        }
        /// <summary>
        /// 计算两个序数值的相异性
        /// 如果该序数名在该属性中没有定义，会引发KeyNotFoundException
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override decimal CalculateDiversity(object x, object y)
        {
            decimal zx = GetZ(x);
            decimal zy = GetZ(y);
            return zx > zy ? zx - zy : zy - zx;
        }

        /// <summary>
        /// 得到序数对应的数值，在0到1之间
        /// </summary>
        public decimal GetZ(object i)
        {
            decimal r = ordinalValues[i.ToString()];
            return r / (ordinalValues.Count - 1);
        }
    }

    /// <summary>
    /// 标称属性
    /// </summary>
    public class NorminalAttribute : Attribute
    {
        public List<string> Norminals { get; private set; }
        public NorminalAttribute(string name) : base(name)
        {
            Norminals = new List<string>();
        }

        /// <summary>
        /// 标称属性计算相异性，相同为0，不同为1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override decimal CalculateDiversity(object x, object y)
        {
            string xs = x.ToString();
            string ys = y.ToString();
            if (Norminals.Contains(xs))
            {
                if (Norminals.Contains(ys))
                {
                    if (xs == ys) return 0;
                    else return 1;
                }
                else throw new KeyNotFoundException(ys);

            }
            else throw new KeyNotFoundException(xs);
        }
    }

    /// <summary>
    /// 二元属性
    /// </summary>
    public class BinaryAttribute : Attribute
    {
        /// <summary>
        /// 是非对称的二元属性
        /// </summary>
        public bool IsAsymmetry { get; set; }

        public string Value0 { get; set; }

        public string Value1 { get; set; }

        public BinaryAttribute(string name, string value0 = "0", string value1 = "1", bool asymmetry = false) : base(name) { }

        /// <summary>
        /// 二元属性计算相异性，相同为0，不同为1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override decimal CalculateDiversity(object x, object y)
        {
            return GetValue(x.ToString()) == GetValue(y.ToString()) ? 0 : 1;
        }

        public int GetValue(string x)
        {
            if (x == Value0) return 0;
            if (x == Value1) return 1;
            throw new KeyNotFoundException(x);
        }
    }

    public class NumericalAttribute : Attribute
    {
        public NumericalAttribute(string name) : base(name) { }

        private decimal _maxMinusMin = NotAssigned;
        public decimal MaxMinusMin
        {
            get { return _maxMinusMin; }
            set
            {
                if (value < 0) throw new Exception("Max - Min can not be negative.");
                _maxMinusMin = value;
            }
        }

        public const decimal NotAssigned = -1;

        /// <summary>
        /// 数值类型相异性是两个向量间的欧式距离与所有数据中最大距离的商
        /// x和y应是decimal或double或int类型，或者是相同维度的Vector对象
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override decimal CalculateDiversity(object x, object y)
        {
            return CalculateDistance(x, y) / MaxMinusMin;
        }

        public static decimal CalculateDistance(object x, object y)
        {
            if (x is Vector vx && y is Vector vy)
            {
                return vx.CalculateDistance(vy);
            }
            else
            {
                decimal dx = GetDecimalFromIntOrDouble(x);
                decimal dy = GetDecimalFromIntOrDouble(y);
                return dx > dy ? dx - dy : dy - dx;
            }

        }

        public static decimal GetDecimalFromIntOrDouble(object x)
        {
            if (x is int ix)
                return new decimal(ix);
            else if (x is double doublex)
                return new decimal(doublex);
            else throw new Exception("Type " + x.GetType() + " not allowed: " + x);
        }

    }


}
