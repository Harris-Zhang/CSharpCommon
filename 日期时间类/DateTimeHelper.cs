using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// 日期时间操作类
    /// <para>主要方法如下：</para>
    /// <para>1. GetDateTimeByTimeStamp(long timeStamp) //Unix时间戳转为C#格式时间</para>
    /// <para>2. GetTimeStampByDateTime(DateTime dt)    //将日期转换为Unix时间戳格式</para>
    /// <para>3. GetDate(DateTime dt, char separator = '-') //将日期格式化 年月日 ，如果日期为null，返回当前系统日期</para>
    /// <para>4. GetTime(DateTime dt, char separator = ':') //将日期格式化 时分秒 ，如果日期为null，返回当前系统日期</para>
    /// <para>5. GetFormatDate(DateTime dt, int dateModel = 0)  //格式化日期时间</para>
    /// <para>6. GetTimeSeconds(int ss, int dateModel=0)    //由秒数 得到 几天几小时几分钟</para>
    /// <para>7. DateDeff(DateTime dtStart, DateTime dtEnd) //两日期间隔(秒)</para>
    /// <para>8. </para>
    /// <para>9. </para>
    /// <para></para>
    /// </summary>
    public class DateTimeHelper
    {
        #region 时间戳
        /*
         * 首先要清楚JavaScript与Unix的时间戳的区别：
         * JavaScript时间戳：是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总毫秒数。
         * Unix时间戳：是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总秒数。
         * 可以看出JavaScript时间戳总毫秒数，Unix时间戳是总秒数。
         * 比如同样是的 2016/11/03 12:30:00 ，转换为JavaScript时间戳为 1478147400000；转换为Unix时间戳为 1478147400。
         * 
         */

        public static DateTime GetDateTimeByTimeStampJs(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            //long lTime = long.Parse(timeStamp + "0000000");
            //TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.AddMilliseconds(timeStamp);
        }

        /// <summary>
        /// Unix时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns></returns>
        public static DateTime GetDateTimeByTimeStamp(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(timeStamp);
        }

        /// <summary>
        /// 将日期转换为Unix时间戳格式
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static long GetTimeStampByDateTime(DateTime dt)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(dt - startTime).TotalSeconds;
        }
        #endregion

        /// <summary>
        /// 将日期格式化 年月日 ，如果日期为null，返回当前系统日期
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="separator">格式化分隔符(默认 '-')</param>
        /// <returns></returns>
        public static string GetDate(DateTime dt, char separator = '-')
        {
            string format = string.Format("yyyy{0}MM{1}dd", separator, separator);
            if (dt != null && dt.Equals(DBNull.Value))
                dt = DateTime.Now;
            return dt.ToString(format);
        }

        /// <summary>
        /// 将日期格式化 时分秒 ，如果日期为null，返回当前系统日期
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="separator">格式化分隔符(默认 ':')</param>
        /// <returns></returns>
        public static string GetTime(DateTime dt, char separator = ':')
        {
            string format = string.Format("hh{0}mm{1}ss", separator, separator);
            if (dt != null && dt.Equals(DBNull.Value))
                dt = DateTime.Now;
            return dt.ToString(format);
        }
        /// <summary>
        /// 格式化日期时间
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <param name="dateModel">显示模式(整型 0-N)
        /// <para>0 (yyyy-MM-dd HH:mm:ss)</para>
        /// <para>1 (yyyy/MM/dd HH:mm:ss)</para>
        /// <para>2 (yyyy-MM-dd)</para>
        /// <para>3 (yyyy/MM/dd)</para>
        /// <para>4 (yyyy年MM月dd日)</para>
        /// <para>5 (MM-dd)</para>
        /// <para>6 (MM/dd)</para>
        /// <para>7 (MM月dd日)</para>
        /// <para>8 (yyyy-MM)</para>
        /// <para>9 (yyyy/MM)</para>
        /// <para>10 (yyyy年MM月)</para>
        /// </param>
        /// <returns>日期</returns>
        public static string GetFormatDate(DateTime dt, int dateModel = 0)
        {
            if (dt != null && dt.Equals(DBNull.Value))
                dt = DateTime.Now;
            string result = "";
            switch (dateModel)
            {
                case 0:
                    result = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case 1:
                    result = dt.ToString("yyyy/MM/dd HH:mm:ss");
                    break;
                case 2:
                    result = dt.ToString("yyyy-MM-dd");
                    break;
                case 3:
                    result = dt.ToString("yyyy/MM/dd");
                    break;
                case 4:
                    result = dt.ToString("yyyy年MM月dd日");
                    break;
                case 5:
                    result = dt.ToString("MM-dd");
                    break;
                case 6:
                    result = dt.ToString("MM/dd");
                    break;
                case 7:
                    result = dt.ToString("MM月dd日");
                    break;
                case 8:
                    result = dt.ToString("yyyy-MM");
                    break;
                case 9:
                    result = dt.ToString("yyyy/MM");
                    break;
                case 10:
                    result = dt.ToString("yyyy年MM月");
                    break;
                default:
                    result = dt.ToString();
                    break;
            }
            return result;
        }

        /// <summary>
        /// 由秒数 得到 几天几小时几分钟
        /// </summary>
        /// <param name="ss">总秒数</param>
        /// <param name="dateModel">显示模式(整型 0-N)
        /// <para>0  n天n小时n分钟n秒</para>
        /// </param>
        /// <returns>返回经历的时间</returns>
        public static string GetTimeSeconds(int ss, int dateModel=0)
        {
            TimeSpan ts = new TimeSpan(0, 0, ss);
            int d = ts.Days;
            int h = ts.Hours;
            int m = ts.Minutes;
            int s = ts.Seconds;

            string result = "";
            switch (dateModel)
            {
                case 0:
                    result = d + "天" + h + "小时" + m + "分钟" + s + "秒";
                    break;

            }
            return result;

            //string r = "";
            //int day, hour, minute, second;
            //if (ss >= 86400) //天,
            //{
            //    day = Convert.ToInt16(ss / 86400);
            //    hour = Convert.ToInt16((ss % 86400) / 3600);
            //    minute = Convert.ToInt16((ss % 86400 % 3600) / 60);
            //    second = Convert.ToInt16(ss % 86400 % 3600 % 60);
                 
            //        r = day + ("day") + hour + ("hour") + minute + ("minute") + second + ("second");
                 

            //}
            //else if (ss >= 3600)//时,
            //{
            //    hour = Convert.ToInt16(ss / 3600);
            //    minute = Convert.ToInt16((ss % 3600) / 60);
            //    second = Convert.ToInt16(ss % 3600 % 60);
                
            //        r = hour + ("hour") + minute + ("minute") + second + ("second");
                
            //}
            //else if (ss >= 60)//分
            //{
            //    minute = Convert.ToInt16(ss / 60);
            //    second = Convert.ToInt16(ss % 60);
            //    r = minute + ("minute") + second + ("second");
            //}
            //else
            //{
            //    second = Convert.ToInt16(ss);
            //    r = second + ("second");
            //}
            //return r;

        }
        /// <summary>
        /// 两日期间隔(秒)
        /// </summary>
        /// <param name="dtStart">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        /// <returns>间隔秒</returns>
        public static int DateDeff(DateTime dtStart, DateTime dtEnd)
        {
            TimeSpan ts1 = new TimeSpan(dtStart.Ticks);
            TimeSpan ts2 = new TimeSpan(dtEnd.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return (int)ts.TotalSeconds;
        }
    }
}
