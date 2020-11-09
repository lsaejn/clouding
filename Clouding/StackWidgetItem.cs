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
    /// <summary>
    /// 一次具体的下载。不用异步是为了避免和之前c++版本一样混乱。
    /// </summary>
    public class DownLoadTask
    {
        StackWidgetItem item;
        bool stopped { get; set; }
        string url { get; set; }
        /// <summary>
        /// 任务开始时，已下载到文件的字节数
        /// </summary>
        long bytesDown { get; set; }
        /// <summary>
        /// 目标字节数
        /// </summary>
        long bytesTotal { get; set; }
        public void Stop()
        {
            stopped = true;
        }
        public DownLoadTask(StackWidgetItem _item)
        {
            item = _item;
            url = item.url;
            stopped = false;
        }
        public void DownloadFileImpl()
        {
            Stream ofs = null;
            Stream netStream = null;
            try
            {
                //属性绑定, 不需要delegate
                //item.state_ = "you know nothing";
                //item.stopped_ = false;
                //return;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.AllowAutoRedirect = false;
                request.Timeout = 5000;
                WebResponse respone = request.GetResponse();
                netStream = respone.GetResponseStream();
                string downLoadPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\download\\";
                string localFilePath = downLoadPath + item.packageName;
                ofs = new FileStream(localFilePath, FileMode.Append, FileAccess.Write);
                byte[] read = new byte[1024 * 64];
                int realReadLen = netStream.Read(read, 0, read.Length);
                if(realReadLen>0)
                    item.state_ = "正在下载";
                while (realReadLen > 0 && !stopped)
                {
                    lock (this)
                    {
                        if(!stopped)
                        {
                            ofs.Write(read, 0, realReadLen);
                            ofs.Flush();
                            if (bytesDown == bytesTotal)
                                item.state_ = "下载完成";
                        }
                    }
                    realReadLen = netStream.Read(read, 0, read.Length);
                }
                //item update
            }
            catch (Exception e)
            {
                Logger.Log().Error(e.Message);
                lock (this)
                {
                    if (!stopped)
                    {
                        item.state_ = "下载失败";
                        item.task = null;
                    }     
                }   
            }
            finally
            {
                if(netStream!=null)
                    netStream.Close();
                if (ofs!=null)
                    ofs.Close();
            }
        }
    }



    public class StackWidgetItem: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string packageName;
        public string speed;
        public string timeLeft;
        public string state;
        public string fileSizeState;
        public int progressValue;
        public string url;
        public bool stopped;
        public long bytesDown;
        public long bytesTotal;

        //fix me. 等待删除
        static int num;
        public bool useTestCase;

        public string imageSrc
        {
            get
            {
                if(useTestCase)
                {
                    num++;
                    if (num % 4 == 0)
                        return "/Assets/bell.png";
                    else if (num % 4 == 1)
                        return "/Assets/apps.png";
                    else if (num % 4 == 2)
                        return "/Assets/cartcolor.png";
                    return "/Assets/computer.png";
                }
                else
                    return "/test;component/Assets/bell.png";
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
        public StackWidgetItem(string timeLeft, string speed, string name, int progressValue, string url, long bytesTotal)
        {
            useTestCase = true;
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
            //fix me, 下载速度格式化, 我们基本上都很大，MB就行了
            fileSizeState_ = (bytesDown / (1024.0*1024)).ToString("f2") + "MB/"+ (bytesTotal / (1024.0 * 1024)).ToString("f2") +"MB";
            progressValue_ = (int)(bytesDown / bytesTotal);
            //filesiz
            //bytesTotal;
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
        public int progressValue_
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

        public DownLoadTask task { get; set; }
        Object lockObj=new Object();

        public void ResetUIWhenDownloadFinished()
        {

        }
        public void ResetUIWhenDownloadFailed()
        {

        }

        public void OnClickDownloadBtn()
        {
            if (true == stopped_)
                StartDownLoad();
            else
                StopDownLoad();
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
                
            //call
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

        }

        public void OpenFolder()
        {

        }

        public bool InstallFile()
        {
            return true;
        }

        //pack these into download class


    }//class StackWidgetItem
}// namespace Clouding
