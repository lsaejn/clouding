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
        public string server;
        public string updateInfoUrl;
        public string pkgRootFolder;
        public string fixPackFolder;
        public string updatePackFolder;
        public string integralImageFileFolder;
        public bool useLocalPackInfo;
        public bool useHttp;
        public string localPackInfo;
    }

    /// <summary>
    /// 路径皆为全路径
    /// </summary>
    public sealed class ConfigFileRW
    {
        public ConfigFile config_;
        public static ConfigFileRW GetInstance { get; } = new ConfigFileRW();

        public string DownloadDir
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory+config_.downloadDir;
            }
        }
        public string LogFilePath
        {
            get
            {
                return DownloadDir+config_.logFilePath;
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
        public string UpdateInfoUrl
        {
            get
            {
                return PkgRootFolder + config_.updateInfoUrl;
            }
        }
        public string PkgRootFolder
        {
            get
            {
                string url = string.Empty;
                if (UseHttp)
                    url += "http://";
                else
                    url += "https://";
                url += Server;
                return url+config_.pkgRootFolder;
            }
        }
        public string FixPackFolder
        {
            get
            {
                return PkgRootFolder+config_.fixPackFolder;
            }
        }
        public string UpdatePackFolder
        {
            get
            {
                return PkgRootFolder+config_.updatePackFolder;
            }
        }
        public bool UseLocalPackInfo
        {
            get
            {
                return config_.useLocalPackInfo;
            }
        }
        public bool UseHttp
        {
            get
            {
                return config_.useHttp;
            }
        }
        public string LocalPackInfo
        {
            get
            {
                return config_.localPackInfo;
            }
        }
        public string Server
        {
            get
            {
                return config_.server;
            }
        }


        private ConfigFileRW()
        {
            string appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string configFile = appPath + "config.json";
            try
            {
                string str = File.ReadAllText(configFile, Encoding.UTF8);
                var serializer = new JavaScriptSerializer();
                config_ = serializer.Deserialize<ConfigFile>(str);
            }
            catch(Exception e)
            {
                //Logger.Log().Debug(e.Message);
                MessageBox.Show("配置文件错误，程序即将关闭");
                System.Environment.Exit(0);
            }
        }
    }
}
