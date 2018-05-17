using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace BZ.Common
{
    public class LogHelper
    { 
        static string symbol = "---------------------------------------------------------------------------------------";

        #region  记录执行操作日志 log type

        public static void Warning(string msg)
        {
            Write($"<Warning> { msg}");
        }
        public static void Error(string msg)
        {
            Write($"<Error> { msg}");
        }
        public static void Info(string msg)
        {
            Write($"<Message> {msg}");
        }
 
        #endregion
         
        static string LogPath
        {
            get
            {
                string path = ConfigHelper.GetConfig("OptLogPath");
                return path;
            }
        }

        private static void Write(string message)
        {
            var logName = DateTime.Now.ToString("yyMMdd") + ".log";
            string fileFullName = LogPath + "\\" + logName;
            try
            {
                if (!FileHelper.IsExistDir(LogPath))
                    FileHelper.CreateDir(LogPath);
                using (StreamWriter sw = new StreamWriter(fileFullName, true, Encoding.Default))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : " + message);
                    sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + symbol);
                    sw.WriteLine(sb);
                    sw.Dispose();
                    sw.Close();
                }
            }
            catch { }
        }

    }
}
