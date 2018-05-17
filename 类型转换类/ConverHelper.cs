using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
/*
&lt; < 小于
&gt; > 大于
&amp;& 和号
&apos;'省略号
&quot;"引号
 * */
namespace BZ.Common
{
    /// <summary>
    /// 类型转换帮助类
    /// <para>主要方法如下：</para>
    /// <para>01. ToTSource&lt;T>()        //转换 object为 T 类型</para>
    /// <para>03. ToBool()                 //将object转换为 bool 类型</para>
    /// <para>04. ToInt()                  //将object转换Int 类型</para>
    /// <para>05. ToFloat()                //将object转换 Float 类型</para>
    /// <para>06. ToDouble()               //将object转换 double 类型</para>
    /// <para>07. ToDecimal()              //将对象转换 decimal 类型</para>
    /// <para>08. ToDateTime()             //将object转换 DateTime 日期类型</para>
    /// <para>09. ToList&lt;TSource>()     //DataTable 转换 IList</para>
    /// <para>10. ToJson()                 //object 转换 Json</para>
    /// <para>11. JsonToTSource&lt;T>()    //解析Json字符串 生成实体对象</para>
    /// <para>12. JsonToListTSource&lt;T>()//解析Json字符串 生成实体对象集合</para>
    /// <para>13. JsonToAnonymous&lt;T>()  //解析Json字符串 生成匿名对象</para>
    /// <para>14. ToRMB()                  //将数值转换 RMB 大写金额</para>
    /// <para>15. FromToTSource&lt;T>()        //将客户端提交的值 转换 T 对象</para>
    /// </summary>
    public class ConverHelper
    {
        #region object 转换T类型数据
        /// <summary>
        /// 转换 object为 T 类型
        /// </summary>
        /// <typeparam name="T">T 类型</typeparam>
        /// <param name="obj">object 数据</param>
        /// <param name="defVal">默认值</param>
        /// <param name="exp">是否抛出异常</param>
        /// <returns></returns>
        public static T ToTSource<T>(object obj, T defVal = default(T), bool exp = false)
        {
            T result = defVal;
            if (obj == null)
            {
                return result;
            }
            if (obj is T)
            {
                return (T)obj;
            }

            try
            {
                Type conversionType = typeof(T);
                object obj2 = null;
                if (conversionType.Equals(typeof(Guid)))
                    obj2 = new Guid(Convert.ToString(obj));
                else
                    obj2 = Convert.ChangeType(obj, conversionType);
                result = (T)obj2;
            }
            catch (Exception ex)
            {
                if (exp == true)
                {
                    throw ex;
                }
            }
            return result;
        }
        #endregion

        #region 数值类型转换
        #region 转换为 bool 类型
        /// <summary>
        /// 将object转换为 bool 类型
        /// </summary>
        /// <param name="obj">待转换的object</param>
        /// <param name="defVal">缺省值(转换不成功)</param>
        /// <param name="exp">是否抛出异常(默认不抛出)</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ToBool(object obj, bool defVal = false, bool exp = false)
        {
            bool result = defVal;
            try
            {
                if (obj != null)
                    result = Convert.ToBoolean(obj);
            }
            catch (Exception ex)
            {
                if (exp == true)
                {
                    throw ex;
                }
            }
            return result;
        }

        #endregion

        #region 转换为 Int 数值类型
        /// <summary>
        /// 将object转换Int 类型
        /// </summary>
        /// <param name="str">待转换的object</param>
        /// <param name="defVal">缺省值(转换不成功)</param>
        /// <param name="exp">是否抛出异常(默认不抛出)</param>
        /// <returns>转换后的Int类型结果</returns>
        public static int ToInt(object obj, int defVal = 0, bool exp = false)
        {
            int result = defVal;
            try
            {
                if (obj != null)
                    result = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                if (exp == true)
                {
                    throw ex;
                }
            }
            return result;
        }

        #endregion

        #region 转换为 Float 数值类型
        /// <summary>
        /// 将object转换 Float 类型
        /// </summary>
        /// <param name="str">待转换的object</param>
        /// <param name="defVal">缺省值(转换不成功)</param>
        /// <param name="exp">是否抛出异常(默认不抛出)</param>
        /// <returns>转换后的Int类型结果</returns>
        public static float ToFloat(object obj, float defVal = 0, bool exp = false)
        {
            float result = defVal;
            try
            {
                if (obj != null)
                    result = Convert.ToSingle(obj);
            }
            catch (Exception ex)
            {
                if (exp == true)
                {
                    throw ex;
                }
            }
            return result;
        }

        #endregion

