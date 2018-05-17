using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// 配置文件帮助类
    /// App.config web.config
    /// <para>主要方法如下：</para>
    /// <para>01.GetConfig  //获取App.config 或者web.config 中配置信息</para>
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 获取App.config 或者web.config 中配置信息
        /// 并加入缓存中
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            string cacheKey = "AppSettings-"+key;
            //先从缓存中获取
            object objData = CacheHelper.GetCache(cacheKey);
            if (objData == null)
            {
                try
                {
                    objData = ConfigurationManager.AppSettings[key];
                    if (objData != null)
                    {
                        //添加到缓存中
                        CacheHelper.SetCache(cacheKey, objData);
                    }
                    else
                    {
                        objData = "";
                    }
                }
                catch
                {

                }
            }
            return objData.ToString();
        }
        /// <summary>
        /// 获取配置文件ERP区域
        /// </summary>
        public static string GetAreaEnt
        {
            get
            {
                return GetConfig("ERP_ENT");
            }
        }
    }
}
