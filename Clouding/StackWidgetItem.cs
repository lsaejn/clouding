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

namespace Clouding
{
    public class StackWidgetItem: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string packageName { get; set; }
        public string speed { get; set; }
        public string timeLeft { get; set; }
        public string state { get; set; }
        public int progressValue { get; set; }
        public string url { get; set; }
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

        public void DownloadFile()
        {
            new Thread(DownloadFileImpl).Start();
            Thread.Sleep(2000);
            lock (this)
            {
                speed_ = "fuck2";
            }
        }


        public void DownloadFileImpl()
        {
            speed_ = "fuck";
            //lb.Content = "you know nothing";
            //return;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.AllowAutoRedirect = true;
            WebResponse respone = request.GetResponse();
            Stream netStream = respone.GetResponseStream();

            Stream stm = new FileStream("1.txt", FileMode.Append, FileAccess.Write);

            byte[] read = new byte[1024*64];
            int realReadLen = netStream.Read(read, 0, read.Length);
            while (realReadLen > 0)//& !stopped
            {
                lock (this)
                {
                    Thread.Sleep(5000);
                    stm.Write(read, 0, realReadLen);
                    stm.Flush();
                }
                realReadLen = netStream.Read(read, 0, read.Length);
            }
            netStream.Close();  
            stm.Close();
        }

    }
}
