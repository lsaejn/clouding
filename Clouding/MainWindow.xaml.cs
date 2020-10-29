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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /*
         * 没有考虑多屏幕
         */
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton==MouseButtonState.Pressed)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    Point pt = Mouse.GetPosition(e.Source as FrameworkElement);
                    pt=PointToScreen(pt);
                    this.WindowState = WindowState.Normal;
                    var w=this.Width;
                    var h = this.Height;
                    if (pt.X < w-120)
                    {
                        this.Left = 0;
                        this.Top = 0;
                    }
                    else if (pt.X > w + 120)
                    {
                        this.Left = SystemParameters.PrimaryScreenWidth - w;
                        this.Top = 0;
                    }
                    else
                    {
                        this.Left = (SystemParameters.PrimaryScreenWidth - w) / 2;
                        this.Top = 0;
                    }
                }
                Point pp = Mouse.GetPosition(e.Source as FrameworkElement);
                if (pp.Y<40)
                    DragMove();
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ?WindowState.Normal : WindowState.Maximized;
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void OnClickUpdateVersion(object sender, RoutedEventArgs e)
        {
           
        }

        private void OnClickPatching(object sender, RoutedEventArgs e)
        {

        }

        private void OnClickMinBtn(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnClickMaxBtn(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void OnClickSettingBtn(object sender, RoutedEventArgs e)
        {

        }
    }

}
