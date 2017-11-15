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
    /// ClusterResultDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class ClusterResultDisplay : UserControl
    {
        private List<PRAlgorithmLibrary.Vector> _inputVectors;

        public List<PRAlgorithmLibrary.Vector> InputVectors
        {
            get => _inputVectors;
            set
            {
                _inputVectors = value;
                InputVectorChanged();
            }
        }

        public PRAlgorithmLibrary.ClusterAlgorithms.IClusterAlgorithm _clusterAlgorithm;
        public PRAlgorithmLibrary.ClusterAlgorithms.IClusterAlgorithm ClusterAlgorithm
        {
            get => _clusterAlgorithm;
            set
            {
                _clusterAlgorithm = value;
                ClusterAlgorithmChanged();
            }
        }

        public bool CanBeVisible
        {
            get
            {
                if (InputVectors != null && InputVectors.Count > 0)
                    return InputVectors[0].Dimension == 2;
                return false;
            }
        }

        public ClusterResultDisplay()
        {
            InitializeComponent();
        }

        private void BtnCluster_Clicked(object sender, RoutedEventArgs e)
        {
            //计算结果
            var clusters = ClusterAlgorithm.Cluster(InputVectors.ToArray());

            //显示结果为文本
            ResultTextBlock.Text = GetClusterResultString(clusters);

            //显示可视化结果
            if (!CanBeVisible)
            {
                TipsTextBox.Visibility = Visibility.Visible;
                return;
            }
            TipsTextBox.Visibility = Visibility.Collapsed;
            //TODO show 2d

        }

        private void InputVectorChanged()
        {
            InputTextBlock.Text = GetVectorsString(InputVectors);
        }
        private void ClusterAlgorithmChanged()
        {
            ParameterSetters.Children.Clear();
            foreach (var p in ClusterAlgorithm.Parameters)
            {
                ParameterSetters.Children.Add(new ParameterSetter()
                {
                    Parameter = new Parameter() { Name = p, Value = ClusterAlgorithm.GetParameterValue(p) }
                });
            }
        }

        private static string GetVectorsString(List<PRAlgorithmLibrary.Vector> vectors)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var v in vectors)
            {
                builder.AppendFormat("{0}\t", v.ToString());
            }
            return builder.ToString();
        }

        private static string GetClusterResultString(List<List<PRAlgorithmLibrary.Vector>> clusters)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var cluster in clusters)
            {
                builder.AppendLine("[");

                builder.AppendLine(GetVectorsString(cluster));

                builder.AppendLine("]");

            }

            return builder.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseButtonClicked(this);
        }

        public event Action<ClusterResultDisplay> CloseButtonClicked;
    }


}
