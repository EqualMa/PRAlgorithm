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

namespace PRAlgorithm.ClusterUserControls
{
    /// <summary>
    /// ParameterSetter.xaml 的交互逻辑
    /// </summary>
    public partial class ParameterSetter : UserControl
    {
        public Parameter _parameter;
        public Parameter Parameter
        {
            get => _parameter;
            set
            {
                _parameter = value;
                DataContext = value;
            }
        }

        public ParameterSetter()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (decimal.TryParse(ValueTextBox.Text, out decimal v))
            {
                TipText.Visibility = Visibility.Collapsed;
                Parameter.Value = v;
            }
            else
            {
                TipText.Visibility = Visibility.Visible;
            }
        }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
