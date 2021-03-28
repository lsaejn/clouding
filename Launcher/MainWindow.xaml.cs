using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = System.Windows.SystemParameters.WorkArea.Height;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("fff");
        }

        private void Title_LButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ApplySkin(object sender, RoutedEventArgs e)
        {
            string skinDictPath = ".\\styles\\Dark.xaml";
            Uri skinDictUri = new Uri(skinDictPath, UriKind.Relative);

            // Tell the Application to load the skin resources.
            Launcher.App app = Application.Current as Launcher.App;
            // Load the ResourceDictionary into memory.
            ResourceDictionary skinDict =
              Application.LoadComponent(skinDictUri) as ResourceDictionary;

            //我们加载到windowResourse里
            Collection<ResourceDictionary> mergedDicts =
              base.Resources.MergedDictionaries;

            // Remove the existing skin dictionary, if one exists.
            // NOTE: In a real application, this logic might need
            // to be more complex, because there might be dictionaries
            // which should not be removed.
            if (mergedDicts.Count > 0)
                mergedDicts.Clear();

            // Apply the selected skin so that all elements in the
            // application will honor the new look and feel.
            mergedDicts.Add(skinDict);

            //这里演示加载到app(App.xaml)的MergedDictionaries
            //Application.Current.Resources.MergedDictionaries.Add(skinDict);

            ResourceDictionary rd = new ResourceDictionary();
            //pack://application:,,,/Launcher;component/styles/Blue.xaml
            //rd.Source = new Uri(@"/Launcher;Component/styles/Dark.xaml", UriKind.RelativeOrAbsolute);
            //this.Resources.MergedDictionaries.Add(rd);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
