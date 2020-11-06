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
    /// 只使用一次，每次登录时，我们会将更新信息保存。
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
            this.TipLabel.Foreground= new SolidColorBrush(Colors.Red);
            this.TipLabel.FontSize = 20;
        }

        public void HidePage()
        {
            this.Visibility= Visibility.Collapsed;
            //HideProgressBar();
            //this.TipLabel.Visibility= Visibility.Collapsed;
        }

        public void HideProgressBar()
        {
            CircularProgress.Visibility = Visibility.Collapsed;
        }

        public void RestorePage()
        {
            CircularProgress.Visibility = Visibility.Visible;
        }
    }
}
