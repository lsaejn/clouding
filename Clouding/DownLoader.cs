using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clouding
{
    class DownLoader
    {
        public bool stopped { get; set; }
        public bool downloadFinished { get; set; }
        public string url { get; set; }
        public string localPath { get; set; }
        long bytesTotal;
        long byteDown;
        bool hasError;
        StackWidgetItem itemFrom;
        DownLoader(StackWidgetItem itemFrom, string url, string localPath, int bytesTotal, int byteDown)
        {
            this.stopped = false;
            this.downloadFinished = false;
            this.itemFrom = itemFrom;
            this.localPath = localPath;
            this.byteDown = byteDown;
            this.bytesTotal = bytesTotal;
            this.url = url;
        }
        void run()
        {
            Stream fileStream = new FileStream(localPath, FileMode.Append, FileAccess.Write);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.AllowAutoRedirect = false;
                WebResponse respone = request.GetResponse();
                Stream netStream = respone.GetResponseStream();
                

                //fix me, 缓冲区选择多少合适?
                byte[] buffer = new byte[1024*4];
                int realReadLen = netStream.Read(buffer, 0, buffer.Length);
                while(!stopped&& realReadLen>0)
                {
                    fileStream.Write(buffer, 0, realReadLen);
                    realReadLen = netStream.Read(buffer, 0, buffer.Length);
                }
            }
            catch (Exception e)
            {
                var str=e.Message;
                hasError = true;
            }
            finally
            {
                fileStream.Flush();
                downloadFinished = true;
            }
        }


    }
}
