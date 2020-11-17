using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clouding
{
    public static class Logger
    {
        /// <summary>
        /// 简单的同步日志
        /// 使用方法Logger.Log.debug("fuck");
        /// </summary>
        /// <param name="LoggerName">唯一的logger</param>
        /// <returns></returns>
        public static ILog Log(string LoggerName="SingleLogger")
        {
            //log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            return LogManager.GetLogger(LoggerName);
        }
    }
}
