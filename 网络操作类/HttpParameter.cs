using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BZ.Common
{
        /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResultType
    {
        String,//表示只返回字符串
        Byte//表示返回字符串和字节流
    }

    /// <summary>
    /// Post的数据格式默认为string
    /// </summary>
    public enum PostDataType
    {
        String,//字符串
        Byte,//字符串和字节流
        FilePath//表示传入的是文件
    }

    /// <summary>
    /// HTTP 请求参数
    /// </summary>
    public class HttpRequestParam
    {
        private string _url;
        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        private string _method = "GET";
        /// <summary>
        /// 请求方式默认为GET方式
        /// </summary>
        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }

        private int _timeout = 100000;
        /// <summary>
        /// 请求的超时时间
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        private int _readWriteTimeout = 30000;
        /// <summary>
        /// 写入、读取流的超时时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return _readWriteTimeout; }
            set { _readWriteTimeout = value; }
        }

        private string _accept = "text/html, application/xhtml+xml, */*";
        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept
        {
            get { return _accept; }
            set { _accept = value; }
        }

        private string _contentType = "text/html";
        /// <summary>
        /// 请求返回类型默认 text/html
        /// </summary>
        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        private string _userAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        private string _encoding = string.Empty;
        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别
        /// </summary>
        public string Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        private PostDataType _postDataType = PostDataType.String;
        /// <summary>
        /// Post的数据类型
        /// </summary>
        public PostDataType PostDataType
        {
            get { return _postDataType; }
            set { _postDataType = value; }
        }

        private string _postDate;
        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public string PostDate
        {
            get { return _postDate; }
            set { _postDate = value; }
        }

        private byte[] _postDataByte = null;
        /// <summary>
        /// Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public byte[] PostDataByte
        {
            get { return _postDataByte; }
            set { _postDataByte = value; }
        }

        private CookieCollection cookiecollection = null;
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection Cookiecollection
        {
            get { return cookiecollection; }
            set { cookiecollection = value; }
        }

        private string _cookie = string.Empty;
        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _cookie; }
            set { _cookie = value; }
        }

        private string _referer = string.Empty;
        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer
        {
            get { return _referer; }
            set { _referer = value; }
        }

        private string _cerPath = string.Empty;
        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath
        {
            get { return _cerPath; }
            set { _cerPath = value; }
        }

        private Boolean _isToLower = true;
        /// <summary>
        /// 是否设置为全文小写
        /// </summary>
        public Boolean IsToLower
        {
            get { return _isToLower; }
            set { _isToLower = value; }
        }

        private Boolean _allowAutoRedirect;
        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面
        /// </summary>
        public Boolean AllowAutoRedirect
        {
            get { return _allowAutoRedirect; }
            set { _allowAutoRedirect = value; }
        }

        private int _connectionLimit = 1024;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int ConnectionLimit
        {
            get { return _connectionLimit; }
            set { _connectionLimit = value; }
        }

        private string _proxyUserName = string.Empty;
        /// <summary>
        /// 代理 服务器用户名
        /// </summary>
        public string ProxyUserName
        {
            get { return _proxyUserName; }
            set { _proxyUserName = value; }
        }

        private string _proxyPwd = string.Empty;
        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd
        {
            get { return _proxyPwd; }
            set { _proxyPwd = value; }
        }

        private string _proxyIp = string.Empty;
        /// <summary>
        /// 代理 服务器IP
        /// </summary>
        public string ProxyIp
        {
            get { return _proxyIp; }
            set { _proxyIp = value; }
        }

        private ResultType _resultType = ResultType.String;
        /// <summary>
        /// 置返回类型String和Byte
        /// </summary>
        public ResultType ResultType
        {
            get { return _resultType; }
            set { _resultType = value; }
        }
    }
    /// <summary>
    /// HTTP 返回参数
    /// </summary>
    public class HttpResultParam
    {
        private string _cookie = string.Empty;
        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _cookie; }
            set { _cookie = value; }
        }

        private CookieCollection _cookieCollection = null;
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return _cookieCollection; }
            set { _cookieCollection = value; }
        }

        private string _resultHtml = string.Empty;
        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string ResultHtml
        {
            get { return _resultHtml; }
            set { _resultHtml = value; }
        }

        private byte[] _resultByte = null;
        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte
        {
            get { return _resultByte; }
            set { _resultByte = value; }
        }

        private WebHeaderCollection _header = new WebHeaderCollection();
        /// <summary>
        /// Header 对象
        /// </summary>
        public WebHeaderCollection Header
        {
            get { return _header; }
            set { _header = value; }
        }

    }
}
