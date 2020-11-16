using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileManager
{
    public class settingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string folderName;
        public string folderPath;
        public string FolderName
        {
            get { return folderName; }
            set
            {
                folderName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FolderName"));
            }
        }
        public string FolderPath
        {
            get { return folderPath; }
            set
            {
                folderName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FolderPath"));
            }
        }
    }
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
            settingItemList = new List<settingItem>();
        }

        public List<settingItem> settingItemList;
        public void SetData(ConfigData d)
        {
            foreach (PropertyInfo info in d.GetType().GetProperties())
            {
                if (info.PropertyType == typeof(string))
                {
                    settingItemList.Add(new settingItem
                    {
                        folderName = info.Name,
                        folderPath = (string)info.GetValue(d, null)
                    });
                }
            }
            container.ItemsSource = null;
            container.ItemsSource = settingItemList;
        }
        private void OnClickButton(object sender, RoutedEventArgs e)
        {

        }
    }
}