        #region 转换为 Double 数值类型
        /// <summary>
        /// 将object转换 double 类型
        /// </summary>
        /// <param name="str">待转换的object</param>
        /// <param name="defVal">缺省值(转换不成功)</param>
        /// <param name="exp">是否抛出异常(默认不抛出)</param>
        /// <returns>转换后的Int类型结果</returns>
        public static double ToDouble(object obj, double defVal = 0, bool exp = false)
        {
            double result = defVal;
            try
            {
                if (obj != null)
                    result = Convert.ToDouble(obj);
            }
            catch (Exception ex)
            {
                if (exp == true)
                {
                    throw ex;
                }
            }
            return result;
        }

        #endregion

        #region 转换为 decimal 数值类型
        /// <summary>
        /// 将对象转换 decimal 类型
        /// </summary>
        /// <param name="obj">待转换的字符串</param>
        /// <param name="defVal">缺省值(转换不成功)</param>
        /// <param name="exp">是否抛出异常(默认不抛出)</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ToDecimal(object obj, decimal defVal = 0m, bool exp = false)
        {
            decimal result = defVal;
            try
            {
                if (obj != null)
                    result = Convert.ToDecimal(obj);
            }
            catch (Exception ex)
            {
                if (exp == true)
                {
                    throw ex;
                }
            }
            return result;
        }
        #endregion
        #endregion

        #region 转换为 DateTime 日期类型
        /// <summary>
        /// 将object转换 DateTime 日期类型
        /// </summary>
        /// <param name="str">待转换的字符串</param>
        /// <param name="defVal">缺省值(转换不成功)</param>
        /// <param name="exp">是否抛出异常(默认不抛出)</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime ToDateTime(object obj, string defVal = "1970-01-01 08:00:00", bool exp = false)
        {
            DateTime result = DateTime.Parse(defVal);
            try
            {
                if (obj != null)
                    result = Convert.ToDateTime(obj);
            }
            catch (Exception ex)
            {
                if (exp == true)
                {
                    throw ex;
                }
            }
            return result;
        }

        #endregion

        #region DataTable 转 IList
        /// <summary>
        /// DataTable 转换 IList
        /// </summary>
        /// <typeparam name="TSource">model 类型</typeparam>
        /// <param name="dataTable">DataTable集合</param>
        /// <returns>List集合</returns>
        public static IList<TSource> ToList<TSource>(DataTable dataTable) where TSource : new()
        {
            var dataList = new List<TSource>();
            if (dataTable == null)
            {
                return null;
            }
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(TSource);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表 
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dataTable.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });

