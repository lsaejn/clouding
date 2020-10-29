using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Clouding
{
    public struct PatchItemInfo
    {
        public string patchFile { get; set; }
        public string fileName { get; set; }
    }
    public struct PatchItem
    {
        public string version { get; set; }
        public PatchItemInfo info { get; set; }
    }
    public struct UpdatePacks
    {
        public string version { get; set; }
        public string fileName { get; set; }
    };
    class PackInfoFile
    {
        public string LatestVersion { get; set; }
        public List<PatchItem> FixPacks { get; set; }
        public UpdatePacks UpdatePacks { get; set; }
        public string LatestIsoUrl { get; set; }
    }

    public class UserItem
    {
        public string packageName { get; set; }
        public string speed { get; set; }
        public string timeLeft { get; set; }
        public int progressValue { get; set; }

        static int num;
        public string imageSrc
        {
            get
            {
                num++;
                if(num%4==0)
                    return "/Assets/bell.png";
                else if(num%4==1)
                    return "/Assets/apps.png";
                else if (num%4 == 2)
                    return "/Assets/cartcolor.png";
                return "/Assets/computer.png";
                //return "/test;component/Assets/bell.png";

            }
        }
        public UserItem(string timeLeft, string speed, string name, int progressValue)
        {
            this.timeLeft = timeLeft;
            this.packageName = name;
            this.speed = speed;
            this.progressValue = progressValue;
        }
    }

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            protect = false;
        }

        public DispatcherTimer timer;
        public bool protect { get; set; }
        /*
         * 没有考虑多屏幕
         */
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton==MouseButtonState.Pressed)
            {
                if (protect)
                    return;
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
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer1_Tick;
            protect = true;
            timer.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            protect = false;
            timer.Stop();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //test();
            ParsePackInfo();
            InitStackWidget();
        }

        private void InitStackWidget()
        {
            StackWidget.ItemsSource = new List<UserItem>
            {
                new UserItem("00:01:33", "123KB/S","V5.2.1.Setup.exe",30),
                new UserItem("10:21:34", "443KB/S","V5.3.Setup.exe",50),
                new UserItem("00:01:04", "333KB/S","V5.2.2.2.Setup.exe",80),
                new UserItem("00:08:34", "555KB/S","V6.0.Setup.exe",100)
            };
        }
        public void test()
        {
            using (var sr = new StreamReader("packsInfoWpf_.json"))
            {
                // Read the stream as a string, and write the string to the console.
                var serializer = new JavaScriptSerializer();
                var str = sr.ReadToEnd();
                var ret = serializer.Deserialize<PackInfoFile>(str);
                var lv = ret.LatestVersion;
                var iso = ret.LatestIsoUrl;
                var fixs = ret.FixPacks;
                var updatepk = ret.UpdatePacks;
            }
        }

        public void ParsePackInfo()
        {
            string serverFilePath = "http://update.pkpm.cn/PKPM2010/Info/pkpmSoft/packsInfoWpf1.json";

            string packCtn = ReadPackInfo(serverFilePath);
            var serializer = new JavaScriptSerializer();
            var ret = serializer.Deserialize<PackInfoFile>(packCtn);
            var lv = ret.LatestVersion;
            var iso = ret.LatestIsoUrl;
            var fixs = ret.FixPacks;
            var updatepk = ret.UpdatePacks;
        }

        public string ReadPackInfo(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.AllowAutoRedirect = true;
            WebResponse respone = request.GetResponse();
            Stream netStream = respone.GetResponseStream();

            string buffer = string.Empty;

            byte[] read = new byte[1024];
            int realReadLen = netStream.Read(read, 0, read.Length);
            while (realReadLen > 0)
            {
                buffer += System.Text.Encoding.UTF8.GetString(read, 0, realReadLen);
                realReadLen = netStream.Read(read, 0, read.Length);
            }
            netStream.Close();
            return buffer;
        }

        private void OnDownloadFile(object sender, RoutedEventArgs e)
        {

        }

        private void OnDeleteFile(object sender, RoutedEventArgs e)
        {

        }

        private void OnSetupFile(object sender, RoutedEventArgs e)
        {

        }
    }
}
