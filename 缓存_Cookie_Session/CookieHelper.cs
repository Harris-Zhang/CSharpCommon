using System;
using System.Text.RegularExpressions;
using System.Web;

namespace BZ.Common
{
    /// <summary>
    /// Cookie帮助类
    /// <para>主要方法如下：</para>
    /// <para>01. GetCookie(string keyName)  //获得cookie值</para>
    /// <para>03. SetCookie(string cookieName, string keyValue)  //写cookie值 单一项值</para>
    /// <para>04. SetCookies(string cookieName, string keyName, string keyValue) //写cookie值 多值</para>
    /// <para>05. ClearCookie(string cookieName) //清除指定Cookie</para>
    /// </summary>
    public class CookieHelper
    {
        #region 获取cookie值
        /// <summary>
        /// 获得cookie值
        /// </summary>
        /// <param name="keyName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string keyName)
        {
            var httpCookie = HttpContext.Current.Request.Cookies[keyName];
            return httpCookie != null ? httpCookie.Value : "";
        }

        /// <summary>
        /// 获得cookie值
        /// </summary>
        /// <param name="cookieName">Cookie 名称</param>
        /// <param name="keyName">项</param>
        /// <returns>值</returns>
        public static string GetCookie(string cookieName, string keyName)
        {
            var httpCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (httpCookie != null && httpCookie[keyName] != null)
            {
                return HttpUtility.UrlDecode(httpCookie.Values[keyName]);
            }
            return "";
        }
        #endregion

        #region 设置cookie值 单一值

        /// <summary>
        /// 写cookie值 
        /// 存放cookie值（单一项值）
        /// </summary>
        /// <param name="cookieName">名称</param>
        /// <param name="keyValue">值</param>
        public static void SetCookie(string cookieName, string keyValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = keyValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 写cookie值 
        /// 存放cookie值（单一项值）
        /// </summary>
        /// <param name="cookieName">名称</param>
        /// <param name="keyValue">值</param>
        /// <param name="domain">域</param>
        public static void SetCookie(string cookieName, string keyValue, string domain)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = keyValue;
            if (domain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(domain) > -1  && IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = domain;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值 
        /// 存放cookie值（单一项值）
        /// </summary>
        /// <param name="cookieName">名称</param>
        /// <param name="keyValue">值</param>
        /// <param name="domain">域</param>
        /// <param name="expires">cookie 保存时长 单位分种</param>
        public static void SetCookie(string cookieName, string keyValue, string domain, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = keyValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            if (domain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(domain) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = domain;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        #endregion

        #region 设置cookie值 多值

        /// <summary>
        /// 写cookie值 多值
        /// </summary>
        /// <param name="cookieName">cookies名字</param>
        /// <param name="keyName">cookie项的名称，cookie[keyName][]</param>
        /// <param name="keyValue">cookie项的值，cookie[keyName][keyValue]</param>
        public static void SetCookies(string cookieName, string keyName, string keyValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Values[keyName] = HttpUtility.UrlEncode(keyValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值 多值
        /// </summary>
        /// <param name="cookieName">cookies名字</param>
        /// <param name="keyName">cookie项的名称，cookie[keyName][]</param>
        /// <param name="keyValue">cookie项的值，cookie[keyName][keyValue]</param>
        /// <param name="domain">cookie域属性</param>
        public static void SetCookies(string cookieName, string keyName, string keyValue, string domain )
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Values[keyName] = HttpUtility.UrlEncode(keyValue);
            if (domain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(domain) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = domain;
            HttpContext.Current.Response.AppendCookie(cookie);
        }


        /// <summary>
        /// 写cookie值 多值
        /// </summary>
        /// <param name="cookieName">cookies名字</param>
        /// <param name="keyName">cookie项的名称，cookie[keyName][]</param>
        /// <param name="keyValue">cookie项的值，cookie[keyName][keyValue]</param>
        /// <param name="domain">cookie域属性</param>
        /// <param name="expires">cookies 有效时间 单位天</param>
        public static void SetCookies(string cookieName, string keyName, string keyValue, string domain, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Values[keyName] = HttpUtility.UrlEncode(keyValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            if (domain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(domain) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = domain;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region 清楚cookie

        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookieName">cookiename</param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        public static void ClearCookie(string cookieName, string domain)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values.Clear();
            cookie.Expires = DateTime.Now.AddYears(-1);
            //string cookieDomain = ConfigFactory.GetConfig().CookieDomain.Trim();
            if (domain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(domain) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = domain;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        #endregion

        /// <summary>
        /// 是否为有效域
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns></returns>
        private static bool IsValidDomain(string host)
        {
            Regex r = new Regex(@"^\d+$");
            if (host.IndexOf(".") == -1)
            {
                return false;
            }
            return !r.IsMatch(host.Replace(".", string.Empty));
        }
    }
}
