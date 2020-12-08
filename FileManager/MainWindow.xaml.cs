using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileManager
{
    public class ConfigData
    {
        /// <summary>
        /// CFG2014所在目录, 沈工发布目录
        /// </summary>
        public string cfglib2014 { get; set; }
        /// <summary>
        /// CFG2015所在目录, 公共更新目录
        /// </summary>
        public string cfglib2015 { get; set; }
        /// <summary>
        /// pkpmmain暂时只用到了两个cfglib
        /// </summary>
        public List<string> cfglibForPkpmmainDebug { get; set; }
        public List<string> cfglibForPkpmmainRelease { get; set; }
        /// <summary>
        /// PKPMMain项目的CFGLIB目录
        /// </summary>
        public string pkpm_cfglib { get; set; }
        /// <summary>
        /// stskey项目的include目录
        /// </summary>
        public string generate_stskey_include { get; set; }
        /// <summary>
        /// stskey项目人为更新的头文件
        /// </summary>
        public List<string> generate_stskey_include_names { get; set; }
        /// <summary>
        /// stskey项目的lib生成根目录
        /// </summary>
        public string generate_stskey_lib { get; set; }
        /// <summary>
        /// stskey项目生成的库文件
        /// </summary>
        public List<string> generate_stskey_lib_names { get; set; }
        /// <summary>
        /// PKPMmain的stskey库目录
        /// </summary>
        public string pkpm_stskey_lib { get; set; }
        /// <summary>
        /// pkpmmain只需要更新两个stskey库文件
        /// </summary>
        public List<string> pkpm_stskey_lib_debugFiles { get; set; }
        /// <summary>
        /// pkpmmain只需要更新两个stskey库文件
        /// </summary>
        public List<string> pkpm_stskey_lib_releaseFiles { get; set; }
        /// <summary>
        /// PKPMmain的stskey头文件目录
        /// </summary>
        public string pkpm_stskey_include { get; set; }
        /// <summary>
        /// pkpmmain的输出根目录
        /// </summary>
        public string pkpmmain_out_dir { get; set; }
        /// <summary>
        /// pkpmmain的debug编译选项的输出文件(需要更新到P盘)
        /// </summary>
        public List<string> pkpmmain_out_file_Debug { get; set; }
        public List<string> pkpmmain_out_file_Release { get; set; }
        public List<string> pkpmmain_out_file_ReleaseDebug { get; set; }
        /// <summary>
        /// Stskey库的临时目录
        /// </summary>
        public string temp_stskey_lib { get; set; }

        public string update_pkpmmain { get; set; }
        /// <summary>
        /// stskey的头文件发布目录
        /// </summary>
        public string update_stskey_include { get; set; }
        /// <summary>
        /// stskey的库发布目录
        /// </summary>
        public string update_stskey_lib { get; set; }
        public string pkpm_debug_Folder { get; set; }
        public string pkpm_release_Folder { get; set; }
        public string pkpm_releaseDebug_Folder { get; set; }
    }
    public struct configDataRW
    {
        public void Init(ref ConfigData d)//or out
        {
            string appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string configFile = appPath + "\\config.json";
            try
            {
                string str = File.ReadAllText(configFile, Encoding.UTF8);
                d = JsonConvert.DeserializeObject<ConfigData>(str);
            }
            catch
            {
                MessageBox.Show("配置文件错误，程序即将关闭");
                System.Environment.Exit(0);
            }
        }

        public void Save(ref ConfigData d)
        {
            var str=JsonConvert.SerializeObject(d);
            string appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string configFile = appPath + "/config.json";
            File.WriteAllText(configFile, str);
        }
    }
    class FileListItem
    {
        public string fileName;
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }
    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        List<FileListItem> list_left;
        List<FileListItem> list_right;
        string currentSrcPath;
        string currentDesPath;
        string pageName;
        TimeSpan defaultDeadLine;
        ConfigData configData;
        public string CurrentDesPath
        {
            get { return currentDesPath; }
            set
            {
                currentDesPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentDesPath"));
            }
        }
        public string CurrentSrcPath
        {
            get { return currentSrcPath; }
            set
            {
                currentSrcPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentSrcPath"));
            }
        }



        public MainWindow()
        {
            InitializeComponent();

            configDataRW rw = new configDataRW();
            rw.Init(ref configData);//or out
            list_left = new List<FileListItem>
            {
                new FileListItem{ fileName ="测试"},
                new FileListItem{ fileName ="测试"}
            };
            list_right = new List<FileListItem>();
            leftBox.ItemsSource = list_left;

            CurrentDesPath = configData.temp_stskey_lib;
            CurrentSrcPath = configData.temp_stskey_lib;

            defaultDeadLine = TimeSpan.FromDays(10);
        }

        private void MoveToLeft(object sender, RoutedEventArgs e)
        {

        }

        private void MoveToRight(object sender, RoutedEventArgs e)
        {
            var item=leftBox.SelectedItem as FileListItem;
            var name=item.FileName;
            list_right.Add( new FileListItem { FileName= name });
            rightBox.ItemsSource = null;
            rightBox.ItemsSource = list_right;
        }

        //签出文件 https://cloud.tencent.com/developer/ask/68364
        private void OnMenuButtonClicked(object sender, RoutedEventArgs e)
        {
            //var button = (Button)sender; sender是stackPanel
            Button button = (Button)e.OriginalSource;
            if (button != null)
            {
                var btnName = button.Content.ToString();
                pageName = btnName;
                //注意，这里是跑脚本，脚本目录需要单独设置
                if (btnName== "编译StsKey")
                {
                    CurrentDesPath = "";
                    CurrentSrcPath = "";
                }
                else if(btnName== "更新Sts_Key到P")
                {
                    CurrentSrcPath = configData.generate_stskey_lib;

                    CurrentDesPath = configData.update_stskey_lib;
                }
                else if(btnName == "针对Sts_Key更新Pkpmmain")
                {
                    
                }
                else if(btnName == "移动CFGLIB2014到2015")
                {
                    
                }
                else if (btnName == "移动CFGLIB到Pkpmmain目录")
                {
                 
                }
                else if (btnName == "编译Pkpmmain")
                {

                }
                else if (btnName == "移动Pkpmmain程序到P盘")
                {
                    CopyPkpmmainToDiskP();
                }
                else if (btnName == "一键更新全部")
                {
                    GenerateStsLibs();
                    ReleaseStsLibsToDiskP();
                    ReleaseStsLibsToPkpmmainSolution();
                    CopyCFG2014ToCFG2015();
                    CopyCFG2015ToPKPkpmmainSolution();
                    GeneratePkpmmainSolution();
                    CopyPkpmmainToDiskP();
                }
                else if (btnName == "设置")
                {
                    Type type = this.GetType();
                    Assembly assembly = type.Assembly;
                    Window win = (Window)assembly.CreateInstance(
                        type.Namespace + "." + "Setting");
                    if (null == win)
                        MessageBox.Show($"not class name as {btnName}");
                    else
                    {
                        var settingWindow = ((Setting)win);
                        settingWindow.SetData(configData);
                        settingWindow.ShowDialog();
                    }
                }
                else if (btnName == "更新调试环境")
                {

                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            configDataRW rw = new configDataRW();
            rw.Save(ref configData);
        }

        private void GenerateStsLibs()
        {
            Process proc = new Process();
            var appPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            proc.StartInfo.WorkingDirectory = appPath;
            proc.StartInfo.FileName = "stskeyCompile.bat";

            proc.Start();
            proc.WaitForExit();
            //fix me check lib are all new here
        }
        private void ReleaseStsLibsToDiskP()
        {
            //header file
            var srcDir = configData.generate_stskey_include;
            var desDir = configData.update_stskey_include;
            foreach (var filename in configData.generate_stskey_include_names)
            {
                File.Copy(srcDir + filename, desDir + filename, true);
            }
            //lib file
            srcDir = configData.generate_stskey_lib;
            desDir = configData.update_stskey_lib;
            foreach (var filename in configData.generate_stskey_lib_names)
            {
                var i = filename.IndexOf(".");
                if (!File.Exists(srcDir + filename))//本地只编译了部分选项
                    continue;
                var fileNameWithNoPrefix = filename;
                var j = fileNameWithNoPrefix.IndexOf("\\");
                if (j != -1)
                    fileNameWithNoPrefix = fileNameWithNoPrefix.Substring(j + 1);
                if (filename[i - 1] == 'D' || filename.Substring(i - 2, 2) == "D_")
                {
                    File.Copy(srcDir + filename, desDir + "Debug\\" + fileNameWithNoPrefix, true);
                }
                else
                {
                    File.Copy(srcDir + filename, desDir + "Release\\" + fileNameWithNoPrefix, true);
                }
            }
        }
        private void ReleaseStsLibsToPkpmmainSolution()
        {
            //更新sts_key头文件到Pkpmmain
            var srcDir = configData.generate_stskey_include;
            var desDir = configData.pkpm_stskey_include;
            foreach (var filename in configData.generate_stskey_include_names)
            {
                File.Copy(srcDir + filename, desDir + filename, true);
            }
            //lib file
            srcDir = configData.generate_stskey_lib;
            desDir = configData.pkpm_stskey_lib;
            foreach (var filename in configData.generate_stskey_lib_names)
            {
                var i = filename.IndexOf(".");
                if (!File.Exists(srcDir + filename))//本地只编译了部分选项
                    continue;
                var fileNameWithNoPrefix = filename;
                var j = fileNameWithNoPrefix.IndexOf("\\");
                if (j != -1)
                    fileNameWithNoPrefix = fileNameWithNoPrefix.Substring(j + 1);
                if (filename[i - 1] == 'D' || filename.Substring(i - 2, 2) == "D_")
                {
                    if (configData.pkpm_stskey_lib_debugFiles.Contains(fileNameWithNoPrefix))
                        File.Copy(srcDir + filename, desDir + "Debug\\" + fileNameWithNoPrefix, true);
                }
                else
                {
                    if (configData.pkpm_stskey_lib_releaseFiles.Contains(fileNameWithNoPrefix))
                        File.Copy(srcDir + filename, desDir + "Release\\" + fileNameWithNoPrefix, true);
                }
            }
        }
        private void CopyCFG2014ToCFG2015()
        {
            var srcDir = configData.cfglib2014;
            var desDir = configData.cfglib2015;
            //debug
            var debugSrcDir = srcDir + "Debug\\";
            var debugDesDir = desDir + "Debug\\";
            if (!Directory.Exists(debugSrcDir))
            {
                MessageBox.Show("CFGLIB2014目录不存在Debug子目录");
                return;
            }
            foreach (var fullPath in Directory.GetFiles(debugSrcDir))
            {
                FileInfo f = new FileInfo(fullPath);
                var filename = f.Name;
                if (!f.Exists)
                {
                    MessageBox.Show("{filename}不存在");
                    continue;
                }
                var tm = f.LastWriteTime;
                //十天以内
                if (DateTime.Now.Subtract(tm) < defaultDeadLine)
                {
                    File.Copy(debugSrcDir + filename, debugDesDir + filename, true);
                }
            }
            //release
            var releaseSrcDir = srcDir + "Release\\";
            var releaseDesDir = desDir + "Release\\";
            if (!Directory.Exists(releaseSrcDir))
            {
                MessageBox.Show("CFGLIB2014目录不存在Debug子目录");
                return;
            }
            foreach (var fullPath in Directory.GetFiles(releaseSrcDir))
            {
                FileInfo f = new FileInfo(fullPath);
                var filename = f.Name;
                if (!f.Exists)
                {
                    MessageBox.Show("{filename}不存在");
                    continue;
                }
                var tm = f.LastWriteTime;
                //十天以内
                if (DateTime.Now.Subtract(tm) < defaultDeadLine)
                {
                    File.Copy(releaseSrcDir + filename, releaseDesDir + filename, true);
                }
            }
        }
        private void CopyCFG2015ToPKPkpmmainSolution()
        {
            var srcDir = configData.cfglib2014;
            //debug
            var debugSrcDir = srcDir + "Debug\\";
            var debugDesDir = configData.pkpm_cfglib + "Debug\\";

            if (!Directory.Exists(debugSrcDir))
            {
                MessageBox.Show("CFGLIB2014目录不存在Debug子目录");
                return;
            }
            foreach (var fullPath in Directory.GetFiles(debugSrcDir))
            {
                FileInfo f = new FileInfo(fullPath);
                if (!f.Exists)
                {
                    MessageBox.Show("{filename}不存在");
                    continue;
                }
                var filename = f.Name;
                var tm = f.LastWriteTime;
                //十天以内
                if (DateTime.Now.Subtract(tm) < defaultDeadLine)
                {
                    if (configData.cfglibForPkpmmainDebug.Contains(f.Name))
                        File.Copy(debugSrcDir + filename, debugDesDir + filename, true);
                }
            }
            //release
            var releaseSrcDir = srcDir + "Release\\";
            var releaseDesDir = configData.pkpm_cfglib + "Release\\";
            if (!Directory.Exists(releaseSrcDir))
            {
                MessageBox.Show("CFGLIB2014目录不存在Debug子目录");
                return;
            }
            foreach (var fullPath in Directory.GetFiles(releaseSrcDir))
            {
                FileInfo f = new FileInfo(fullPath);
                if (!f.Exists)
                {
                    MessageBox.Show("{filename}不存在");
                    continue;
                }
                var filename = f.Name;
                var tm = f.LastWriteTime;
                //十天以内
                if (DateTime.Now.Subtract(tm) < defaultDeadLine)
                {
                    if (configData.cfglibForPkpmmainRelease.Contains(f.Name))
                        File.Copy(releaseSrcDir + filename, releaseDesDir + filename, true);
                }
            }
        }
        private void GeneratePkpmmainSolution()
        {
            Process proc = new Process();
            var appPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            proc.StartInfo.WorkingDirectory = appPath;
            proc.StartInfo.FileName = "pkpmCompile.bat";

            proc.Start();
            proc.WaitForExit();
        }
        private void CopyPkpmmainToDiskP()
        {
            //var pkpmOutPath = configData.pkpmmain_out_dir;
            //var desDir_debug = configData.update_pkpmmain + "Debug\\";
            //var desDir_release = configData.update_pkpmmain + "Release\\";
            //var desDir_releaseDebug = configData.update_pkpmmain + "DebugRelease\\";
            foreach (string s in configData.pkpmmain_out_file_Debug)
            {
                FileInfo fi = new FileInfo(configData.pkpmmain_out_dir + s);
                if (!fi.Exists)
                {
                    MessageBox.Show($"找不到文件{s}");
                }
                else
                {
                    fi.CopyTo(configData.update_pkpmmain + s, true);
                }
            }
            foreach (string s in configData.pkpmmain_out_file_Release)
            {
                FileInfo fi = new FileInfo(configData.pkpmmain_out_dir + s);
                if (!fi.Exists)
                {
                    MessageBox.Show($"找不到文件{s}");
                }
                else
                {
                    fi.CopyTo(configData.update_pkpmmain + s, true);
                }
            }
            foreach (string s in configData.pkpmmain_out_file_ReleaseDebug)
            {
                FileInfo fi = new FileInfo(configData.pkpmmain_out_dir + s);
                if (!fi.Exists)
                {
                    MessageBox.Show($"找不到文件{s}");
                }
                else
                {
                    fi.CopyTo(configData.update_pkpmmain + s);
                }
            }
        }

        private void DoOperator(object sender, RoutedEventArgs e)
        {

        }
    }
}
