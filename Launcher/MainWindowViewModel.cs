using Launcher.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    /// <summary>
    /// 快捷键
    /// </summary>
    public class ShortCutItem
    {
        public string Name { get; set; }
    }

    
    /// <summary>
    /// 工程数据
    /// </summary>
    public class Project : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string txt { get; set; }
        public string timeCreate { get; set; }
        public string projectName { get; set; }

    }

    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<ShortCutItem> _shortCutItems;
        public  List<ShortCutItem> ShortCutItems
        {
            get => _shortCutItems;
            set
            {
                _shortCutItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShortCutItems)));
            }
        }

        private ObservableCollection<NaviMenuItem> _naviMenuItems;
        private NaviMenuItem _selectedItem;
        private int _selectedIndex;

        private ObservableCollection<Project> _projects;
        private Project _selectedProject;
        private int _selectedProjectIndex;

        //public Tree tr;

        //我还没弄明白怎么动态加载一段xaml，跨窗口的时候，里面有一些坑
        public void ReadNaviMenu(string fileName)
        {

        }

        public MainWindowViewModel()
        {
            _naviMenuItems = new ObservableCollection<NaviMenuItem>()
            {
                new NaviMenuItem() { Name = "结构1", Index=0, MenuFile="heelo" },
                new NaviMenuItem() { Name = "结构2", Index = 1, MenuFile = "heelo" },
                new NaviMenuItem() { Name = "结构3", Index = 2, MenuFile = "heelo" }
            };
            SelectedIndex = 0;


            ShortCutItems = new List<ShortCutItem>()
            {
                new ShortCutItem(){Name="ffff"},
                new ShortCutItem(){Name="ffff"},
                new ShortCutItem(){Name="ffff"},
                new ShortCutItem(){Name="ffff"}
            };
            //SelectedItem = _naviMenuItems[0];
        }

        public ObservableCollection<NaviMenuItem> NaviMenuItems
        {
            get => _naviMenuItems;
            set
            {
                _naviMenuItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NaviMenuItems)));
            }
        }

        public NaviMenuItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == null || value.Equals(_selectedItem))
                    return;

                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                //tr.DataContext = new TreesViewModel(_selectedItem._cates);
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        public Project SelectedProject
        {
            get => _selectedProject;
            set 
            {
                _selectedProject = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedProject)));
               // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedProject.projectName)));
            }
        }
    }
}
