﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    public struct ConfigData
    {
        public string cfglib2014 { get; set; }
        public string cfglib2015 { get; set; }
        public string pkpm_cfglib { get; set; }
        public string stskey_include { get; set; }
        public string stskey_lib { get; set; }
        public string pkpm_stskey_lib { get; set; }
        public string pkpm_stskey_include { get; set; }
        public string temp_stskey_lib { get; set; }
        public string update_stskey_include { get; set; }
        public string update_stskey_lib { get; set; }
    }

    public struct configDataRW
    {
        public void Init(ref ConfigData d)
        {
            string appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string configFile = appPath + "/config.json";
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
    }

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        ArrayList list_left;
        ArrayList list_right;
        ConfigData configData;

        public MainWindow()
        {
            InitializeComponent();
            configDataRW rw = new configDataRW();
            rw.Init(ref configData);
        }

        private void MoveToLeft(object sender, RoutedEventArgs e)
        {

        }

        private void MoveToRight(object sender, RoutedEventArgs e)
        {

        }

        private void DoMoving(object sender, RoutedEventArgs e)
        {

        }

        private void OnMenuButtonClicked(object sender, RoutedEventArgs e)
        {
            //var button = (Button)sender; sender是stackPanel
            Button button = (Button)e.OriginalSource;
            if (button != null)
            {
                var btnName = button.Content.ToString();
                if(btnName== "设置")
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
            }
        }
    }
}
