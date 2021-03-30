using Launcher.Domain;
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
    public class ProjectInfo
    {
        public string txt { get; set; }
        public string timeCreate { get; set; }
        public string projectName { get; set; }
    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TreesViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = System.Windows.SystemParameters.WorkArea.Height;

            this.InitializeComponent();
            List<ProjectInfo> pjs = new List<ProjectInfo>() {
                new ProjectInfo() { txt = "选项1", timeCreate="2010-03-02 14:00", projectName="just a test prj"},
                new ProjectInfo() { txt = "选项2", timeCreate="2010-03-02 14:00", projectName="just a test prj"},
                new ProjectInfo() { txt = "选项3", timeCreate="2010-03-02 14:00", projectName="just a test prj"},
                new ProjectInfo() { txt = "选项4", timeCreate="2010-03-02 14:00", projectName="just a test prj"},
                new ProjectInfo() { txt = "选项5", timeCreate="2010-03-02 14:00", projectName="just a test prj"},
                new ProjectInfo() { txt = "选项6", timeCreate="2010-03-02 14:00", projectName="just a test prj"},
                new ProjectInfo() { txt = "选项7", timeCreate="2010-03-02 14:00", projectName="just a test prj"}
             };
           //var sl= this.scrList;
            this.itemsControl.ItemsSource = pjs;

            _viewModel = new TreesViewModel();
            DataContext = _viewModel;

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

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
