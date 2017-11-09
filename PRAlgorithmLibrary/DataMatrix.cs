using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary
{
    class DataMatrix
    {

        public Attribute[] Attributes { get; private set; }

        public DataMatrix(int attributeCount)
        {
            Attributes = new Attribute[attributeCount];

            DataRows = new List<DataRow>();
        }

        public List<DataRow> DataRows { get; set; }

        public DataRow this[int rowIndex]
        {
            get { return DataRows[rowIndex]; }
        }

        public DataColumn this[Attribute attribute]
        {
            get { return GetColumn(attribute); }
        }

        private DataColumn GetColumn(Attribute attribute)
        {
            if (!HasAttribute(attribute))
                return null;

            object[] data = new object[DataRows.Count];

            int attributeIndex = GetAttributeIndex(attribute);
            if (attributeIndex < 0) return null;

            int i = 0;
            foreach (var r in DataRows)
            {
                data[i] = r[attributeIndex];
                i++;
            }

            return new DataColumn(attribute, data);
        }

        public bool HasAttribute(Attribute attribute)
        {
            foreach (var a in Attributes)
            {
                if (a == attribute) return true;
            }

            return false;
        }

        public int GetAttributeIndex(Attribute attribute)
        {
            for (int i = 0; i < Attributes.Length; i++)
            {
                if (Attributes[i] == attribute)
                    return i;
            }
            return -1;
        }

        public void AddData(object[] data)
        {
            DataRows.Add(new DataRow(Attributes, data));
        }
    }
}
