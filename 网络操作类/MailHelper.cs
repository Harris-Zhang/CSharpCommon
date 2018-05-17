using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// 发送邮件帮助类
    /// <para>smtp地址、smtp端口号、发件人、密码等通过配置文件获取(SmtpHost、SmtpPort、FromEmailAddress、FromEmailPwd)</para>
    /// </summary>
    public class MailHelper
    {
        #region 邮件属性
        #region 发 邮件相关属性[SMTP服务器，SMTP端口，发件人地址，发件人密码(加密过的)]
        private string _smtpHost = string.Empty;
        /// <summary>
        /// Smtp 服务器地址
        /// </summary>
        public string SmtpHost
        {
            get
            {
                if (ValidateHelper.IsNullOrEmpty(_smtpHost))
                {
                    //获取配置文件中的Smtp地址
                    _smtpHost = ConfigHelper.GetConfig("SmtpHost");
                }
                return _smtpHost;
            }
        }
        private int _smtpPort = -1;
        /// <summary>
        /// Smtp服务器端口 默认25
        /// </summary>
        public int SmtpPort
        {
            get
            {
                if (_smtpPort == -1)
                {
                    //获取配置文件中的Smtp端口
                    string port = ConfigHelper.GetConfig("SmtpPort");
                    if (ValidateHelper.IsNetPort(port))
                    {
                        _smtpPort = ConverHelper.ToInt(port);
                    }
                    else
                    {
                        //获取不正确端口 默认25
                        _smtpPort = 25;
                    }
                }
                return _smtpPort;
            }

        }
        private string _fromAddress = string.Empty;
        /// <summary>
        /// 发送者Email地址
        /// </summary>
        public string FromAddress
        {
            get
            {
                if (ValidateHelper.IsNullOrEmpty(_fromAddress))
                {
                    _fromAddress = ConfigHelper.GetConfig("FromEmailAddress");
                }
                return _fromAddress;
            }

        }
        private string _fromPwd = string.Empty;
        /// <summary>
        /// 发送者 Email 密码
        /// </summary>
        public string FromPwd
        {
            get
            {
                if (ValidateHelper.IsNullOrEmpty(_fromPwd))
                {
                    //这里密码通过DES加密过的，所以需要解密
                    _fromPwd = DESCryption.Decrypt(ConfigHelper.GetConfig("FromEmailPwd"));
                }
                return _fromPwd;
            }

        }
        #endregion

        #region 收 邮件相关属性[收件人,抄送人,密送人,标题,正文内容,是否启用Html,附件]

        private List<string> _toList;
        /// <summary>
        /// 收件人邮件列表(abc@163.com)
        /// </summary>
        public List<string> ToList
        {
            get { return _toList; }
            set { _toList = value; }
        }

        private List<string> _ccList;
        /// <summary>
        /// 抄送人邮件列表(abc@163.com)
        /// </summary>
        public List<string> CcList
        {
            get { return _ccList; }
            set { _ccList = value; }
        }

        private List<string> _bccList;
        /// <summary>
        /// 密送人邮件列表(abc@163.com)
        /// </summary>
        public List<string> BccList
        {
            get { return _bccList; }
            set { _bccList = value; }
        }

        private string _subject;
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private string _body;
        /// <summary>
        /// 邮件正文内容
        /// </summary>
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        private bool _isHtml;
        /// <summary>
        /// 正文是否启用Html格式
        /// </summary>
        public bool IsHtml
        {
            get { return _isHtml; }
            set { _isHtml = value; }
        }
        /// <summary>
        /// 邮件附件
        /// </summary>
        private List<Attachment> _attachmentList = new List<Attachment>();
        #endregion

        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造
        /// </summary>
        public MailHelper()
        {

        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="to">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文内容</param>
        /// <param name="isHtml">正文是否启用Html格式</param>
        public MailHelper(List<string> to, string subject, string body, bool isHtml = true)
        {
            this._toList = to;
            this._subject = subject;
            this._body = body;
            this._isHtml = isHtml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to">收件人</param>
        /// <param name="cc">抄送人</param>
        /// <param name="bcc">密送人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文内容</param>
        /// <param name="isHtml">正文是否启用Html格式</param>
        public MailHelper(List<string> to, List<string> cc, List<string> bcc, string subject, string body, bool isHtml = true)
        {
            this._toList = to;
            this._ccList = cc;
            this._bccList = bcc;
            this._subject = subject;
            this._body = body;
            this._isHtml = isHtml;
        }

        #endregion

        #region 添加附件 因不想在外部引用 所以在用方法实现
        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="fileName">附件名称路径</param>
        public void AddAttachment(string fileName)
        {
            Attachment attach = new Attachment(fileName);
            _attachmentList.Add(attach);
        }
        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mediaType"></param>
        public void AddAttachment(string fileName, string mediaType)
        {
            Attachment attach = new Attachment(fileName, mediaType);
            _attachmentList.Add(attach);
        }
        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="contentStream"></param>
        /// <param name="name"></param>
        public void AddAttachment(Stream contentStream, string name)
        {
            Attachment attach = new Attachment(contentStream, name);
            _attachmentList.Add(attach);
        }
        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="contentStream"></param>
        /// <param name="name"></param>
        /// <param name="mediaType"></param>
        public void AddAttachment(Stream contentStream, string name, string mediaType)
        {
            Attachment attach = new Attachment(contentStream, name, mediaType);
            _attachmentList.Add(attach);
        }

        #endregion

        /// <summary>
        /// 发送邮件
        /// </summary>
        public void Send()
        {
            #region smtp设置
            //实例化一个SmtpClient
            SmtpClient smtp = new SmtpClient();
            //将Smtp设定为通过网络发送
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //Smtp服务器是否启用ssl加密
            smtp.EnableSsl = false;
            //Smtp 服务器地址
            smtp.Host = this._smtpHost;
            //Smtp 端口号 默认是25
            smtp.Port = this._smtpPort;
            //如果 UseDefaultCredentials 屬性設定為 false, 設定值 Credentials 屬性將使用的認證，連接到伺服器時。 
            //如果 UseDefaultCredentials 屬性設定為 false 和 Credentials 屬性尚未設定，則郵件會以匿名方式傳送到伺服器。
            smtp.UseDefaultCredentials = false;
            //认证用户名&密码
            smtp.Credentials = new NetworkCredential(this._fromAddress, this._fromPwd);
            #endregion
            //实例化邮件类
            MailMessage mailMsg = new MailMessage();
            //设置邮件优先级(Low,Normal,High) 通常Normal即可
            mailMsg.Priority = MailPriority.Normal;
            //发件人
            mailMsg.From = new MailAddress(this._fromAddress, "**系统管理员", Encoding.GetEncoding(936));
            //邮件主题
            mailMsg.Subject = this._subject;
            //这里非常重要，如果你的邮件标题包含中文，这里一定要指定，否则对方收到的极有可能是乱码
            mailMsg.SubjectEncoding = Encoding.GetEncoding(936);
            //邮件正文是否是HTML格式  
            mailMsg.IsBodyHtml = this._isHtml;
            ////邮件正文 
            //mailMsg.Body = this._body;
            ////邮件正文的编码， 设置不正确， 接收者会收到乱码 
            //mailMsg.BodyEncoding = Encoding.GetEncoding(936);
            //邮件正文 
            AlternateView htmlBody = AlternateView.CreateAlternateViewFromString(this._body, Encoding.GetEncoding(936), "text/html");
            mailMsg.AlternateViews.Add(htmlBody);


            //收件人
            if (this._toList != null && this._toList.Count > 0)
            {
                foreach (string to in this._toList)
                {
                    mailMsg.To.Add(to);
                }
            }
            //抄送人
            if (this._ccList != null && this._ccList.Count > 0)
            {
                foreach (string cc in this._ccList)
                {
                    mailMsg.CC.Add(cc);
                }
            }
            //密送人
            if (this._bccList != null && this._bccList.Count > 0)
            {
                foreach (string bcc in this._bccList)
                {
                    mailMsg.Bcc.Add(bcc);
                }
            }
            //添加附件
            if (this._attachmentList != null && this._attachmentList.Count > 0)
            {
                foreach (Attachment attach in this._attachmentList)
                {
                    mailMsg.Attachments.Add(attach);
                }
            }
            //发送邮件
            smtp.Send(mailMsg);
        }
    }
}
