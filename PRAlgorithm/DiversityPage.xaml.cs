using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using PRAlgorithmLibrary;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace PRAlgorithm
{
    /// <summary>
    /// DiversityPage.xaml 的交互逻辑
    /// </summary>
    public partial class DiversityPage : Page
    {
        public DiversityPage()
        {
            InitializeComponent();
        }

        public void ReadFromXlsx(string path)
        {
            Excel.Application xlApp = new Excel.Application();

            var xlWorkBook = xlApp.Workbooks.Open(path, Editable: true);

            var sheet = (Excel.Worksheet)xlWorkBook.Sheets.Item[1];


            var range = sheet.UsedRange;
            var rowCount = range.Rows.Count;
            var colCount = range.Columns.Count;

            int dataCount = rowCount - 1;


            if (rowCount < 1)
            {
                ShowTips(USAGE);
                return;
            }

            Data = new DataMatrix(colCount);
            AttributesNames = new string[colCount];

            for (int i = 1; i <= colCount; i++)
            {

                AttributesNames[i - 1] = (range.Cells[1, i] as Excel.Range).Value2;
            }

            for (int i = 2; i <= rowCount; i++)
            {
                object[] data = new object[colCount];
                for (int j = 1; j <= colCount; j++)
                {
                    data[j - 1] = (range.Cells[i, j] as Excel.Range).Value2; ;
                }
                Data.AddData(data);
            }

            xlWorkBook.Close();
            xlApp.Quit();

        }


        public const string USAGE =
            "请将Excel文件中的第一个sheet用于存放数据，第一行是所有属性的名称，" +
                "下面所有行是一行一行的数据。不要增加其他行。";
        public void ShowTips(string tips)
        {
            TipsTextBlock.Text = tips;

        }
        public void ClearTips()
        {
            ShowTips("");
        }

        public DataMatrix Data { get; set; }
        public string[] AttributesNames { get; set; }

        public DataTable DataTable { get; set; }



        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Excel文档|*.xlsx",
                DefaultExt = "xlsx"
            };

            if (dialog.ShowDialog() is bool result && result)
            {
                TextBlockFilePath.Text = dialog.FileName;
                try
                {
                    ReadFromXlsx(dialog.FileName);
                    ShowAttriabutes();
                    ClearTips();
                }
                catch (Exception excp)
                {
                    ShowTips(excp.Message + Environment.NewLine + USAGE);
                }
            }


        }

        private void ShowAttriabutes()
        {
            AttributePanel.Children.Clear();

            for (int i = 0; i < AttributesNames.Length; i++)
            {
                AttributePanel.Children.Add(CreateAttributeOption(i));

            }

        }


        private StackPanel CreateAttributeOption(int i)
        {
            var sp = new StackPanel
            {
                Background = new SolidColorBrush(Colors.LightGray),

                Margin = new Thickness(5)
            };

            TextBlock nameText = new TextBlock() { Text = AttributesNames[i] };

            ComboBox cb = new ComboBox();
            cb.Items.Add("二元属性");
            cb.Items.Add("标称属性");
            cb.Items.Add("序数属性");
            cb.Items.Add("数值属性");
            cb.SelectionChanged += AttributeType_SelectionChanged;

            StackPanel childSP = new StackPanel();

            sp.Children.Add(nameText);
            sp.Children.Add(cb);
            sp.Children.Add(childSP);

            return sp;
        }

        private void AttributeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (sender as ComboBox);

            if (cb.SelectedItem == null)
                return;

            StackPanel fsp = cb.Parent as StackPanel;
            var sp = fsp.Children[2] as StackPanel;
            int i = GetIndexOfStackPanel(fsp);

            try
            {
                ClearTips();
                switch (cb.SelectedIndex)
                {
                    case 0:
                        var ba = new BinaryAttribute(AttributesNames[i]);
                        Data.Attributes[i] = ba;
                        ba.SetValueFromData(Data[ba].RowsOfData);
                        ShowAttributeParas(Data.Attributes[i] as BinaryAttribute, sp);
                        break;
                    case 1:
                        var nora = new NorminalAttribute(AttributesNames[i]);
                        Data.Attributes[i] = nora;
                        nora.SetNorminalsFromData(Data[nora].RowsOfData);
                        sp.Children.Clear();
                        break;
                    case 2:
                        Data.Attributes[i] = new OrdinalAttribute(AttributesNames[i]);
                        ShowAttributeParas(Data.Attributes[i] as OrdinalAttribute, sp);
                        break;
                    case 3:
                        var na = new NumericalAttribute(AttributesNames[i]);
                        Data.Attributes[i] = na;
                        na.SetMaxMinuxMinFromData(Data[na].RowsOfData);
                        sp.Children.Clear();
                        break;
                }

            }
            catch (Exception excp)
            {
                ShowTips(excp.Message);
                cb.SelectedItem = null;
            }

        }

        private int GetIndexOfStackPanel(StackPanel sp)
        {
            return AttributePanel.Children.IndexOf(sp);
        }

        private void ShowAttributeParas(BinaryAttribute ba, StackPanel sp)
        {
            CheckBox cb = new CheckBox
            {
                Content = "非对称",
                IsChecked = ba.IsAsymmetry
            };


            cb.Checked += (s, e) => { ba.IsAsymmetry = true; };
            cb.Unchecked += (s, e) => { ba.IsAsymmetry = false; };

            TextBlock t0 = new TextBlock() { Text = "值为0的字符串" };

            TextBlock tb0 = new TextBlock() { Text = ba.Value0 };

            TextBlock t1 = new TextBlock() { Text = "值为1的字符串" };

            TextBlock tb1 = new TextBlock() { Text = ba.Value1 }; ;

            Button switchButton = new Button() { Content = "对换" };

            switchButton.Click += (s, e) =>
            {
                ba.SwitchValue();
                tb0.Text = ba.Value0;
                tb1.Text = ba.Value1;
            };

            sp.Children.Clear();
            sp.Children.Add(cb);
            sp.Children.Add(t0);
            sp.Children.Add(tb0);
            sp.Children.Add(t1);
            sp.Children.Add(tb1);

            sp.Children.Add(switchButton);
        }

        private void ShowAttributeParas(OrdinalAttribute oa, StackPanel sp)
        {

            TextBlock t0 = new TextBlock() { Text = "升序输入各枚举类型，以回车分隔" };

            TextBox tb0 = new TextBox() { AcceptsReturn = true };
            tb0.TextChanged += (s, e) =>
            {
                var ss = tb0.Text.Split('\n');

                for (int i = 0; i < ss.Length; i++)
                {
                    ss[i] = ss[i].Trim();
                }

                oa.SetAscendingSequence(ss);
            };


            sp.Children.Clear();
            sp.Children.Add(t0);
            sp.Children.Add(tb0);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearTips();
                ResultTextBlock.Text = GetDiversity();

            }
            catch (Exception excp)
            {
                ShowTips(excp.Message);
            }
        }

        private string GetDiversity()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var dm = DiversityCalculator.CalculateDiversityMatrix(Data);

            int maxWidth = (dm.GetLength(0) - 1).ToString().Length + 3;

            //第-1行第-1列
            stringBuilder.Append("\t".PadLeft(dm.GetLength(0).ToString().Length + 2));

            for (int i = 0; i < dm.GetLength(1); i++)
            {
                //第-1行第i列
                stringBuilder.AppendFormat("{0}\t", i);
            }
            stringBuilder.AppendLine();

            for (int i = 0; i < dm.GetLength(0); i++)
            {
                stringBuilder.AppendFormat("{0} |\t", i.ToString().PadRight(maxWidth));
                for (int j = 0; j < dm.GetLength(1); j++)
                {
                    stringBuilder.Append(decimal.Round(dm[i, j], 2, MidpointRounding.AwayFromZero));
                    stringBuilder.Append("\t");
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}


