using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace BZ.Common
{
    public class Log4Helper
    { 
        private static ILog ILogger; 
        private static string configFile = "log4net.config";

        static Log4Helper()
        {
            if (File.Exists(Log4NetConfigFile))
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    XmlConfigurator.ConfigureAndWatch(new FileInfo(Log4NetConfigFile));
                }
                else
                {
                    XmlConfigurator.Configure(new FileInfo(Log4NetConfigFile));
                }
            }
            else
            {
                BasicConfigurator.Configure();
            }
             
            ILogger= GetLogger(typeof(Log4Helper));
        }

        #region Abbributes
        public static string Log4NetConfigFile
        {
            get
            {
                object log4Config = CacheHelper.GetCache("Log4NetConfigFile");
                if (ValidateHelper.IsNullOrEmpty(log4Config))
                {
                    //return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), configFile);
                    //判断是Web程序还是window程序
                    if (HttpContext.Current != null)
                    {
                        //return Path.Combine(HttpRuntime.AppDomainAppPath, configFile);
                        log4Config = System.Web.HttpContext.Current.Server.MapPath("~/bin/" + configFile);
                    }
                    else
                    {
                        log4Config = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), configFile);
                    }
                    CacheHelper.SetCache("Log4NetConfigFile", log4Config);
                }

                return log4Config.ToString();
            }
        }

        public static ILog GetLogger(System.Type type)
        {
            return LogManager.GetLogger(type);
        }
        #endregion 

        //#region  记录执行操作日志 log type

        //public static void Error(string message)
        //{
        //    optLogger.Error(message);
        //}

        //public static void Warning(string message)
        //{
        //    optLogger.Warn(message);
        //}

        //public static void Fatal(string message)
        //{
        //    optLogger.Fatal(message);

        //}

        //public static void Info(string message)
        //{
        //    optLogger.Info(message);
        //}


        //#endregion

        #region 执行SQL日志
        public static void Info(string message)
        {
            ILogger.Info(message);
        }

        public static void Warn(string message)
        {
            ILogger.Warn(message);
        }

        public static void Error(string message)
        {
            ILogger.Error(message);
        }
        #endregion
 
        public static void ClearOldLogFiles(string keepDays)
        {
            int days = 0;
            int.TryParse(keepDays, out days);
            if (days <= 0)
            {
                return;
            }

            try
            {
                DateTime now = DateTime.Now;

                Hierarchy hierarchy = (Hierarchy)LogManager.GetLoggerRepository();
                foreach (IAppender appender in hierarchy.Root.Appenders)
                {
                    if (appender is RollingFileAppender)
                    {
                        string logPath = ((RollingFileAppender)appender).File;
                        DirectoryInfo dir = new DirectoryInfo(logPath.Substring(0, logPath.LastIndexOf("\\")));

                        foreach (FileInfo file in dir.GetFiles())
                        {
                            if (file.LastWriteTime < now.AddDays(-days))
                            {
                                file.Delete();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 记录执行SQL语句和执行SQL的时间
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">接收时间</param>
        public static void WriteSqlLogInfo(string sql, DateTime start, DateTime end)
        {
            if (ConfigHelper.GetConfig("DBLOG").ToLower() == "true")
            {
                TimeSpan span = end - start;
                Log4Helper.Info("************ SQL: " + sql);
                Log4Helper.Info("************ Start: " + start.ToString("yyyy/MM/dd HH:mm:ss") + "." + start.Millisecond.ToString("000")
                    + ", End: " + end.ToString("yyyy/MM/dd HH:mm:ss") + "." + end.Millisecond.ToString("000")
                    + ", Cost: " + span.TotalMilliseconds.ToString("0.000"));
                Log4Helper.Info("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
        }

        /// <summary>
        /// 记录执行SQL语句的异常
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="ex">异常</param>
        public static void WriteSqlLogError(string sql, Exception ex)
        {
            Log4Helper.Error("************ SQL: " + sql);
            Log4Helper.Error("************ exMessage: " + ex.Message);
            Log4Helper.Error("************ exStackTrace: " + ex.StackTrace);
            Log4Helper.Error("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        }

    }
}
