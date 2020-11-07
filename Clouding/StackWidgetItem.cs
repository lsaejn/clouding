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

namespace Clouding
{
    /// <summary>
    /// 一次具体的下载。这个思路是对的，避免和之前c++一样混乱。
    /// </summary>
    public class DownLoadTask
    {
        StackWidgetItem item;
        bool stopped;
        string url;
        public DownLoadTask(StackWidgetItem _item)
        {
            item = _item;
            url = item.url;
            stopped = false;
        }
        public void DownloadFileImpl()
        {
            Stream ofs = null;
            try
            {
                //属性绑定, 不需要代理
                item.state_ = "you know nothing";
                item.stopped_ = false;
                return;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.AllowAutoRedirect = false;
                request.Timeout = 5000;
                WebResponse respone = request.GetResponse();
                Stream netStream = respone.GetResponseStream();
                ofs = new FileStream("1.txt", FileMode.Append, FileAccess.Write);
                byte[] read = new byte[1024 * 64];
                int realReadLen = netStream.Read(read, 0, read.Length);
                while (realReadLen > 0 & !stopped)
                {
                    lock (this)
                    {
                        Thread.Sleep(5000);
                        ofs.Write(read, 0, realReadLen);
                        ofs.Flush();
                    }
                    realReadLen = netStream.Read(read, 0, read.Length);
                }
                netStream.Close();
                item.state_ = "下载完成";
                //check filelengthhere
            }
            catch (Exception e)
            {
                Logger.Log().Error(e.Message);
                item.state_ = "下载失败";
            }
            finally
            {
                if(ofs!=null)
                    ofs.Close();
            }
        }
    }



    public class StackWidgetItem: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string packageName { get; set; }
        public string speed { get; set; }
        public string timeLeft { get; set; }
        public string state { get; set; }
        public int progressValue { get; set; }
        public string url { get; set; }
        public bool stopped { get; set; }
        public System.Windows.Controls.Label lb;



        static int num;
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

        public bool useTestCase;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeLeft"></param>
        /// <param name="speed"></param>
        /// <param name="name">下载文件名</param>
        /// <param name="progressValue"></param>
        /// <param name="url">下载网址</param>
        /// <param name="lb">测试label</param>
        public StackWidgetItem(string timeLeft, string speed, string name, int progressValue, string url, System.Windows.Controls.Label lb)
        {
            useTestCase = true;
            this.timeLeft = timeLeft;
            this.packageName = name;
            this.speed = speed;
            this.progressValue = progressValue;
            this.state = "暂停中";
            this.url = url;
            this.lb = lb;
            stopped = true;
        }

        public int progressValue_
        {
            get { return progressValue; }
            set
            {
                progressValue_ = value;
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

        DownLoadTask task;
        Object lockObj=new Object();

        public void ResetUIWhenDownloadFinished()
        {

        }
        public void ResetUIWhenDownloadFailed()
        {

        }

        public void DownloadFile()
        {
            task = new DownLoadTask(this);
            new Thread(task.DownloadFileImpl).Start();
            //Thread.Sleep(2000);
            //lock (this)
            //{
            //    speed_ = "fuck2";
            //}
        }

        public void StopDownLoad()
        {
            //call
            lock (lockObj)//lock似乎应该放在task对象里
            {
                stopped = true;
                task = null;
            }
            
        }

        public void StartDownLoad()
        {

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
