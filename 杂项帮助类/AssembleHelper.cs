using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BZ.Common
{
    public class AssembleHelper
    {
        /// <summary>
        /// 获取类的属性、方法  
        /// </summary>  
        /// <param name="assemblyName">程序集</param>  
        /// <param name="className">类名</param>  
        public static Type GetClassInfo(string assemblyName, string className)
        {
            try
            {

                string assemblyUrl = FileHelper.GetAbsolutePath(assemblyName + ".dll");
                Assembly assembly = null;
                //if (!AssemblyDict.TryGetValue(assemblyName, out assembly))
                //{
                //assembly = Assembly.LoadFrom(assemblyUrl);
                //    AssemblyDict[assemblyName] = assembly;
                //}

                //先将插件拷贝到内存缓冲
                byte[] addinStream =FileHelper.ReadFile(assemblyUrl);

                assembly = Assembly.Load(addinStream); //加载内存中的Dll

                Type type = assembly.GetType(assemblyName + "." + className, true, true);
                return type;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
