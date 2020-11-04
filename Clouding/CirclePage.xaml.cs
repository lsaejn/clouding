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

namespace Clouding
{
    /// <summary>
    /// FixsPackagePage.xaml 的交互逻辑
    /// </summary>
    public partial class CirclePage : Page
    {
        public CirclePage()
        {
            InitializeComponent();
        }

        public void SetTip(string str)
        {
            this.TipLabel.Content = str;
        }

        public void HidePage()
        {
            this.Height = 0;
        }

        public void HideProgressBar()
        {
            CircularProgress.Visibility = Visibility.Hidden;
        }

        public void RestorePage()
        {
            this.Height = double.NaN;//fix me...
        }
    }
}
