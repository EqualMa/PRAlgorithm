using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary
{
    public class DataColumn
    {
        public DataColumn(Attribute attribute, object[] rowsOfData)
        {
            Attribute = attribute;
            RowsOfData = rowsOfData;
        }

        public Attribute Attribute { get; private set; }

        public Object[] RowsOfData { get; private set; }
    }
}
