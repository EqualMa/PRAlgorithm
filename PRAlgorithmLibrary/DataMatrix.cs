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

            int i = 0;
            foreach (var d in DataRows)
            {
                data[i] = d[attribute];
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

        public void AddData(object[] data)
        {
            DataRows.Add(new DataRow(Attributes, data));
        }
    }
}
