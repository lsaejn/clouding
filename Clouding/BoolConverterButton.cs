using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Clouding
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class boolDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool reValue = System.Convert.ToBoolean(value);
            if (reValue == true)
            {
                return "下载";
            }
            else
            {
                return "暂停";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "下载" == (string)value;
        }
    }
}
