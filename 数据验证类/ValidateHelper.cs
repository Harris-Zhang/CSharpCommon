using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace BZ.Common
{
    #region 记录正则表达式验证
    /*
     * 1.1 校验数字的表达式
     *      数字：^[0-9]*$
     *      n位的数字：^\d{n}$
     *      至少n位的数字：^\d{n,}$
     *      m-n位的数字：^\d{m,n}$
     *      零和非零开头的数字：^(0|[1-9][0-9]*)$
     *      非零开头的最多带两位小数的数字：^([1-9][0-9]*)+(.[0-9]{1,2})?$
     *      带1-2位小数的正数或负数：^(\-)?\d+(\.\d{1,2})?$
     *      正数、负数、和小数：^(\-|\+)?\d+(\.\d+)?$
     *      有两位小数的正实数：^[0-9]+(.[0-9]{2})?$
     *      有1~3位小数的正实数：^[0-9]+(.[0-9]{1,3})?$
     *      非零的正整数：^[1-9]\d*$ 或 ^([1-9][0-9]*){1,3}$ 或 ^\+?[1-9][0-9]*$
     *      非零的负整数：^\-[1-9][]0-9"*$ 或 ^-[1-9]\d*$
     *      非负整数：^\d+$ 或 ^[1-9]\d*|0$
     *      非正整数：^-[1-9]\d*|0$ 或 ^((-\d+)|(0+))$
     *      非负浮点数：^\d+(\.\d+)?$ 或 ^[1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0$
     *      非正浮点数：^((-\d+(\.\d+)?)|(0+(\.0+)?))$ 或 ^(-([1-9]\d*\.\d*|0\.\d*[1-9]\d*))|0?\.0+|0$
     *      正浮点数：^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$ 或 ^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$
     *      负浮点数：^-([1-9]\d*\.\d*|0\.\d*[1-9]\d*)$ 或 ^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$
     *      浮点数：^(-?\d+)(\.\d+)?$ 或 ^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$
     *  1.2 校验字符的表达式
     *      汉字：^[\u4e00-\u9fa5]{0,}$
     *      英文和数字：^[A-Za-z0-9]+$ 或 ^[A-Za-z0-9]{4,40}$
     *      长度为3-20的所有字符：^.{3,20}$
     *      由26个英文字母组成的字符串：^[A-Za-z]+$
     *      由26个大写英文字母组成的字符串：^[A-Z]+$
     *      由26个小写英文字母组成的字符串：^[a-z]+$
     *      由数字和26个英文字母组成的字符串：^[A-Za-z0-9]+$
     *      由数字、26个英文字母或者下划线组成的字符串：^\w+$ 或 ^\w{3,20}$
     *      中文、英文、数字包括下划线：^[\u4E00-\u9FA5A-Za-z0-9_]+$
     *      中文、英文、数字但不包括下划线等符号：^[\u4E00-\u9FA5A-Za-z0-9]+$ 或 ^[\u4E00-\u9FA5A-Za-z0-9]{2,20}$
     *      可以输入含有^%&',;=?$\"等字符：[^%&',;=?$\x22]+
     *      禁止输入含有~的字符：[^~\x22]+
     *  1.3 特殊需求表达式
     *      Email地址：^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$
     *      域名：[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(/.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+/.?
     *      InternetURL：[a-zA-z]+://[^\s]* 或 ^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$
     *      手机号码：^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$
     *      电话号码("XXX-XXXXXXX"、"XXXX-XXXXXXXX"、"XXX-XXXXXXX"、"XXX-XXXXXXXX"、"XXXXXXX"和"XXXXXXXX)：^(\(\d{3,4}-)|\d{3.4}-)?\d{7,8}$ 
     *      国内电话号码(0511-4405222、021-87888822)：\d{3}-\d{8}|\d{4}-\d{7}
     *      身份证号(15位、18位数字)：^\d{15}|\d{18}$
     *      短身份证号码(数字、字母x结尾)：^([0-9]){7,18}(x|X)?$ 或 ^\d{8,18}|[0-9x]{8,18}|[0-9X]{8,18}?$
     *      帐号是否合法(字母开头，允许5-16字节，允许字母数字下划线)：^[a-zA-Z][a-zA-Z0-9_]{4,15}$
     *      密码(以字母开头，长度在6~18之间，只能包含字母、数字和下划线)：^[a-zA-Z]\w{5,17}$
     *      强密码(必须包含大小写字母和数字的组合，不能使用特殊字符，长度在8-10之间)：^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$  
     *      日期格式：^\d{4}-\d{1,2}-\d{1,2}
     *      一年的12个月(01～09和1～12)：^(0?[1-9]|1[0-2])$
     *      一个月的31天(01～09和1～31)：^((0?[1-9])|((1|2)[0-9])|30|31)$ 
     *      中文字符的正则表达式：[\u4e00-\u9fa5]
     *      双字节字符：[^\x00-\xff]    (包括汉字在内，可以用来计算字符串的长度(一个双字节字符长度计2，ASCII字符计1))
     *      空白行的正则表达式：\n\s*\r    (可以用来删除空白行)
     *      HTML标记的正则表达式：<(\S*?)[^>]*>.*?</\1>|<.*? />    (网上流传的版本太糟糕，上面这个也仅仅能部分，对于复杂的嵌套标记依旧无能为力)
     *      首尾空白字符的正则表达式：^\s*|\s*$或(^\s*)|(\s*$)    (可以用来删除行首行尾的空白字符(包括空格、制表符、换页符等等)，非常有用的表达式)
     *      腾讯QQ号：[1-9][0-9]{4,}    (腾讯QQ号从10000开始)
     *      中国邮政编码：[1-9]\d{5}(?!\d)    (中国邮政编码为6位数字)
     *      IP地址：\d+\.\d+\.\d+\.\d+    (提取IP地址时有用)
     *      IP地址：((?:(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d)\\.){3}(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d))
     *  1.4 钱的输入格式
     *      有四种钱的表示形式我们可以接受:"10000.00" 和 "10,000.00", 和没有 "分" 的 "10000" 和 "10,000"：^[1-9][0-9]*$
     *      这表示任意一个不以0开头的数字,但是,这也意味着一个字符"0"不通过,所以我们采用下面的形式：^(0|[1-9][0-9]*)$
     *      一个0或者一个不以0开头的数字.我们还可以允许开头有一个负号：^(0|-?[1-9][0-9]*)$
     *      这表示一个0或者一个可能为负的开头不为0的数字.让用户以0开头好了.把负号的也去掉,因为钱总不能是负的吧.下面我们要加的是说明可能的小数部分：^[0-9]+(.[0-9]+)?$
     *      必须说明的是,小数点后面至少应该有1位数,所以"10."是不通过的,但是 "10" 和 "10.2" 是通过的：^[0-9]+(.[0-9]{2})?$
     *      这样我们规定小数点后面必须有两位,如果你认为太苛刻了,可以这样：^[0-9]+(.[0-9]{1,2})?$
     *      这样就允许用户只写一位小数.下面我们该考虑数字中的逗号了,我们可以这样：^[0-9]{1,3}(,[0-9]{3})*(.[0-9]{1,2})?$
     *      1到3个数字,后面跟着任意个 逗号+3个数字,逗号成为可选,而不是必须：^([0-9]+|[0-9]{1,3}(,[0-9]{3})*)(.[0-9]{1,2})?$
     * 
     * 
    */
    #endregion

    /// <summary>
    /// 数据验证类
    /// <para>主要方法如下：</para>
    /// <para>01. IsMatch(string inputStr, string patternStr) //验证字符串是否匹配正则表达式描述的规则  </para>
    /// <para>02. IsNumber(string str)    //验证数值是否正确 (包括整数、小数)</para>
    /// <para>03. IsNumberPosInt(string str)   //验证正整数是否正确</para>
    /// <para>04. IsNumberInt(string str) //验证整数是否正确</para>
    /// <para>05. IsNumberDecimal(string str) //验证小数是否正确</para>
    /// <para>06. IsDate(string str)  //验证日期是否正确</para>
    /// <para>07. IsMobile(string str, bool IsStrict = false) //验证手机号码是否正确</para>
    /// <para>08. IsIDCard(string str, bool IsStrict = false) //验证身份证号码是否正确</para>
    /// <para>09. IsEmail(string str, bool IsStrict = false)  //验证邮箱是否正确</para>
    /// <para>10. IsBase64(string str)    //验证Base64编码 是否正确</para>
    /// <para>11. IsFax(string str)   //验证传真是否正确</para>
    /// <para>12. IsZipCode(string str)   //验证邮政编码</para>
    /// <para>13. IsChinese(string str)   //验证是否只包含汉字</para>
    /// <para>14. IsDBC(string str)   //验证是否是半角</para>
    /// <para>15. IsSBC(string str)   //验证是否是全角</para>
    /// <para>16. IsUrl(string str)   //验证URL是否正确</para>
    /// <para>17. IsNetMac(string str)    //验证Mac地址是否正确</para>
    /// <para>18. IsNetIp(string str) //验证IP地址是否正确</para>
    /// <para>19. IsNetPort(string str)   //验证端口号是否正确</para>
    /// <para>20. IsNullOrEmpty(object data)  //判断对象是否为空，为空返回true</para>
    /// <para>21. IsDataTableNotData(DataTable dt) //判断DataTable 有没有数据</para>
    /// <para></para>
    /// <para></para>
    /// <para></para>
    /// <para></para>
    /// <para></para>
    /// </summary>
    public static class ValidateHelper
    {
        #region 正则表达式 匹配方法
        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMatch(string inputStr, string patternStr)
        {
            return IsMatch(inputStr, patternStr, false, false);
        }

        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <param name="ifIgnoreCase">匹配时是否不区分大小写</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMatch(string inputStr, string patternStr, bool ifIgnoreCase)
        {
            return IsMatch(inputStr, patternStr, ifIgnoreCase, false);
        }

        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <param name="ifIgnoreCase">匹配时是否不区分大小写</param>  
        /// <param name="ifValidateWhiteSpace">是否验证空白字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMatch(string inputStr, string patternStr, bool ifIgnoreCase, bool ifValidateWhiteSpace)
        {
            if (!ifValidateWhiteSpace && IsNullOrEmpty(inputStr))//.NET 4.0 新增IsNullOrWhiteSpace 方法，便于对用户做处理
                return false;//如果不要求验证空白字符串而此时传入的待验证字符串为空白字符串，则不匹配  
            Regex regex = null;
            if (ifIgnoreCase)
                regex = new Regex(patternStr, RegexOptions.IgnoreCase);//指定不区分大小写的匹配  
            else
                regex = new Regex(patternStr);
            return regex.IsMatch(inputStr);
        }
        #endregion

        #region 验证数字
        /// <summary>
        /// 验证数值是否正确 (包括整数、小数)
        /// </summary>
        /// <param name="str">数值</param>
        /// <returns>返回bool类型</returns>
        public static bool IsNumber(string str)
        {
            //TODO
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^(-)?\d+(\.\d+)?$";
            return Regex.IsMatch(str, regex);
        }

        /// <summary>
        /// 验证正整数是否正确
        /// </summary>
        /// <param name="str">正整数</param>
        /// <returns>返回bool类型</returns>
        public static bool IsNumberPosInt(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^[1-9]+\d*$";
            return Regex.IsMatch(str, regex);
        }


        /// <summary>
        /// 验证整数是否正确
        /// </summary>
        /// <param name="str">整数</param>
        /// <returns>返回bool类型</returns>
        public static bool IsNumberInt(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^[+-]?\d*$";
            return Regex.IsMatch(str, regex);
        }
        /// <summary>
        /// 验证小数是否正确
        /// </summary>
        /// <param name="str">小数</param>
        /// <returns>返回bool类型</returns>
        public static bool IsNumberDecimal(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^[+-]?\d*[.]?\d*$";
            return Regex.IsMatch(str, regex);
        }

        #endregion

        #region 验证日期
        /// <summary>
        /// 验证日期是否正确
        /// </summary>
        /// <param name="str">日期</param>
        /// <returns>返回bool类型</returns>
        /// <remarks>
        /// 可判断格式如下（其中-可替换为/，不影响验证)
        /// YYYY | YYYY-MM | YYYY-MM-DD | YYYY-MM-DD HH:MM:SS | YYYY-MM-DD HH:MM:SS.FFF
        /// </remarks>
        public static bool IsDate(string str)
        {
            if (IsNullOrEmpty(str))
            {
                return false;
            }
            string regexDate = @"[1-2]{1}[0-9]{3}((-|\/|\.){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|\/|\.){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";
            if (Regex.IsMatch(str, regexDate))
            {
                //以下各月份日期验证，保证验证的完整性
                int _IndexY = -1;
                int _IndexM = -1;
                int _IndexD = -1;
                if (-1 != (_IndexY = str.IndexOf("-")))
                {
                    _IndexM = str.IndexOf("-", _IndexY + 1);
                    _IndexD = str.IndexOf(":");
                }
                else
                {
                    _IndexY = str.IndexOf("/");
                    _IndexM = str.IndexOf("/", _IndexY + 1);
                    _IndexD = str.IndexOf(":");
                }
                //不包含日期部分，直接返回true
                if (-1 == _IndexM)
                    return true;
                if (-1 == _IndexD)
                {
                    _IndexD = str.Length + 3;
                }
                int iYear = Convert.ToInt32(str.Substring(0, _IndexY));
                int iMonth = Convert.ToInt32(str.Substring(_IndexY + 1, _IndexM - _IndexY - 1));
                int iDate = Convert.ToInt32(str.Substring(_IndexM + 1, _IndexD - _IndexM - 4));
                //判断月份日期
                if ((iMonth < 8 && 1 == iMonth % 2) || (iMonth > 8 && 0 == iMonth % 2))
                {
                    if (iDate < 32)
                        return true;
                }
                else
                {
                    if (iMonth != 2)
                    {
                        if (iDate < 31)
                            return true;
                    }
                    else
                    {
                        //闰年
                        if ((0 == iYear % 400) || (0 == iYear % 4 && 0 < iYear % 100))
                        {
                            if (iDate < 30)
                                return true;
                        }
                        else
                        {
                            if (iDate < 29)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        #endregion

        #region 验证手机号码（中国电信：China Telecom中国移动：China Mobile 中国联通：China Unicom）、电话号码
        /**
         * 手机号码: 
         * 13[0-9], 14[5,7], 15[0, 1, 2, 3, 5, 6, 7, 8, 9], 17[6, 7, 8], 18[0-9], 170[0-9]
         * 移动号段: 134,135,136,137,138,139,150,151,152,157,158,159,182,183,184,187,188,147,178,1705
         * 联通号段: 130,131,132,155,156,185,186,145,176,1709
         * 电信号段: 133,153,180,181,189,177,1700
         */
        /// <summary>
        /// 验证手机号码是否正确
        /// </summary>
        /// <param name="str">手机号</param>
        /// <param name="IsStrict">是否严格验证</param>
        /// <returns>返回bool类型</returns>
        public static bool IsMobile(string str, bool IsStrict = false)
        {
            if (IsNullOrEmpty(str))
                return false;
            //string regex=@"^1[0123456789]\d{9}$";
            string regex = IsStrict ? @"^[1][358]\d{9}$" : @"[1]\d{10}";
            return Regex.IsMatch(str, regex);
        }
        /// <summary>
        /// 验证手机号码(中国移动)是否正确
        /// </summary>
        /// <param name="str">手机号</param>
        /// <returns>返回bool类型</returns>
        public static bool IsMobileMob(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"(^1(3[4-9]|4[7]|5[0-27-9]|7[8]|8[2-478])\d{8}$)|(^1705\d{7}$)";
            /**
             * 中国移动：China Mobile
             * 134,135,136,137,138,139,150,151,152,157,158,159,182,183,184,187,188,147,178,1705
             */
            return Regex.IsMatch(str, regex);
        }
        /// <summary>
        /// 验证手机号码(中国电信)是否正确
        /// </summary>
        /// <param name="str">手机号</param>
        /// <returns>返回bool类型</returns>
        public static bool IsMobileTel(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"(^1(33|53|77|8[019])\d{8}$)|(^1700\d{7}$)";
            /**
             * 中国电信：China Telecom
             * 133,153,180,181,189,177,1700
             */
            return Regex.IsMatch(str, regex);
        }
        /// <summary>
        /// 验证手机号码(中国联通)是否正确
        /// </summary>
        /// <param name="str">手机号</param>
        /// <returns>返回bool类型</returns>
        public static bool IsMobileUni(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"(^1(3[0-2]|4[5]|5[56]|7[6]|8[56])\d{8}$)|(^1709\d{7}$)";
            /**
             * 中国联通：China Unicom
             * 130,131,132,155,156,185,186,145,176,1709
             */
            return Regex.IsMatch(str, regex);
        }


        /// <summary>
        /// 验证电话号码是否正确
        /// </summary>
        /// <param name="str">电话号码</param>
        /// <returns>返回bool类型</returns>
        public static bool IsMobilePhone(string str)
        {
            string regex = @"(^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$)|(^0?1[35]\d{9}$)";
            return Regex.IsMatch(str, regex, RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证身份证是否有效
        /// <summary>
        /// 验证身份证号码是否正确
        /// </summary>
        /// <param name="str">身份证号码(支持18位和15位身份证)</param>
        /// <param name="IsStrict">是否严格验证</param>
        /// <returns>返回bool类型</returns>
        public static bool IsIDCard(string str, bool IsStrict = false)
        {
            if (IsNullOrEmpty(str))
                return false;
            if (str.Length == 18)
            {
                return IsIDCard18(str, IsStrict);
            }
            else if (str.Length == 15)
            {
                return IsIDCard15(str, IsStrict);
            }
            return false;
        }

        /// <summary>
        /// 验证输入字符串为18位的身份证号码
        /// </summary>
        /// <param name="str">18位身份证号码</param>
        /// <param name="IsStrict">是否严格验证</param>
        /// <returns>返回bool类型</returns>
        public static bool IsIDCard18(string str, bool IsStrict = false)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$";
            if (!IsStrict)
            {
                return Regex.IsMatch(str, regex, RegexOptions.IgnoreCase);
            }

            long n = 0;
            if (long.TryParse(str.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(str.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(str.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = str.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = str.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != str.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }
        /// <summary>
        /// 验证输入字符串为15位的身份证号码
        /// </summary>
        /// <param name="str">15位身份证号</param>
        /// <param name="IsStrict">是否严格验证</param>
        /// <returns>返回bool类型</returns>
        public static bool IsIDCard15(string str, bool IsStrict = false)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$";
            if (!IsStrict)
            {
                return Regex.IsMatch(str, regex, RegexOptions.IgnoreCase);
            }
            long n = 0;
            if (long.TryParse(str, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(str.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = str.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

        #endregion

        #region 验证邮箱
        /// <summary>
        /// 验证邮箱是否正确
        /// </summary>
        /// <param name="str">邮箱地址</param>
        /// <param name="IsStrict">是否严格验证</param>
        /// <returns>返回bool类型</returns>
        public static bool IsEmail(string str, bool IsStrict = false)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = IsStrict ? @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" : @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return Regex.IsMatch(str, regex, RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证编码格式
        /// <summary>
        /// 验证Base64编码 是否正确
        /// </summary>
        /// <param name="str">Base64编码</param>
        /// <returns>返回bool类型</returns>
        public static bool IsBase64(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"[A-Za-z0-9\+\/\=]";
            return Regex.IsMatch(str, regex);
        }

        #endregion

        #region 验证传真
        /// <summary>
        /// 验证传真是否正确
        /// </summary>
        /// <param name="str">传真地址</param>
        /// <returns>返回bool类型</returns>
        public static bool IsFax(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$";
            return Regex.IsMatch(str, regex);
        }
        #endregion

        #region 验证邮政编码
        /// <summary>
        /// 验证邮政编码
        /// </summary>
        /// <param name="str">邮政编码</param>
        /// <returns>返回bool类型</returns>
        public static bool IsZipCode(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^[1-9]\d{5}$";
            return Regex.IsMatch(str, regex);
        }

        #endregion

        #region 验证是否只包含汉字
        /// <summary>
        /// 验证是否只包含汉字
        /// </summary>
        /// <param name="str">输入的字符串</param>
        /// <returns>返回bool类型</returns>
        public static bool IsChinese(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^[\u4e00-\u9fa5]+$";
            return Regex.IsMatch(str, regex);
        }
        /// <summary>
        /// 验证是包含汉字
        /// </summary>
        /// <param name="str">输入的字符串</param>
        /// <returns>返回bool类型</returns>
        public static bool IsChineseContain(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"[\u4e00-\u9fa5]+";
            return Regex.IsMatch(str, regex);
        }
        #endregion

        #region 验证字符串全角、半角
        /// <summary>
        /// 验证是否是半角
        /// </summary>
        /// <param name="str">输入的字符串</param>
        /// <returns>返回bool类型</returns>
        public static bool IsDBC(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            return Encoding.Default.GetByteCount(str) == str.Length;
        }
        /// <summary>
        /// 验证是否是全角
        /// </summary>
        /// <param name="str">输入的字符串</param>
        /// <returns>返回bool类型</returns>
        public static bool IsSBC(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            return Encoding.Default.GetByteCount(str) == str.Length * 2;
        }

        #endregion

        #region 验证URL
        /// <summary>
        /// 验证URL是否正确
        /// </summary>
        /// <param name="str">URL地址</param>
        /// <returns>返回bool类型</returns>
        public static bool IsUrl(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$";
            return Regex.IsMatch(str, regex);
        }

        /// <summary>
        /// 验证Uri是否正确
        /// </summary>
        /// <param name="str">Uri地址</param>
        /// <returns>返回bool类型</returns>
        public static bool IsUri(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            if (str.IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1)
                return false;

            var schemes = new[]
             {
               "file",
               "ftp",
               "gopher",
               "http",
               "https",
               "ldap",
               "mailto",
               "net.pipe",
               "net.tcp",
               "news",
               "nntp",
               "telnet",
               "uuid"
             };

            bool hasValidSchema = false;
            foreach (string scheme in schemes)
            {
                if (hasValidSchema)
                {
                    continue;
                }
                if (str.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
                {
                    hasValidSchema = true;
                }
            }
            if (!hasValidSchema)
            {
                str = "http://" + str;
            }
            return Uri.IsWellFormedUriString(str, UriKind.Absolute);
        }

        #endregion

        #region 验证MAC地址、IP地址、端口号 是否有效

        /// <summary>
        /// 验证Mac地址是否正确
        /// </summary>
        /// <param name="str">Mac地址</param>
        /// <returns>返回bool类型</returns>
        public static bool IsNetMac(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^([0-9a-fA-F]{2})(([/\s:-][0-9a-fA-F]{2}){5})$";
            return Regex.IsMatch(str, regex);
        }
        /// <summary>
        /// 验证IP地址是否正确
        /// </summary>
        /// <param name="str">IP地址</param>
        /// <returns>返回bool类型</returns>
        public static bool IsNetIp(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            string regex = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
            return Regex.IsMatch(str, regex);
        }
        /// <summary>
        /// 验证端口号是否正确
        /// </summary>
        /// <param name="str">端口号</param>
        /// <returns>返回bool类型</returns>
        public static bool IsNetPort(string str)
        {
            if (IsNullOrEmpty(str))
                return false;
            if (!IsNumberPosInt(str))
                return false;
            if (Int32.Parse(str) > 65535 || Int32.Parse(str) < 0)
                return false;
            return true;
        }

        #endregion

        #region 验证是否为空
        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(object data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }
        /// <summary>
        /// 判断DataTable 有没有数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>(true没有)</returns>
        public static bool IsDataTableNotData(DataTable dt)
        {
            if (dt == null)
            {
                return true;
            }
            if (dt.Rows.Count < 1)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
