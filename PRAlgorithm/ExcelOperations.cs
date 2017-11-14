using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
namespace PRAlgorithm
{
    static class ExcelOperations
    {
        private static Excel.Application xlApp = new Excel.Application();

        public static Excel.Worksheet OpenFirstSheet(string filename)
        {

            var xlWorkBook = xlApp.Workbooks.Open(filename, Editable: true);
            return (Excel.Worksheet)xlWorkBook.Sheets[0];
        }
    }
}
