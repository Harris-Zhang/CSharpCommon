using System;
using System.Text.RegularExpressions;
using System.Web;

#region 使用方法
/* 
 *  修改Global.asax文件
 *  直接调用
 *  void Application_BeginRequest(object sender, EventArgs e)
 *  {
 *      string q="<div style='position:fixed;top:0px;width:100%;height:100%;background-color:white;color:green;font-weight:bold;border-bottom:5px solid #999;'><br>您的提交带有不合法参数,谢谢合作!<br><br>了解更多请点击:<a href='http://www.kuiyu.net'>奎宇工作室</a></div>";
 *      if (Request.Cookies != null)
 *      {
 *          if (WebSafe.CookieData())
 *          {
 *              Response.Write("您提交的Cookie数据有恶意字符！");
 *              Response.End();
 *          }
 *      }
 *
 *      if (Request.UrlReferrer != null)
 *      {
 *          if (WebSafe.referer())
 *          {
 *              Response.Write("您提交的Referrer数据有恶意字符！");
 *              Response.End();
 *          }
 *      }
 *
 *      if (Request.RequestType.ToUpper() == "POST")
 *      {
 *          if (WebSafe.PostData())
 *          {
 *              Response.Write("您提交的Post数据有恶意字符！");
 *              Response.End();
 *          }
 *      }
 *      if (Request.RequestType.ToUpper() == "GET")
 *      {
 *          if (WebSafe.GetData())
 *          {
 *              Response.Write("您提交的Get数据有恶意字符！");
 *              Response.End();
 *          }
 *      }
 *  }
 */
#endregion
namespace BZ.Common
{
    /// <summary>
    /// 网站安全(防止跨站脚本攻击的代码)
    /// <para>主要方法如下：</para>
    /// <para>1. IsPostData //验证Post数据是否安全</para>
    /// <para>2. IsGetData  //验证Get数据是否安全</para>
    /// <para>3. IsCookieData   //验证Cookie数据是否安全</para>
    /// <para>4. IsReferer  //验证 Referrer数据是否安全</para>
    /// <para></para>
    /// </summary>
    public class WebSafe
    {
        private const string StrRegex = @"^\+/v(8|9)|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|<\s*script\b|<\s*img\b|\bEXEC\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";

        /// <summary>
        /// 检测数据是否安全
        /// </summary>
        /// <param name="inputData">待检测的数据</param>
        /// <returns></returns>
        public static bool CheckData(string inputData)
        {
            if (Regex.IsMatch(inputData, StrRegex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证Post数据是否安全
        /// </summary>
        /// <returns>true(不安全)</returns>
        public static bool IsPostData()
        {
            bool result = false;
            try
            {
                for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
                {
                    string data = HttpContext.Current.Request.Form[i].ToString();
                    result = CheckData(data);
                    if (result == true)
                        break;
                }
            }
            catch 
            { 
                result = true;
            }


            return result;
        }

        /// <summary>
        /// 验证Get数据是否安全
        /// </summary>
        /// <returns>true(不安全)</returns>
        public static bool IsGetData()
        {
            bool result = false;
            try
            {
                for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
                {
                    string data = HttpContext.Current.Request.QueryString[i].ToString();
                    result = CheckData(data);
                    if (result == true)
                        break;
                }
            }
            catch  
            { 
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 验证Cookie数据是否安全
        /// </summary>
        /// <returns>true(不安全)</returns>
        public static bool IsCookieData()
        {
            bool result = false;
            try
            {
                for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
                {
                    string data = HttpContext.Current.Request.Cookies[i].Value.ToLower();
                    result = CheckData(data);
                    if (result == true)
                    {
                        break;
                    }
                }
            }
            catch  
            { 
                result = true;
            }
            return result;

        }
        /// <summary>
        /// 验证 Referrer数据是否安全
        /// </summary>
        /// <returns>true(不安全)</returns>
        public static bool IsReferer()
        {
            bool result = false;
            try
            {
                string data = HttpContext.Current.Request.UrlReferrer.ToString();
                result = CheckData(data);
            }
            catch  
            { 
                result = true;
            }
            return result;
        }


    }
}
