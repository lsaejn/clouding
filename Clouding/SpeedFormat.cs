using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clouding
{
    class SpeedFormat
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
    }
}