            foreach (DataRow row in dataTable.Rows)
            {
                //创建TResult的实例
                TSource ob = new TSource();

                //找到对应的数据  并赋值
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, ChangeType(row[p.Name], p.PropertyType), null); });
                //放入到返回的集合中.
                dataList.Add(ob);
            }
            return dataList;
        }

        ///     将数据转化为 type 类型
        /// </summary>
        /// <param name="value">要转化的值</param>
        /// <param name="type">目标类型</param>
        /// <returns>转化为目标类型的 Object 对象</returns>
        private static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                NullableConverter convertor = new NullableConverter(type);
                return Convert.IsDBNull(value) ? null : convertor.ConvertFrom(value);
            }
            return Convert.ChangeType(Convert.IsDBNull(value) ? null : value, type);
        }

        #endregion

        #region Json 解析操作 （引用 Newtonsoft.Json）
        /// <summary>
        /// object 转换 Json
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <returns>json字符串</returns>
        public static string ToJson(object obj)
        {
            var settings = new JsonSerializerSettings()
            {
                //解决json序列化时的循环引用问题
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,//None--不序列化，Error-抛出异
                //对JSON 数据使用混合大小写。跟属性名同样的大小.输出  
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //忽略Null值
                NullValueHandling = NullValueHandling.Include,
                //解决时间中带有T的问题,如:2018-01-12T16:00:44.3
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DateFormatString = "yyyy-MM-dd hh:mm:ss",
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                //高级用法九中的Bool类型转换 设置  
                //Converters.Add(new BoolConvert("是,否")),
                Formatting = Formatting.Indented,
                MaxDepth = 10, //设置序列化的最大层数  
            };


            string json = JsonConvert.SerializeObject(obj, settings);
            return json;
        }

        public static string ToJson(object obj, JsonSerializerSettings settings)
        {
            string json = JsonConvert.SerializeObject(obj, settings);
            return json;
        }

        /// <summary>
        /// 解析Json字符串 生成实体对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <returns>实体对象</returns>
        public static T JsonToObj<T>(string json) where T : class,new()
        {
            JsonSerializer js = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = js.Deserialize(new JsonTextReader(sr), typeof(T));
            T result = o as T;
            return result;
        }

        /// <summary>
        /// 解析Json字符串 生成实体对象集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <returns>实体对象集合</returns>
        public static IList<T> JsonToListObj<T>(string json) where T : class,new()
        {
            JsonSerializer js = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = js.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            IList<T> result = o as List<T>;
            return result;
        }
        /// <summary>
        /// 解析Json字符串 生成匿名对象
        /// </summary>
        /// <typeparam name="T">匿名对象</typeparam>
        /// <param name="json">Json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T JsonToAnonymous<T>(string json, T anonymousTypeObject)
        {
            T result = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return result;
        }
        #endregion

        #region 杂项转换
        #region 转换为 大写金额
        /// <summary>
        /// 将数值转换 RMB 大写金额
        /// </summary>
        /// <param name="money">待转换数值</param>
        /// <returns>大写金额</returns>
        public static string ToRMB(decimal money)
        {
            var s = money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }

        #endregion

        #region 将客户端提交的值 转换 T 对象
        /// <summary>
        /// 将客户端提交的值 转换T对象
        /// </summary>
        /// <typeparam name="T">T 类型</typeparam>
        /// <param name="frmDats">Request.Form</param>
        /// <returns></returns>
        /// <example>
        /// <!--UserModel model = ConverHelper.To<UserModel>(context.Request.Form); -->
        /// </example>
        public static T FromToTSource<T>(NameValueCollection frmDats) where T : class ,new()
        {
            Type type = typeof(T);
            string[] strArray = type.FullName.Split(new char[] { '.' });
            string cls = strArray[strArray.Length - 1];
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            T local = Activator.CreateInstance<T>();
            foreach (string data in frmDats.AllKeys)
            {
                string key = frmDats[data];
                if (!string.IsNullOrEmpty(key))
                    key = key.TrimEnd(new char[0]);
                foreach (PropertyInfo info in properties)
                {
                    string cls_name = string.Format("", cls, info.Name);
                    if (data.Equals(info.Name, StringComparison.CurrentCultureIgnoreCase)
                        || data.Equals(cls_name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        string typeName = info.PropertyType.ToString();
                        if (info.PropertyType.IsGenericType)
                        {
                            typeName = info.PropertyType.GetGenericArguments()[0].ToString();
                        }
                        object nullInternal = GetNullInternal(info.PropertyType);
                        Type conversionType = Type.GetType(typeName, false);
                        if (!string.IsNullOrEmpty(key))
                        {
                            nullInternal = Convert.ChangeType(key, conversionType);
                        }
                        info.SetValue(local, nullInternal, null);
                    }
                }
            }
            return local;
        }

        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object GetNullInternal(Type type)
        {
            if (type.IsValueType)
            {
                if (type.IsEnum)
                {
                    return GetNullInternal(Enum.GetUnderlyingType(type));
                }
                if (type.IsPrimitive)
                {
                    if (type == typeof(int))
                    {
                        return 0;
                    }
                    if (type == typeof(double))
                    {
                        return 0.0;
                    }
                    if (type == typeof(short))
                    {
                        return (short)0;
                    }
                    if (type == typeof(sbyte))
                    {
                        return (sbyte)0;
                    }
                    if (type == typeof(long))
                    {
                        return 0L;
                    }
                    if (type == typeof(byte))
                    {
                        return (byte)0;
                    }
                    if (type == typeof(ushort))
                    {
                        return (ushort)0;
                    }
                    if (type == typeof(uint))
                    {
                        return 0;
                    }
                    if (type == typeof(ulong))
                    {
                        return (ulong)0L;
                    }
                    if (type == typeof(ulong))
                    {
                        return (ulong)0L;
                    }
                    if (type == typeof(float))
                    {
                        return 0f;
                    }
                    if (type == typeof(bool))
                    {
                        return false;
                    }
                    if (type == typeof(char))
                    {
                        return '\0';
                    }
                }
                else
                {
                    if (type == typeof(DateTime))
                    {
                        return DateTime.MinValue;
                    }
                    if (type == typeof(decimal))
                    {
                        return 0M;
                    }
                    if (type == typeof(Guid))
                    {
                        return Guid.Empty;
                    }
                    if (type == typeof(TimeSpan))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                }
            }
            return null;
        }
        #endregion
        #endregion

        #region 转换字符串
        /// <summary>
        /// 转换字符串，因为可能有空，直接转换会报错
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static string ToString(object obj, string defVal = "")
        {
            string result = defVal;
            if (obj == null)
            {
                return result;
            }
            return obj.ToString();
        }
        #endregion
    }
}
