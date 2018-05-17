using System;
using System.Net;
using System.Web;

namespace BZ.Common
{
    /// <summary>
    /// 网络帮助类
    /// <para>主要方法如下：</para>
    /// <para>01. GetUserIp  //取得客户端真实IP。如果有代理则取第一个非内网地址</para>
    /// <para>02. LocalHostName //获取本机的计算机名</para>
    /// <para>03. LanIp //获取本机的局域网IP</para>
    /// <para>04. WanIp //获取本机在Internet网络的广域网IP</para>
    /// <para></para>
    /// </summary>
    public class NetHelper
    {
        #region 获取客户端真实IP
        /// <summary>
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址
        /// </summary>
        public static string GetUserIp
        {
            get
            {
                string result = String.Empty;
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (result != null && result != String.Empty)
                {
                    //可能有代理
                    if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。
                            result = result.Replace(" ", "").Replace("'", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (ValidateHelper.IsNetIp(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];    //找到不是内网的地址
                                }
                            }
                        }
                        else if (ValidateHelper.IsNetIp(result)) //代理即是IP格式 ,IsIPAddress判断是否是IP的方法,
                            return result;
                        else
                            result = null;    //代理中的内容 非IP，取IP
                    }


                }

                string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null
                                 && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (null == result || result == String.Empty)
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (result == null || result == String.Empty)
                    result = HttpContext.Current.Request.UserHostAddress;
                if (result == "::1")
                    result = "127.0.0.1";   
                return result;
            }
        }
        #endregion

        #region 获取本机的计算机名
        public static string LocalHostName
        {
            get { return Dns.GetHostName(); }
        }
        #endregion

        #region 获取本机的局域网IP
        /// <summary>
        /// 获取本机的局域网IP
        /// </summary>        
        public static string LanIp
        {
            get
            {
                //获取本机的IP列表,IP列表中的第一项是局域网IP，第二项是广域网IP
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                //如果本机IP列表为空，则返回空字符串
                if (addressList.Length < 1)
                {
                    return "";
                }

                //返回本机的局域网IP
                return addressList[0].ToString();
            }
        }
        #endregion

        #region 获取本机在Internet网络的广域网IP
        /// <summary>
        /// 获取本机在Internet网络的广域网IP
        /// </summary>        
        public static string WanIp
        {
            get
            {
                //获取本机的IP列表,IP列表中的第一项是局域网IP，第二项是广域网IP
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                //如果本机IP列表小于2，则返回空字符串
                if (addressList.Length < 2)
                {
                    return "";
                }

                //返回本机的广域网IP
                return addressList[1].ToString();
            }
        }
        #endregion
    }
}
