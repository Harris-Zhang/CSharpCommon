using System.Web;

namespace BZ.Common
{
    /// <summary>
    /// Session操作类
    /// <para>主要方法如下：</para>
    /// <para>1. GetSession(string keyName) //获取Session值</para>
    /// <para>2. SetSession(string keyName, object val) // 设置Session</para>
    /// <para>3. Clear()    // 除清所有键和值</para>
    /// <para>4. Remove(string keyName) // 移除指定键的Session</para>
    /// <para>5. RemoveAll()    // 移除所有键和值</para>
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="keyName">键</param>
        /// <returns></returns>
        public static object GetSession(string keyName)
        {
            return HttpContext.Current.Session[keyName];
        }

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="keyName">键</param>
        /// <param name="val">值</param>
        public static void SetSession(string keyName, object val)
        {
            Remove(keyName);
            HttpContext.Current.Session.Add(keyName, val);
        }
        
        /// <summary>
        /// 除清所有键和值
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        /// <summary>
        /// 移除指定键的Session
        /// </summary>
        /// <param name="keyName">键</param>
        public static void Remove(string keyName)
        {
            HttpContext.Current.Session.Remove(keyName);
        }

        /// <summary>
        /// 移除所有键和值
        /// </summary>
        public static void RemoveAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}
