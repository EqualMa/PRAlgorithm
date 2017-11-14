using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary
{
    public class DataRow
    {
        public Attribute[] Attributes { get; }

        private Dictionary<Attribute, object> dataDictionary;

        public object[] Data { get; }

        public DataRow(Attribute[] attributes, object[] data)
        {
            if (data.Length != attributes.Length)
                throw new Exception(String.Format(
                    "This row of data has {0} attributes, but there are {1} columns of data.",
                    attributes.Length, data.Length));

            Attributes = attributes;
            Data = data;
        }

        public object this[int col]
        {
            get => Data[col];
            set => Data[col] = value;
        }
    }
}
