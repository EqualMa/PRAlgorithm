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
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {
        private ClusterPage clusterPage;

        private DiversityPage diversityPage;

        public HomePage()
        {
            InitializeComponent();
        }

        private void btnToCalculateDiversity_Click(object sender, RoutedEventArgs e)
        {
            if (diversityPage == null) diversityPage = new DiversityPage();
            this.NavigationService.Navigate(diversityPage);
        }

        private void btnToCluster_Click(object sender, RoutedEventArgs e)
        {
            if (clusterPage == null) clusterPage = new ClusterPage();
            this.NavigationService.Navigate(clusterPage);
        }
    }
}
