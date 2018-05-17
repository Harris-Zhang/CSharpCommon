using System;
using System.Security.Cryptography;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// MD5加密 主要用于加密密码
    /// <para>主要方法如下：</para>
    /// <para>1. MD5(string text)   //MD5 加密 主要用于加密密码</para>
    /// </summary>
    public class MD5Cryption
    {
        /// <summary>
        /// MD5 加密 主要用于加密密码
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <returns>加密后字符串</returns>
        public static string MD5(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            MD5 md5 = new MD5CryptoServiceProvider();//创建MD5密码服务提供程序
            byte[] input = Encoding.Default.GetBytes(text);//将要加密的字符串转换为字节数组
            byte[] result = md5.ComputeHash(input);//计算传入的字节数组的哈希值
            md5.Clear(); //释放资源
            //return Convert.ToBase64String(result).Replace("-", "");
            return Convert.ToBase64String(result);//将加密后的字节数组转换为加密字符串
        }
    }
}
