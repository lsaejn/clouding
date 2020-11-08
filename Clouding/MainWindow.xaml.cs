using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
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
        public string packCtn { get; set; }

        public PackInfoFile packinfo { get; set; }
        public List<StackWidgetItem> updateItemList;
        public List<StackWidgetItem> fixItemList;
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
            Point pp = Mouse.GetPosition(e.Source as FrameworkElement);
            if (pp.Y > 40)
                return;
            this.WindowState = this.WindowState == WindowState.Maximized ?WindowState.Normal : WindowState.Maximized;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer1_Tick;
            //timer.Repeat = false; //we are not writing qml~~
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
            if (StackWidget.ItemsSource == updateItemList)
                return;
            StackWidget.ItemsSource = null;
            StackWidget.ItemsSource = updateItemList;
        }
        private void OnClickPatching(object sender, RoutedEventArgs e)
        {
            if (StackWidget.ItemsSource == fixItemList)
                return;
            StackWidget.ItemsSource = null;
            StackWidget.ItemsSource = fixItemList;
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
            Button cmd = (Button)e.OriginalSource;

            Type type = this.GetType();
            Assembly assembly = type.Assembly;

            Window win = (Window)assembly.CreateInstance(
                type.Namespace + "." + "SettingWindow");
            if (null == win)
                MessageBox.Show($"无法打开{cmd.Content}对话框 ");
            else
                win.ShowDialog();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitUI();
            var ins=ConfigFileRW.GetInstance;
        }
        private async void InitUI()
        {
            //test();
            // write to config
            string serverFilePath = ConfigFileRW.GetInstance.updateInfoUrl;
            //packCtn = await ReadPackInfo(serverFilePath);
            packCtn = await Task.Run(() =>
            {
                return ReadPackInfo(serverFilePath);
            });
            //不好的味道
            CirclePage circlePage = (CirclePage)this.circleFrame.Content;
            if (0==packCtn.Length)
            {
                circlePage.HideProgressBar();
                circlePage.SetTip("查询信息失败，请检查您的网络");
            }
            else
            {
                ParsePackInfo();
                bool ret=await Task.Run(() =>
                {
                    return InitStackWidget();
                });
                if(ret)
                {
                    StackWidget.ItemsSource = updateItemList;
                    circleFrame.Height = 0;
                }
                else
                {
                    circlePage.HideProgressBar();
                    circlePage.SetTip("查询信息失败，请检查您的网络");
                }
            }
        }

        /// <summary>
        /// 读取Item到StackWidget, StackWidget对应Qt的概念，一时转不过来
        /// </summary>
        /// <returns>是否发生错误</returns>
        private bool InitStackWidget()
        {
            //fix me, 这里需要比较版本，有软件的旧债，让他们自己还
            string appPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string cfgPath = appPath + "/../CFG/";

            var lv = packinfo.LatestVersion;
            var iso = packinfo.LatestIsoUrl;
            var fixs = packinfo.FixPacks;
            var updatepk = packinfo.UpdatePacks;

            fixItemList = new List<StackWidgetItem>();
            foreach (var fixFile in fixs)
            {
                var fn= fixFile.info.fileName;
                fixItemList.Add(new StackWidgetItem("--:--:--",  "0KB/S", fn, 0,
                    ConfigFileRW.GetInstance.pkgRootFolder + ConfigFileRW.GetInstance.fixPackFolder + fn, null));
            }

            updateItemList = new List<StackWidgetItem>
            {
                new StackWidgetItem("00:01:33", "123KB/S",updatepk.fileName,30, "http://update.pkpm.cn/PKPM2010/Info/pkpmSoft/UpdatePacks/"+updatepk.fileName,null),
                new StackWidgetItem("10:21:34", "443KB/S","V5.3.Setup.exe",50, updatepk.fileName, null),
                new StackWidgetItem("00:01:04", "333KB/S","V5.2.2.2.Setup.exe",80, updatepk.fileName, null),
                new StackWidgetItem("00:08:34", "555KB/S","V6.0.Setup.exe",100, updatepk.fileName, null)
            };
            return true;
            //ItemCollection col =StackWidget.Items;
            //StackWidgetItem elem3 =(StackWidgetItem)col.GetItemAt(3);
            //var f=elem3.ItemData;
            //var fName=elem3.packageName;
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
            var serializer = new JavaScriptSerializer();
            packinfo= serializer.Deserialize<PackInfoFile>(packCtn);
        }

        public string ReadPackInfo(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.AllowAutoRedirect = false;
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
                //我们是有意让客户端慢
                Thread.Sleep(2000);
                return buffer;
            }
            catch(Exception e)
            {
                //OutP
                Thread.Sleep(2000);
                Logger.Log().Error($"无法下载网络信息: {url}。原因:{e.Message}");
                return string.Empty;
            }     
        }

        private void OnDownloadFile(object sender, RoutedEventArgs e)
        {
            //Thread.Sleep(100000);
            var curItem = ((ListBoxItem)StackWidget.ContainerFromElement((System.Windows.Controls.Button)sender)).Content;
            StackWidgetItem item = (StackWidgetItem)curItem;
            var pkName=item.packageName;
            item.state_ = "正在连接";
            item.OnClickDownloadBtn();
            
        }

        private void OnDeleteFile(object sender, RoutedEventArgs e)
        {
            var curItem = ((ListBoxItem)StackWidget.ContainerFromElement((System.Windows.Controls.Button)sender)).Content;
            StackWidgetItem item = (StackWidgetItem)curItem;
        }

        private void OnSetupFile(object sender, RoutedEventArgs e)
        {
            var curItem = ((ListBoxItem)StackWidget.ContainerFromElement((System.Windows.Controls.Button)sender)).Content;
            StackWidgetItem item = (StackWidgetItem)curItem;
        }

        private void OnClickOneKeyUpdate(object sender, RoutedEventArgs e)
        {
            //StackWidget.Items.RemoveAt(3);
            List<StackWidgetItem> itemsSource =(List<StackWidgetItem>) StackWidget.ItemsSource;
            itemsSource.RemoveAt(3);

            //Refreshes data binding
            {
                StackWidget.ItemsSource = null;
                StackWidget.ItemsSource = updateItemList;
            }

            //StackWidget
            //itemsSource.
            //var iter = ItemsSource.GetEnumerator();
            //while(iter.)
            //StackWidget.Items.Remove(item);
        }
    }
}
