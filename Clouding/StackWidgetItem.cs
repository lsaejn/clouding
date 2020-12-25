using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection.Emit;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace Clouding
{

    public class StackWidgetItem: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string packageName;
        public string speed;
        public string timeLeft;
        public string state;
        public string fileSizeState;
        public double progressValue;
        public string url;
        public bool stopped;
        public long bytesDown;
        public long bytesTotal;
        public string imageSrc
        {
            get
            {
                if(packageName_.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    return "/Assets/exe.png";
                }
                else if(packageName_.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    return "/Assets/zip.png";
                }
                else if (packageName_.EndsWith(".iso", StringComparison.OrdinalIgnoreCase))
                {
                    return "/Assets/iso.png";
                }
                else
                {
                    return "/Assets/directory.png";
                }
            }
        }

        public string packageName_
        {
            get { return packageName; }
            set
            {
                packageName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("packageName_"));
            }
        }

        public long bytesDown_
        {
            get { return bytesDown; }
            set
            {
                bytesDown = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("bytesDown_"));
            }
        }
        /// <summary>
        /// fix me, bytesTotal_没什么机会变化
        /// </summary>
        public long bytesTotal_
        {
            get { return bytesTotal; }
            set
            {
                bytesTotal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("bytesTotal_"));
            }
        }

        public double progressValue_
        {
            get { return progressValue; }
            set
            {
                progressValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("progressValue_"));
            }
        }

        public string state_
        {
            get { return state; }
            set
            {
                state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("state_"));
            }
        }

        public string fileSizeState_
        {
            get { return fileSizeState; }
            set
            {
                fileSizeState = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("fileSizeState_"));
            }
        }

        public string url_
        {
            get { return url; }
            set
            {
                url = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("url_"));
            }
        }

        public string timeLeft_
        {
            get { return timeLeft; }
            set
            {
                timeLeft = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("timeLeft_"));
            }
        }

        public string speed_
        {
            get { return speed; }
            set
            {
                speed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("speed_"));
            }
        }

        public bool stopped_
        {
            get { return stopped; }
            set
            {
                stopped = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("stopped_"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeLeft"></param>
        /// <param name="speed"></param>
        /// <param name="name">下载文件名</param>
        /// <param name="progressValue"></param>
        /// <param name="url">阿里云下载网址,阿里云只转义了中文</param>
        /// <param name="lb">测试label</param>
        public StackWidgetItem(string timeLeft, string speed, string name, double progressValue, string url, long bytesTotal)
        {
            timeLeft_ = timeLeft;
            var str = System.Web.HttpUtility.UrlDecode(name);
            packageName_ =str.Substring(str.LastIndexOf('/')+1);
            speed_ = speed;
            progressValue_ = progressValue;
            state = "暂停中";
            url_ = url;
            bytesTotal_ = bytesTotal;
            stopped = true;
            ReadLocalFile();
        }

        public void ReadLocalFile()
        {
            string downLoadPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"\\download\\";
            string localFilePath = downLoadPath + packageName;
            FileInfo info = new FileInfo(localFilePath);
            if (info.Exists)
                bytesDown = info.Length;
            else
                bytesDown = 0;
            if(bytesDown== bytesTotal)
                state = "下载完成";
            else if(bytesDown > bytesTotal)
                state = "文件大小错误";
            //fix me, 下载速度格式化, 我们基本上都很大，MB就行了
            fileSizeState_ = (bytesDown / (1024.0*1024)).ToString("f2") + "MB/"+ (bytesTotal / (1024.0 * 1024)).ToString("f2") +"MB";
            progressValue_ = 100.0*bytesDown / bytesTotal;
            //filesiz
            //bytesTotal;
    }


        public IDownLoadStrategy task { get; set; }
        Object lockObj=new Object();

        public void ResetUIWhenDownloadFinished()
        {

        }

        public void ResetUIWhenDownloadFailed()
        {

        }

        public void OnClickDownloadBtn()
        {
            if (stopped_)
            {
                if(bytesDown == bytesTotal)
                {
                    if (MessageBox.Show("确定要重新下载吗？", "提示：", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                        return;
                    else
                    {
                        DeleteFile();
                    }       
                }
                state_ = "正在连接...";
                StartDownLoad();
            }     
            else
            {
                StopDownLoad();
                state_ = "暂停中";
                speed_ = "--";
            } 
        }

        private void DownloadFile()
        {
            stopped_ = false;
            task = new DownLoadTask(this);
            new Thread(task.DownloadFileImpl).Start();
        }

        public void StopDownLoad()
        {
            if (task == null)
            {
                MessageBox.Show("任务尚未开始");
                return;
            }
            lock (task)//lock似乎应该放在task对象里
            {
                stopped_ = true;
                task.Stop() ;
                task = null;
            }   
        }

        public void StartDownLoad()
        {
            DownloadFile();
        }

        public void DeleteFile()
        {
            if(task!=null)
            {
                //StopDownLoad();
            }
            string downLoadPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\download\\";
            string localFilePath = downLoadPath + packageName;
            try
            {
                File.Delete(localFilePath);
                ReadLocalFile();
            }
            catch(Exception e)
            {
                Logger.Log().Warn($"delete file failed because of {e.Message}");
            }
        }

        public void OpenFolder()
        {
            string downLoadPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "download\\";
            string localFilePath = downLoadPath + packageName;
            if (File.Exists(localFilePath))
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                psi.Arguments = "/e,/select," + localFilePath;
                System.Diagnostics.Process.Start(psi);
            }
            else
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                psi.Arguments = "/e" + downLoadPath;
                System.Diagnostics.Process.Start(psi);
            }

        }

        public bool InstallFile()
        {
            return true;
        }

        //pack these into download class


    }//class StackWidgetItem
}// namespace Clouding
