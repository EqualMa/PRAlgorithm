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
using PRAlgorithmLibrary.ClusterAlgorithms;
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
            CreateNewAlgorithmPanel(new MaxMinDiatanceAlgorithm());
        }

        private void BtnHierarchical_Clicked(object sender, RoutedEventArgs e)
        {
            CreateNewAlgorithmPanel(new HierarchicalClusteringAlgorithm());

        }

        private void BtnKMeans_Clicked(object sender, RoutedEventArgs e)
        {
            CreateNewAlgorithmPanel(new KMeansAlgorithm());

        }

        private void BtnISODATA_Clicked(object sender, RoutedEventArgs e)
        {
            CreateNewAlgorithmPanel(new ISODATAAlgorithm());

        }

        private List<PRAlgorithmLibrary.Vector> GetDataFromInput()
        {
            string[] vectors = DataTextBox.Text.Split(' ', '\n', '\t', '\r');

            return PRAlgorithmLibrary.Vector.FromStrings(vectors);
        }

        private void CreateNewAlgorithmPanel(PRAlgorithmLibrary.ClusterAlgorithms.IClusterAlgorithm algorithm)
        {
            var resultUIElement = new ClusterUserControls.ClusterResultDisplay()
            {
                ClusterAlgorithm = algorithm,
                InputVectors = GetDataFromInput()
            };
            resultUIElement.CloseButtonClicked += ResultUIElement_CloseButtonClicked;
            ResultsPanel.Children.Add(resultUIElement);
        }

        private void ResultUIElement_CloseButtonClicked(ClusterUserControls.ClusterResultDisplay obj)
        {
            ResultsPanel.Children.Remove(obj);
        }
    }


}
