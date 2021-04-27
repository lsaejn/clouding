using Launcher.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Launcher
{
    [ValueConversion(typeof(NaviMenuItem), typeof(TreesViewModel))]
    class TreeViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //tr.DataContext = new TreesViewModel(_selectedItem._cates);
            if (value is null)
                return null;
            var item = (NaviMenuItem)value;
            return new TreesViewModel(item._cates);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //fix me, 
            return "下载" == (string)value;
        }
    }
}
