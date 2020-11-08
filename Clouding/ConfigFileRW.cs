using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;

/// <summary>
/// fix me, 没有仔细检查
/// </summary>
namespace Clouding
{
    public class ConfigFile
    {
        public string downloadDir;
        public string maxTaskNum;
        public string logFilePath;
        public string logLever;
        public string logFileRollSize;
        public string assistantDll;
        public string updateInfoUrl;
        public string pkgRootFolder;
        public string fixPackFolder;
        public string updatePackFolder;
        public string IntegralImageFileFolder;
        public string useLocalPackInfo;
        public string localPackInfo;
    }

    public sealed class ConfigFileRW
    {
        public ConfigFile config_;
        public static ConfigFileRW GetInstance { get; } = new ConfigFileRW();

        public string downloadDir
        {
            get
            {
                return config_.downloadDir;
            }
        }
        public string logFilePath
        {
            get
            {
                return config_.logFilePath;
            }
        }
        public string logLever
        {
            get
            {
                return config_.logLever;
            }
        }
        public string logFileRollSize
        {
            get
            {
                return config_.logFileRollSize;
            }
        }
        public string updateInfoUrl
        {
            get
            {
                return config_.updateInfoUrl;
            }
        }
        public string pkgRootFolder
        {
            get
            {
                return config_.pkgRootFolder;
            }
        }
        public string fixPackFolder
        {
            get
            {
                return config_.fixPackFolder;
            }
        }
        public string updatePackFolder
        {
            get
            {
                return config_.updatePackFolder;
            }
        }
        public string useLocalPackInfo
        {
            get
            {
                return config_.useLocalPackInfo;
            }
        }
        public string localPackInfo
        {
            get
            {
                return config_.localPackInfo;
            }
        }


        private ConfigFileRW()
        {
            string appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string configFile = appPath + "/config.json";
            //Stream fstream = new FileStream(configFile, FileMode.Open, FileAccess.Read);
            try
            {
                string str = File.ReadAllText(configFile, Encoding.UTF8);//NET中内存中的字符串都是Unicode
                var serializer = new JavaScriptSerializer();
                config_ = serializer.Deserialize<ConfigFile>(str);
            }
            catch
            {
                MessageBox.Show("配置文件错误，程序即将关闭");
                System.Environment.Exit(0);
            }
        }
    }
}
