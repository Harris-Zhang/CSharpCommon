using System;
using System.Security.Cryptography;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// RSA加密
    /// <para>主要方法如下：</para>
    /// <para>01. RSAKey(out string xmlPrivateKey, out string xmlPublicKey) //RSA 的密钥产生 产生私钥 和公钥</para>
    /// <para>02. Encrypt(string text, string xmlPublicKey) //RSA 加密字符串</para>
    /// <para>03. Decrypt(string text, string xmlPrivateKey)    //RSA 解密字符串</para>
    /// <para></para>
    /// </summary>
    public class RSACryption
    {
        #region RSA 加密、解密

        #region RSA 的密钥产生 产生私钥 和公钥
        /// <summary>
        ///  RSA 的密钥产生 产生私钥 和公钥
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="xmlPublicKey">公钥</param>
        public static void RSAKey(out string xmlPrivateKey, out string xmlPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlPrivateKey = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }
        #endregion

        #region 加密 字符串
        /// <summary>
        /// RSA 加密字符串
        /// </summary>
        /// <param name="text">待加密的字符串</param>
        /// <param name="xmlPublicKey">XML公钥</param>
        /// <returns>加密后字符串</returns>
        public static string Encrypt(string text, string xmlPublicKey)
        {
            byte[] PlainTextArray = (new UnicodeEncoding()).GetBytes(text);
            return Encrypt(PlainTextArray, xmlPublicKey);
            
        }
        /// <summary>
        /// RSA 加密字符串
        /// </summary>
        /// <param name="text">待加密的byte[]</param>
        /// <param name="xmlPublicKey">XML公钥</param>
        /// <returns>加密后字符串</returns>
        public static string Encrypt(byte[] text, string xmlPublicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPublicKey);
                byte[] CypheTextArray = rsa.Encrypt(text, false);
                string result = Convert.ToBase64String(CypheTextArray);
                return result;
            }
        }
        #endregion

        #region 解密 字符串
        /// <summary>
        /// RSA 解密字符串
        /// </summary>
        /// <param name="text">待解密字符串</param>
        /// <param name="xmlPrivateKey">xml私钥</param>
        /// <returns>解密后字符串</returns>
        public static string Decrypt(string text, string xmlPrivateKey)
        {
            byte[] bEncrypt = Convert.FromBase64String(text); 
            //byte[] PlainTextArray = (new UnicodeEncoding()).GetBytes(text);
            return Decrypt(bEncrypt, xmlPrivateKey);
        }
        /// <summary>
        /// RSA 解密字符串
        /// </summary>
        /// <param name="text">待解密 byte[]</param>
        /// <param name="xmlPrivateKey">xml私钥</param>
        /// <returns>解密后字符串</returns>
        public static string Decrypt(byte[] text, string xmlPrivateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPrivateKey);

                byte[] DypherTextArray = rsa.Decrypt(text, false);
                string result = (new UnicodeEncoding()).GetString(DypherTextArray);
                return result;
            }
        }
        #endregion

        #endregion

        #region RSA签名
        //TODO

        #endregion 
    }
}
