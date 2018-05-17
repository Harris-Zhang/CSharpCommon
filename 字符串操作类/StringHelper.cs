using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BZ.Common
{
    /// <summary>
    /// 字符串操作类
    /// <para>主要方法如下：</para>
    /// <para>01. GetStrLength()    //获取字符串实际长度 (中文长度=2)</para>
    /// <para>02. GetClipStringByByte()    //截取指定长度字符串(中英文混合字符串)</para>
    /// <para>03. ToSBC()   //转全角(SBC case)</para>
    /// <para>04. ToDBC()    //转半角(DBC case) </para>
    /// <para>05. GetStrToList()  //把字符串按照分隔符转换成List</para>
    /// <para>06. GetListToStr()   //把List数组 按照分隔符转换字符串</para>
    /// <para>07. GetStrToArray()   //把字符串按照分隔符转换成 string[]</para>
    /// <para>08. DelLastChar()  //删除最后结尾的字符</para>
    /// <para>09. DelRepeatBySpeater(  //把字符串按照分隔符转换List 并去除重复</para>
    /// <para>10. DelRepeatBySpeater()  //把字符串按照分隔符去除重复</para>
    /// <para>11. DelInvisibleChar()    //删除不可见字符</para>
    /// <para>12. Similarity()    //两字符串相似度计算方法(编辑距离算法LevenshteinDistance又称EditDistance)</para>
    /// <para>13. GetSqlLike()  //sql语句模糊查询</para>
    /// <para>14. Trim()    //去除前后空格</para>
    /// <para></para>
    /// <para></para>
    /// <para></para>
    /// <para></para>
    /// <para></para>
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 获取字符串实际长度 (中文长度=2)
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>长度</returns>
        public static int GetStrLength(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }

        /// <summary>
        /// 截取指定长度字符串(中英文混合字符串)
        /// </summary>
        /// <param name="str">待处理字符串</param>
        /// <param name="leng">截取长度</param>
        /// <param name="isEllipsis">结尾添加…(默认 是)</param>
        /// <returns>处理后字符串</returns>
        public static string GetClipStringByByte(string str, int len, bool isEllipsis = true)
        {
            if (str == null || len <= 0)
                return "";
            if (str.Length < len && str.Length * 2 < len)
                return str;
            #region 暂时不用

            //string sContent = str;
            //int iTmp = len;
            //char[] arrC;
            //if (sContent.Length >= iTmp) //防止因为中文的原因使ToCharArray溢出
            //    arrC = str.ToCharArray(0, iTmp);
            //else
            //    arrC = str.ToCharArray(0, sContent.Length);
            //int i = 0;
            //int iLength = 0;
            //foreach (char ch in arrC)
            //{
            //    iLength++;
            //    int k = (int)ch;
            //    if (k > 127 || k < 0)
            //        i += 2;
            //    else
            //        i++;
            //    if (i > iTmp)
            //    {
            //        iLength--;
            //        break;
            //    }
            //    else if (i == iTmp)
            //    {
            //        break;
            //    }
            //}
            //if (iLength < str.Length && isEllipsis)
            //    sContent = sContent.Substring(0, iLength - 3) + "...";
            //else
            //    sContent = sContent.Substring(0, iLength);
            //return sContent;

            #endregion

            bool isShowFix = false;
            if (len % 2 == 1)
            {
                isShowFix = true;
                len--;
            }
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                try
                {
                    tempString += str.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }

            byte[] mybyte = System.Text.Encoding.Default.GetBytes(str);
            if (isShowFix && mybyte.Length > len && isEllipsis)
                tempString += "…";
            return tempString;
        }

        /// <summary>
        /// 转全角(SBC case)
        /// 全角空格为12288，半角空格为32
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248  
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToSBC(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            char[] ch = str.ToCharArray();
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i] == 32)
                {
                    ch[i] = (char)12288;
                    continue;
                }
                if (ch[i] < 127)
                    ch[i] = (char)(ch[i] + 65248);
            }
            return new string(ch);
        }
        /// <summary>
        /// 转半角(DBC case) 
        /// 全角空格为12288，半角空格为32  
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDBC(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            char[] array = str.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 12288)
                {
                    array[i] = (char)32;
                    continue;
                }
                if (array[i] > 65280 && array[i] < 65375)
                {
                    array[i] = (char)(array[i] - 65248);
                }
            }
            return new string(array);
        }

        /// <summary>
        /// 把字符串按照分隔符转换成List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符(默认是',')</param>
        /// <param name="toLower">是否转换小写(默认 false)</param>
        /// <returns>转换后List</returns>
        public static List<string> GetStrToList(string str, char speater = ',', bool toLower = false)
        {

            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(str))
                return list;
            string[] sp = str.Split(speater);
            foreach (string s in sp)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                        strVal = s.ToLower();
                    list.Add(strVal);
                }
            }
            return list;
        }

        /// <summary>
        /// 把List数组 按照分隔符转换字符串
        /// </summary>
        /// <param name="list">List数组</param>
        /// <param name="speater">分隔符(默认是',')</param>
        /// <returns></returns>
        public static string GetListToStr(List<string> list, string speater = ",")
        {
            if (list == null || list.Count == 0)
                return "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把字符串按照分隔符转换成 string[]
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符(默认是',')</param>
        /// <returns>转换后数组</returns>
        public static string[] GetStrToArray(string str, char speater = ',')
        {
            if (string.IsNullOrEmpty(str))
                return new string[] { };
            return str.Split(speater);
        }

        /// <summary>
        /// 删除最后结尾的字符
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="lastChar">结尾字符(默认",")</param>
        /// <returns>删除后字符串</returns>
        public static string DelLastChar(string str, string lastChar = ",")
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return str.Substring(0, str.LastIndexOf(lastChar));
        }

        /// <summary>
        /// 把字符串按照分隔符转换List 并去除重复
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符(默认是',')</param>
        /// <returns>转换后List</returns>
        public static List<string> DelRepeatByList(string str, char speater = ',')
        {
            List<string> list = GetStrToList(str, speater);
            if (string.IsNullOrEmpty(str))
                return list;
            return list.Distinct<string>().ToList();
        }

        /// <summary>
        /// 把字符串按照分隔符去除重复
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符(默认是',')</param>
        /// <param name="merge">合并的分隔符(默认是",")</param>
        /// <returns>去除重复后的字符串</returns>
        public static string DelRepeatByStr(string str, char speater = ',', string merge = ",")
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return String.Join(merge, str.Split(speater).Distinct().ToArray());
        }

        /// <summary>
        /// 删除不可见字符
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>去除后的字符串</returns>
        public static string DelInvisibleChar(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            Regex reg = new Regex(@"[\f\n\r\t\v]*", RegexOptions.IgnoreCase);
            str = reg.Replace(str, "");
            reg = new Regex(@"[ ]+");//合并多个空格为一个
            return reg.Replace(str, " ");
        }

        /// <summary>
        /// 两字符串相似度计算方法(编辑距离算法LevenshteinDistance又称EditDistance)
        /// </summary>
        /// <param name="str1">字符串1</param>
        /// <param name="str2">字符串2</param>
        /// <returns></returns>
        public static string Similarity(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1)
                || string.IsNullOrEmpty(str2))
            {
                return "0";
            }


            int n = str1.Length;
            int m = str2.Length;
            int[,] d = new int[n + 1, m + 1];

            int temp = 0;
            char ch1, ch2;
            int i = 0, j = 0;

            for (i = 0; i <= n; i++)
            {
                //初始化第一列
                d[i, 0] = i;
            }
            for (j = 0; j <= m; j++)
            {
                //初始化第一行
                d[0, j] = j;
            }

            for (i = 1; i <= n; i++)
            {
                ch1 = str1[i - 1];
                for (j = 1; j <= m; j++)
                {
                    ch2 = str2[j - 1];
                    if (ch1 == ch2)
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    //对比获取最小值
                    int min = 0, first = d[i - 1, j] + 1, second = d[i, j - 1] + 1, third = d[i - 1, j - 1] + temp;
                    min = first < second ? first : second;
                    min = min < third ? min : third;

                    d[i, j] = min;
                }
            }

            //输出百分比 保留两位小数
            int maxLenth = str1.Length > str2.Length ? str1.Length : str2.Length;
            decimal percent = (1 - (decimal)d[n, m] / maxLenth) * 100;
            return string.Format("{0:F2}%", percent).Replace(".00", "");
        }
        /// <summary>
        /// sql语句模糊查询
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetSqlLike(string value)
        {
            return "%" + value + "%";
        }
        /// <summary>
        /// 去除前后空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Trim(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return str.Trim();
        }
        /// <summary>
        /// 截取字符串数组中每个字符串，然后再合并
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符(默认是',')</param>
        /// <param name="merge">合并的分隔符(默认是",")</param>
        /// <returns></returns>
        public static string SubStr(string str, int start, int length, char speater = ',', string merge = ",")
        {
            if (string.IsNullOrEmpty(str))
                return "";
            string[] tmp = str.Split(speater).Distinct().ToArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = tmp[i].Substring(start, length);
            }
            return String.Join(merge, tmp);
        }
    }
}
