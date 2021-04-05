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
    public class Project
    {
        public string txt { get; set; }
        public string timeCreate { get; set; }
        public string projectName { get; set; }
    }

    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<NaviMenuItem>? _naviMenuItems;
        private NaviMenuItem? _selectedItem;
        private int _selectedIndex;

        private ObservableCollection<Project> _projects;
        private Project __selectedproject;
        private int _selectedProjectIndex;

        public Tree tr;

        public MainWindowViewModel()
        {
            _naviMenuItems = new ObservableCollection<NaviMenuItem>()
            {
                new NaviMenuItem() { Name = "结构1", Index=0, MenuFile="heelo" },
                new NaviMenuItem() { Name = "结构2", Index = 1, MenuFile = "heelo" },
                new NaviMenuItem() { Name = "结构3", Index = 2, MenuFile = "heelo" }
            };
            SelectedIndex = 0;
            //SelectedItem = _naviMenuItems[0];
        }

        public ObservableCollection<NaviMenuItem>? NaviMenuItems
        {
            get => _naviMenuItems;
            set
            {
                _naviMenuItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NaviMenuItems)));
            }
        }

        public NaviMenuItem? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == null || value.Equals(_selectedItem)) return;

                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                tr.DataContext = new TreesViewModel(_selectedItem._cates);
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
    }
}
