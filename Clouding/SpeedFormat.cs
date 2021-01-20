using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clouding
{
    class FormatHelper
    {
        const int BYTES_PER_KB = 1024;
        const int BYTES_PER_MB = 1024 * 1024;
        const int BYTES_PER_GB = 1024 * 1024 * 1024;

        public static string GetSpeedString(long bytesAdd, double timeSpan)
        {
            if (bytesAdd < BYTES_PER_KB)
            {
                return (bytesAdd / timeSpan).ToString("f2") + "B/s";
            }
            else if (bytesAdd < BYTES_PER_MB)
            {
               return (bytesAdd / timeSpan / BYTES_PER_KB).ToString("f2") + "KB/s";
            }
            else if (bytesAdd < BYTES_PER_GB)
            {
                return (bytesAdd / timeSpan / BYTES_PER_MB).ToString("f2") + "MB/s";
            }
            else//.....
                return (bytesAdd / timeSpan / BYTES_PER_GB).ToString("f2") + "GB/s";
        }

        public static long CalSpeed(long bytesAdd, double timeSpan)
        {
            return (long)(bytesAdd / timeSpan);
        }

        public static string FormatFileSize(long bytesTotal, long bytesDown)
        {
            var bytesTotalInMB = (bytesTotal / 1024.0 / 1024).ToString("f2") + "MB";
            string result = string.Empty;
            if (bytesDown < BYTES_PER_KB)
            {
                result = (bytesDown * 1.0).ToString("f2") + "B/" + bytesTotalInMB;
            }
            else if (bytesDown < BYTES_PER_MB)
            {
                result = (bytesDown * 1.0 / BYTES_PER_KB).ToString("f2") + "KB/" + bytesTotalInMB;
            }
            else if (bytesDown < BYTES_PER_GB)
            {
                result = (bytesDown * 1.0 / BYTES_PER_MB).ToString("f2") + "MB/" + bytesTotalInMB;
            }
            else//有生之年怕是看不见
            {
                result = (bytesDown * 1.0 / BYTES_PER_MB).ToString("f2") + "MB/" + bytesTotalInMB;
            }
            //fix me
            // wrap string here again
            return result;
        }
    }
}
