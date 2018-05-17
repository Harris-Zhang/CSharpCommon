using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BZ.Common
{
    public class JsonHelper
    {
        /// <summary>
        /// 获取json 某个值
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string GetJsonValue(string cc, string json)
        {
            string str = json.Substring(1, json.Length - 2);
            string strJson = str.Replace("\"", "");
            string[] sJson = strJson.Split(',');
            foreach (string s in sJson)
            {
                string[] tmp = s.Split(':');
                if (cc == tmp[0])
                {
                    return tmp[1];
                }
            }
            return "";
        }


    }
}
