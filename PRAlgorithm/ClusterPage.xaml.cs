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

namespace PRAlgorithm
{
    /// <summary>
    /// ClusterPage.xaml 的交互逻辑
    /// </summary>
    public partial class ClusterPage : Page
    {
        public ClusterPage()
        {
            InitializeComponent();
        }

        private void BtnHideOrShowData_CLicked(object sender, RoutedEventArgs e)
        {
            if (DataTextBox.MinLines == 1)
                DataTextBox.MinLines = 5;
            else DataTextBox.MinLines = 1;
        }

        private void BtnMaxMinDiatance_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void BtnHierarchical_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void BtnKMeans_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void BtnISODATA_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private List<PRAlgorithmLibrary.Vector> GetDataFromInput()
        {
            string[] vectors = DataTextBox.Text.Split(' ', '\n', '\t', '\r');

            return PRAlgorithmLibrary.Vector.FromStrings(vectors);
        }
    }


}
