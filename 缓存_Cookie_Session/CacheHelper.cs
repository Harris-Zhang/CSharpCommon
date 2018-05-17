using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace BZ.Common
{
    /// <summary>
    /// 缓存帮助类
    /// <para>主要方法如下：</para>
    /// <para>01. GetCache(string key)  //获取数据缓存</para>
    /// <para>02. SetCache(string key, object objData)  //设置数据缓存</para>
    /// <para>03. RemoveCache(string key)   //移除指定数据缓存</para>
    /// <para>04. RemoveAll()   //移除全部缓存</para>
    /// <para></para>
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="key">键</param>
        public static object GetCache(string key)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache[key];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="objData">要缓存的数据</param>
        public static void SetCache(string key, object objData)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, objData);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="objData">要缓存的数据</param>
        /// <param name="timeOut"></param>
        public static void SetCache(string key, object objData, TimeSpan timeOut)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, objData, null, DateTime.MaxValue, timeOut, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string key, object objData, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, objData, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveCache(string key)
        {
            Cache _cache = HttpRuntime.Cache;
            _cache.Remove(key);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAll()
        {
            Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }
    }
}
