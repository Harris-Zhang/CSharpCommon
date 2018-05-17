using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// DES 加密、解密
    /// <para>主要方法如下：</para>
    /// <para>01. Encrypt(string text, string key)  //DES 加密字符串</para>
    /// <para>02. Decrypt(string text, string key)  //DES 解密字符串</para>
    /// </summary>
    public class DESCryption
    {
        private static string dKey = "0123456789";

        #region 加密字符串
        /// <summary>
        /// DES 加密字符串
        /// </summary>
        /// <param name="text">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, dKey);
        }
        /// <summary>
        /// DES 加密字符串
        /// </summary>
        /// <param name="text">明文</param>
        /// <param name="key">密码(默认 0123456789)</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (string.IsNullOrEmpty(key))
                key = dKey;
            byte[] inputByteArray = Encoding.Default.GetBytes(text);
            byte[] keyByteArray = Encoding.Default.GetBytes(key);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            byte[] sKey = new byte[8];
            byte[] sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            cs.Close();
            ms.Close();
            return ret.ToString();
        }
        #endregion

        #region 解密字符串
        /// <summary>
        /// DES 解密字符串
        /// </summary>
        /// <param name="text">明文</param>
        /// <returns>密文</returns>
        public static string Decrypt(string text)
        {
            return Decrypt(text, dKey);
        }
        /// <summary>
        /// DES 解密字符串
        /// </summary>
        /// <param name="text">明文</param>
        /// <param name="key">密码(默认 0123456789)</param>
        /// <returns>密文</returns>
        public static string Decrypt(string text, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (string.IsNullOrEmpty(key))
                key = dKey;
            byte[] inputByteArray = new byte[text.Length / 2];
            for (int x = 0; x < text.Length / 2; x++)
            {
                int i = (Convert.ToInt32(text.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            byte[] keyByteArray = Encoding.Default.GetBytes(key);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            byte[] sKey = new byte[8];
            byte[] sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion


    }
}
