using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace QQ_Music
{
    [TypeConverter(typeof(SingerConverter))]
    public class Singer
    {
        public string Name { get; set; }
    }
    public class SingerConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if(value is string)
            {
                Singer m = new Singer();
                m.Name = value as string;
                return m;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    //[TypeConverter(typeof(MusicConverter))]
    public class Music
    {
        public string Name { get; set; }
        public Singer Artist { get; set; }
    }


    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var m=FindResource("defaultMusic") as Music;
            MessageBox.Show(m.Name + "  " + m.Artist.Name);
        }
    }
}
