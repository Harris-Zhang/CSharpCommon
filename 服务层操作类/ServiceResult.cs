using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// 服务层返回表示层数据格式
    /// 返回前台页面也用这个
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// 返回Json信息到前台页面
        /// </summary>
        /// <param name="type">类型(ps:1 成功，-1异常，2-10 失败)</param>
        /// <param name="message">信息</param>
        /// <returns></returns>
        public static JsonMessage Message(int type, string message)
        {
            JsonMessage json = new JsonMessage()
            {
                type = type,
                message = message
            };
            return json;
        }
        /// <summary>
        /// 返回json信息到前台页面
        /// </summary>
        /// <param name="type">类型(ps:1 成功，-1异常，2-10 失败)</param>
        /// <param name="message">信息</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static JsonMessage Message(int type, string message, object value)
        {
            JsonMessage json = new JsonMessage()
            {
                type = type,
                message = message,
                value = value
            };
            return json;
        }
        /// <summary>
        /// 返回json信息到前台页面
        /// </summary>
        /// <param name="type">类型(ps:1 成功，-1异常，2-10 失败)</param>
        /// <param name="message">信息</param>
        /// <param name="value">值1</param>
        /// <param name="value2">值2</param>
        /// <returns></returns>
        public static JsonMessage Message(int type, string message, object value, object value2)
        {
            JsonMessage json = new JsonMessage()
            {
                type = type,
                message = message,
                value = value,
                value2 = value2
            };
            return json;
        }
    }

    public class JsonMessage
    {
        public int type { get; set; }

        public string message { get; set; }

        public object value { get; set; }

        public object value2 { get; set; }
    }
}
