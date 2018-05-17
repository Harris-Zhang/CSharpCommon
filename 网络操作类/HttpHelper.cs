using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace BZ.Common
{
    /// <summary>
    /// Http连接帮助类
    /// <para>主要方法如下：</para>
    /// <para>01. HttpGet   //GET请求</para>
    /// <para>02. HttpPost  //Post 请求</para>
    /// <para>03. GetHttpHtml   //多条件设定，获取数据</para>
    /// </summary>
    public class HttpHelper
    {
        #region 模拟GET
        /// <summary>
        ///  GET请求
        /// </summary>
        /// <param name="url">Get Url</param>
        /// <param name="postData">参数数据</param>
        /// <returns></returns>
        public static string HttpGet(string url, string postData)
        {
            string param = postData == "" ? "" : "?" + postData;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + param);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        #endregion

        #region 模拟POST
        /// <summary>
        /// Post 请求
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="postData">参数</param>
        /// <returns></returns>
        public static string HttpPost(string url, string postData)
        {
            Stream outStream = null;
            Stream inStream = null;
            StreamReader sReader = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] data = encoding.GetBytes(postData);
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outStream = request.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                response = (HttpWebResponse)request.GetResponse();
                inStream = response.GetResponseStream();
                sReader = new StreamReader(inStream, encoding);
                string content = sReader.ReadToEnd();
                return content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 多条件设定，获取数据

        private Encoding encoding = Encoding.Default;
        private HttpWebRequest request = null;
        private HttpWebResponse response = null;

        /// <summary>
        /// 多条件设定，获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public HttpResultParam GetHttpHtml(HttpRequestParam param)
        {
            //准备参数
            SetRequestParam(param);
            return GetHttpRequestData(param);
        }

        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="strPostdata">传入的数据Post方式,get方式传NUll或者空字符串都可以</param>
        /// <returns>string类型的响应数据</returns>
        private HttpResultParam GetHttpRequestData(HttpRequestParam objhttpitem)
        {
            //返回参数
            HttpResultParam result = new HttpResultParam();
            try
            {
                #region 得到请求的response
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    result.Header = response.Headers;
                    if (response.Cookies != null)
                    {
                        result.CookieCollection = response.Cookies;
                    }
                    if (response.Headers["set-cookie"] != null)
                    {
                        result.Cookie = response.Headers["set-cookie"];
                    }

                    MemoryStream _stream = new MemoryStream();
                    //GZIIP处理
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //开始读取流并设置编码方式
                        //new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                        //.net4.0以下写法
                        _stream = GetMemoryStream(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
                    }
                    else
                    {
                        //开始读取流并设置编码方式
                        //response.GetResponseStream().CopyTo(_stream, 10240);
                        //.net4.0以下写法
                        _stream = GetMemoryStream(response.GetResponseStream());
                    }
                    //获取Byte
                    byte[] RawResponse = _stream.ToArray();
                    //是否返回Byte类型数据
                    if (objhttpitem.ResultType == ResultType.Byte)
                    {
                        result.ResultByte = RawResponse;
                    }
                    //从这里开始我们要无视编码了
                    if (encoding == null)
                    {
                        string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                        //<meta(.*?)charset([\s]?)=[^>](.*?)>
                        Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0)
                        {
                            charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                            encoding = Encoding.GetEncoding(charter);
                        }
                        else
                        {
                            if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                    }
                    //得到返回的HTML
                    result.ResultHtml = encoding.GetString(RawResponse);
                    //最后释放流
                    _stream.Close();
                }
                #endregion
            }
            catch (WebException ex)
            {
                //这里是在发生异常时返回的错误信息
                result.ResultHtml = "String Error";
                response = (HttpWebResponse)ex.Response;
            }
            if (objhttpitem.IsToLower)
            {
                result.ResultHtml = result.ResultHtml.ToLower();
            }
            return result;
        }

        /// <summary>
        /// 4.0以下.net版本取数据使用
        /// </summary>
        /// <param name="streamResponse">流</param>
        private static MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);
            // write the required bytes  
            while (bytesRead > 0)
            {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return _stream;
        }

        /// <summary>
        /// 设置准备参数
        /// </summary>
        /// <param name="param"></param>
        private void SetRequestParam(HttpRequestParam param)
        {
            //设置证书
            SetCer(param.CerPath, param.Url);
            //设置代理
            SetProxy(param.ProxyUserName, param.ProxyPwd, param.ProxyIp);
            //提交方式 (Post，Get等)
            request.Method = param.Method;
            //请求超时
            request.Timeout = param.Timeout;
            //读写超时
            request.ReadWriteTimeout = param.ReadWriteTimeout;

            request.Accept = param.Accept;

            request.ContentType = param.ContentType;

            request.UserAgent = param.UserAgent;
            //编码
            SetEncoding(param.Encoding);
            //设置cookie
            SetCookie(param.Cookie, param.Cookiecollection);
            //来源地址
            request.Referer = param.Referer;
            //是否执行跳转功能
            request.AllowAutoRedirect = param.AllowAutoRedirect;
            //设置Post数据
            SetPostData(param);
            //最大连接数
            if (param.ConnectionLimit > 0)
            {
                request.ServicePoint.ConnectionLimit = param.ConnectionLimit;
            }
        }

        #region 给Url添加http|https
        /// <summary>
        /// url 前缀是否添加了http|https
        /// </summary>
        /// <param name="URL"></param>
        /// <returns>返回添加后的URL</returns>
        private string GetUrl(string URL)
        {
            if (!(URL.Contains("http://") || URL.Contains("https://")))
            {
                URL = "http://" + URL;
            }
            return URL;
        }
        #endregion

        #region 设定证书
        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="cerPath">证书路径</param>
        /// <param name="url">URL地址</param>
        private void SetCer(string cerPath,string url)
        {
            if (!string.IsNullOrEmpty(cerPath))
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(url));
                //创建证书文件
                X509Certificate objx509 = new X509Certificate(cerPath);
                //添加到请求里
                request.ClientCertificates.Add(objx509);
            }
            else
            {
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(url));
            }
        }
        /// <summary>
        /// 回调验证证书问题
        /// </summary>
        /// <param name="sender">流对象</param>
        /// <param name="certificate">证书</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="errors">SslPolicyErrors</param>
        /// <returns>bool</returns>
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受    
            return true;
        }
        #endregion

        #region 设置编码
        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="encode">编码(uft-8 等等之类)</param>
        private void SetEncoding(string encode)
        {
            //读取数据时的编码方式
            if (string.IsNullOrEmpty(encode) || encode.ToLower().Trim() == "null")
                encoding = null;
            else
                encoding = Encoding.GetEncoding(encode);
        }
        #endregion

        #region 设置cookie
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="cookieCollection"></param>
        private void SetCookie(string cookie,CookieCollection cookieCollection)
        {
            if (!string.IsNullOrEmpty(cookie))
            {
                ////设置Cookie
                request.Headers[HttpRequestHeader.Cookie] = cookie;
            }
            if (cookieCollection != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookieCollection);
            }
        }
        #endregion

        #region 设置Post 数据
        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="param"></param>
        private void SetPostData(HttpRequestParam param)
        {
            //验证在得到结果时是否有传入数据
            if (request.Method.Trim().ToLower().Contains("post"))
            {
                //写入Byte类型
                if (param.PostDataType == PostDataType.Byte)
                {
                    //验证在得到结果时是否有传入数据
                    if (param.PostDataByte != null && param.PostDataByte.Length > 0)
                    {
                        request.ContentLength = param.PostDataByte.Length;
                        request.GetRequestStream().Write(param.PostDataByte, 0, param.PostDataByte.Length);
                    }
                }//写入文件
                else if (param.PostDataType == PostDataType.FilePath)
                {
                    StreamReader r = new StreamReader(param.PostDate, encoding);
                    byte[] buffer = Encoding.Default.GetBytes(r.ReadToEnd());
                    r.Close();
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                else
                {
                    //验证在得到结果时是否有传入数据
                    if (!string.IsNullOrEmpty(param.PostDate))
                    {
                        byte[] buffer = Encoding.Default.GetBytes(param.PostDate);
                        request.ContentLength = buffer.Length;
                        request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        #endregion

        #region 设置代理
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="ip"></param>
        private void SetProxy(string userName, string pwd, string ip)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(pwd) && string.IsNullOrEmpty(ip))
            {
                //不需要设置
            }
            else
            {
                //设置代理服务器
                WebProxy myProxy = new WebProxy(ip, false);
                //建议连接
                myProxy.Credentials = new NetworkCredential(userName, pwd);
                //给当前请求对象
                request.Proxy = myProxy;
                //设置安全凭证
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
        }

        #endregion

        #endregion
    }
}
