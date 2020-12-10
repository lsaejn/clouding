using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clouding
{
    /// <summary>
    /// 一次具体的下载。不用异步是为了避免和之前c++版本一样混乱。
    /// 由于要下载的文件数量极少，根本不需要使用缓冲区
    /// 统一updateTime可能导致的一个问题是，有的任务一直收不到数据包时，速度得不到更新
    /// </summary>
    public class DownLoadTask
    {
        /*static */DateTime updateTime;
        long bytesDownLastUpdate;//上一次更新时的下载量
        StackWidgetItem item;
        bool stopped;
        string url;
        long bytesDown;
        long bytesTotal;
        bool hasError;
        public void Stop()
        {
            stopped = true;
        }
        public DownLoadTask(StackWidgetItem _item)
        {
            item = _item;
            url = _item.url_;
            stopped = false;
            updateTime = DateTime.Now;
            bytesTotal = _item.bytesTotal_;
            bytesDownLastUpdate = _item.bytesDown_;
            bytesDown = _item.bytesDown_;
            hasError = false;
        }

        const int BYTES_PER_KB = 1024;
        const int BYTES_PER_MB = 1024 * 1024;
        const int BYTES_PER_GB = 1024 * 1024 * 1024;

        //fix me
        private long UpdateSpeed(double timeSpan)
        {
            var bytesAdd = bytesDown - bytesDownLastUpdate;
            if (bytesAdd <= 0)
            {
                Logger.Log().Error($"bytesAdd<=0, bytesDown={bytesDown}, bytesDownLastUpdate={bytesDownLastUpdate}");
            }
            if (bytesAdd < BYTES_PER_KB)
            {
                item.speed_ = ((bytesDown - bytesDownLastUpdate) / timeSpan).ToString("f2") + "B/s";
            }
            else if (bytesAdd < BYTES_PER_MB)
            {
                item.speed_ = ((bytesDown - bytesDownLastUpdate) / timeSpan / BYTES_PER_KB).ToString("f2") + "KB/s";
            }
            else if (bytesAdd < BYTES_PER_GB)
            {
                item.speed_ = ((bytesDown - bytesDownLastUpdate) / timeSpan / BYTES_PER_MB).ToString("f2") + "MB/s";
            }
            else//.....
            {
                item.speed_ = ((bytesDown - bytesDownLastUpdate) / timeSpan / BYTES_PER_GB).ToString("f2") + "GB/s";
            }
            return (long)((bytesDown - bytesDownLastUpdate) / timeSpan);
        }

        private void UpdateFileSizeState()
        {
            //文件大小一般是MB-GB, 当然我也预料不了未来
            var bytesTotalInMB = (bytesTotal / 1024.0 / 1024).ToString("f2") + "MB";
            //fix me, 需要封装
            if (bytesDown < BYTES_PER_KB)
            {
                item.fileSizeState_ = (bytesDown * 1.0).ToString("f2") + "B/" + bytesTotalInMB;
            }
            else if (bytesDown < BYTES_PER_MB)
            {
                item.fileSizeState_ = (bytesDown * 1.0 / BYTES_PER_KB).ToString("f2") + "KB/" + bytesTotalInMB;
            }
            else if (bytesDown < BYTES_PER_GB)
            {
                item.fileSizeState_ = (bytesDown * 1.0 / BYTES_PER_MB).ToString("f2") + "MB/" + bytesTotalInMB;
            }
            else//有生之年怕是看不见
            {
                item.fileSizeState_ = (bytesDown * 1.0 / BYTES_PER_MB).ToString("f2") + "MB/" + bytesTotalInMB;
            }
        }

        //fix me, 我没有想好
        enum State : byte
        {
            Running,
            Finished,
            Error
        }
        private void UpdateState(State s)
        {
            if (s == State.Finished)
            {
                item.state_ = "下载完成";
            }
        }

        private void UpdateProgress()
        {
            item.progressValue_ = 100.0 * bytesDown / bytesTotal;
        }

        private void UpdateRemainingTime(long speedInByte)
        {
            var leftInSecond = ((bytesTotal - bytesDown) / speedInByte);
            long hour = leftInSecond / 3600;
            long min = (leftInSecond % 3600) / 60;
            long sec = leftInSecond % 3600 % 60;
            if (hour > 99)
                item.timeLeft_ = "99:99:99";
            else
            {
                item.timeLeft_ = hour.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00");
            }
        }

        public void DownloadFileImpl()
        {
            //这么写最简单，
            //1.如果FileStream让item控制，那么下载完成时，我们还要同步item。繁琐。
            //2.使用现在的方案，只需要保证本任务被stop后，不要再往文件里写。简单。
            //3.使用async。最简单，但不利于以后扩展。比如一键更新。 
            Stream ofs = null;
            Stream netStream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.AllowAutoRedirect = false;
                request.Timeout = 5000;
                request.AddRange(bytesDown);
                WebResponse respone = request.GetResponse();
                netStream = respone.GetResponseStream();
                netStream.ReadTimeout = 5000;
                string downLoadPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\download\\";
                string localFilePath = downLoadPath + item.packageName;
                //为了不反复打开 ofs, 我们将文件属性设为 FileShare.Delete，方便删除本地文件
                ofs = new FileStream(localFilePath, FileMode.Append, FileAccess.Write, FileShare.Delete);
                byte[] read = new byte[1024 * 64];
                int realReadLen = netStream.Read(read, 0, read.Length);
                if (realReadLen > 0)
                    item.state_ = "正在下载";
                while (realReadLen > 0 && !stopped)
                {
                    bool updateFlag = false;
                    DateTime newDateTime = DateTime.Now;
                    double timeSpan = newDateTime.Subtract(updateTime).TotalSeconds;
                    if (timeSpan - 1.0 > 0.0000001)
                    {
                        updateFlag = true;
                        updateTime = newDateTime;
                    }

                    lock (this)
                    {
                        if (!stopped)
                        {
                            ofs.Write(read, 0, realReadLen);
                            ofs.Flush();
                            bytesDown += realReadLen;
                            item.bytesDown_ = bytesDown;
                            if (bytesDown < bytesTotal)
                            {
                                if (updateFlag)
                                {
                                    UpdateFileSizeState();
                                    long speed = UpdateSpeed(timeSpan);
                                    UpdateRemainingTime(speed);
                                    UpdateProgress();
                                    bytesDownLastUpdate = bytesDown;
                                }
                            }
                            else if (bytesDown == bytesTotal)
                            {
                                //fix me,重复的代码
                                UpdateState(State.Finished);
                                //long speed = UpdateSpeed(timeSpan);
                                item.speed_ = "--";
                                //UpdateRemainingTime(speed);
                                UpdateProgress();
                                bytesDownLastUpdate = bytesDown;
                                //item.state_ = "下载完成";//break, or not?
                                UpdateFileSizeState();
                            }
                            else// (bytesDown > bytesTotal)
                            {
                                UpdateFileSizeState();
                                throw new Exception($"file size error bytesDown={bytesDown} with a totalBytes= {bytesTotal}");
                            }
                        }
                    }
                    realReadLen = netStream.Read(read, 0, read.Length);
                }
                //item update
            }
            catch (Exception e)
            {
                //warning user or not?
                Logger.Log().Error(e.Message);
                hasError = true;
            }
            finally
            {
                if (netStream != null)
                    netStream.Close();
                if (ofs != null)
                    ofs.Close();
                //race发生之处是用户点击按钮，改变下载状态的时刻。我们只要保护stopped变量即可
                lock (this)//用户只有stopped这个接口和本类交互
                {
                    if (!stopped)//若没有手动停止本任务
                    {
                        //stopped_用于更新按钮
                        item.stopped_ = true;
                        if (hasError)
                            item.state_ = "下载失败";
                        item.task = null;
                    }
                }
            }
        }
    }
}
