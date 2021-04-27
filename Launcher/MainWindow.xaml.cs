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
    public partial class MainWindow : Window
    {
        
        //fix me, should not expose
        private MainWindowViewModel mvm;
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = System.Windows.SystemParameters.WorkArea.Height;
            //DemoItemsListBox.ItemsSource = ShortCutItems;

            List<Project> pjs = new List<Project>() {
                new Project() { txt = "选项1", timeCreate="2010-03-02 14:00", projectName="just a1 test prj"},
                new Project() { txt = "选项2", timeCreate="2010-03-02 14:00", projectName="just a2 test prj"},
                new Project() { txt = "选项3", timeCreate="2010-03-02 14:00", projectName="just a3 test prj"},
                new Project() { txt = "选项4", timeCreate="2010-03-02 14:00", projectName="just a4 test prj"},
                new Project() { txt = "选项5", timeCreate="2010-03-02 14:00", projectName="just a5 test prj"},
                new Project() { txt = "选项6", timeCreate="2010-03-02 14:00", projectName="just a6 test prj"},
                new Project() { txt = "选项7", timeCreate="2010-03-02 14:00", projectName="just a7 test prj"}
             };
           //var sl= this.scrList;
            this.itemsControl.ItemsSource = pjs;
            //this.NaviBar.ItemsSource=
            DataContext = new MainWindowViewModel();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //_viewModel.ProfessionalCategories = null;
            //_viewModel = null;
            this.treeView.DataContext= new TreesViewModel(null);
        }

        private void StackPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            //mvm.SelectedProject = ()sender;
            var panel=(StackPanel)sender;
            var p=(Project)panel.DataContext;
            mvm.SelectedProject = p;
            //mvm.SelectedProject.ProjectName = p.ProjectName;
            //mvm._selectedProjectString = p.projectName;
        }

        private void OnShortCut(object sender, MouseButtonEventArgs e)
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

        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                scrollViewer.LineUp();
            else
                scrollViewer.LineDown();
            if (e.Delta < 0)
        }
    }
}
