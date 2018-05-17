using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BZ.Common
{
    public class WebServiceHelper : WebService
    {
        #region 私有变量和属性

        /// <summary>                   
        /// web服务地址                           
        /// </summary>                              
        private string _wsdlUrl = string.Empty;
        /// <summary>                   
        /// web服务名称                           
        /// </summary>                              
        private string _wsdlName = string.Empty;
        /// <summary>                   
        /// 代理类命名空间                           
        /// </summary>                              
        private string _wsdlNamespace = "FrameWork.WebService.DynamicWebServiceCalling.{0}";
        /// <summary>                   
        /// 代理类类型名称                           
        /// </summary>                              
        private Type _typeName = null;
        /// <summary>                   
        /// 程序集名称                             
        /// </summary>                              
        private string _assName = string.Empty;
        /// <summary>                   
        /// 代理类所在程序集路径                            
        /// </summary>                              
        private string _assPath = string.Empty;
        /// <summary>                   
        /// 代理类的实例                            
        /// </summary>                              
        private object _instance = null;
        /// <summary>                   
        /// 代理类的实例                            
        /// </summary>                              
        private object Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Activator.CreateInstance(_typeName);
                    return _instance;
                }
                else
                    return _instance;
            }
        }

        #endregion

        #region 构造函数
        public WebServiceHelper(string wsdlUrl)
        {

            this._wsdlUrl = wsdlUrl;
            string wsdlName = WebServiceHelper.getWsclassName(wsdlUrl);
            this._wsdlName = wsdlName;
            this._assName = string.Format(_wsdlNamespace, wsdlName);
            this._assPath = Path.GetTempPath() + this._assName + getMd5Sum(this._wsdlUrl) + ".dll";
            this.CreateServiceAssembly();
        }

        public WebServiceHelper(string wsdlUrl, string wsdlName)
        {
            this._wsdlUrl = wsdlUrl;
            this._wsdlName = wsdlName;
            this._assName = string.Format(_wsdlNamespace, wsdlName);
            this._assPath = Path.GetTempPath() + this._assName + getMd5Sum(this._wsdlUrl) + ".dll";
            this.CreateServiceAssembly();
        }
        #endregion

        #region 得到WSDL信息，生成本地代理类并编译为DLL，构造函数调用，类生成时加载
        /// <summary>                           
        /// 得到WSDL信息，生成本地代理类并编译为DLL                           
        /// </summary>                              
        private void CreateServiceAssembly()
        {
            if (this.checkCache())
            {
                this.initTypeName();
                return;
            }
            if (string.IsNullOrEmpty(this._wsdlUrl))
            {
                return;
            }
            try
            {
                //使用WebClient下载WSDL信息                         
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(this._wsdlUrl);
                ServiceDescription description = ServiceDescription.Read(stream);//创建和格式化WSDL文档  
                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();//创建客户端代理代理类  
                importer.ProtocolName = "Soap";
                importer.Style = ServiceDescriptionImportStyle.Client;  //生成客户端代理                         
                importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
                importer.AddServiceDescription(description, null, null);//添加WSDL文档  
                //使用CodeDom编译客户端代理类                   
                CodeNamespace nmspace = new CodeNamespace(_assName);    //为代理类添加命名空间                  
                CodeCompileUnit unit = new CodeCompileUnit();
                unit.Namespaces.Add(nmspace);
                this.checkForImports(this._wsdlUrl, importer);
                ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CompilerParameters parameter = new CompilerParameters();
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");
                parameter.GenerateExecutable = false;
                parameter.GenerateInMemory = false;
                parameter.IncludeDebugInformation = false;
                CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
                provider.Dispose();
                if (result.Errors.HasErrors)
                {
                    string errors = string.Format(@"编译错误:{0}错误！", result.Errors.Count);
                    foreach (CompilerError error in result.Errors)
                    {
                        errors += error.ErrorText;
                    }
                    throw new Exception(errors);
                }
                this.copyTempAssembly(result.PathToAssembly);
                this.initTypeName();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region 执行Web服务方法
        /// <summary>                           
        /// 执行代理类指定方法，有返回值                                
        /// </summary>                                  
        /// <param   name="methodName">方法名称</param>                           
        /// <param   name="param">参数</param>                              
        /// <returns>object</returns>                                 
        public object ExecuteQuery(string methodName, object[] param)
        {
            object rtnObj = null;
            object[] obj = new object[3];

            try
            {
                if (this._typeName == null)
                {
                    //记录Web服务访问类名错误日志代码位置  
                    throw new TypeLoadException("Web服务访问类名【" + this._wsdlName + "】不正确，请检查！");
                }
                //调用方法  
                MethodInfo mi = this._typeName.GetMethod(methodName);
                if (mi == null)
                {
                    //记录Web服务方法名错误日志代码位置  
                    throw new TypeLoadException("Web服务访问方法名【" + methodName + "】不正确，请检查！");
                }
                try
                {
                    if (param == null)
                        rtnObj = mi.Invoke(Instance, null);
                    else
                    {
                        rtnObj = mi.Invoke(Instance, param);
                    }
                }
                catch (TypeLoadException tle)
                {
                    //记录Web服务方法参数个数错误日志代码位置  
                    throw new TypeLoadException("Web服务访问方法【" + methodName + "】参数个数不正确，请检查！", new TypeLoadException(tle.StackTrace));
                }
            }
            catch (Exception ex)
            {
                if (methodName.Equals("invokeSrv"))//ERP WebService 挂壁
                {
                    throw new Exception("ERP WebService 调用异常:" + ex.Message, new Exception(ex.StackTrace));
                }
                else//SRM WebSerive 挂壁
                {
                    throw new Exception("SRM WebService 调用异常:" + ex.Message, new Exception(ex.StackTrace));
                }

            }
            return rtnObj;
        }

        /// <summary>                           
        /// 执行代理类指定方法，无返回值                                
        /// </summary>                                  
        /// <param   name="methodName">方法名称</param>                           
        /// <param   name="param">参数</param>                              
        public void ExecuteNoQuery(string methodName, object[] param)
        {
            try
            {
                if (this._typeName == null)
                {
                    //记录Web服务访问类名错误日志代码位置  
                    throw new TypeLoadException("Web服务访问类名【" + this._wsdlName + "】不正确，请检查！");
                }
                //调用方法  
                MethodInfo mi = this._typeName.GetMethod(methodName);
                if (mi == null)
                {
                    //记录Web服务方法名错误日志代码位置  
                    throw new TypeLoadException("Web服务访问方法名【" + methodName + "】不正确，请检查！");
                }
                try
                {
                    if (param == null)
                        mi.Invoke(Instance, null);
                    else
                        mi.Invoke(Instance, param);
                }
                catch (TypeLoadException tle)
                {
                    //记录Web服务方法参数个数错误日志代码位置  
                    throw new TypeLoadException("Web服务访问方法【" + methodName + "】参数个数不正确，请检查！", new TypeLoadException(tle.StackTrace));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, new Exception(ex.StackTrace));
            }
        }
        #endregion

        #region 私有方法
        /// <summary>                               
        /// 得到代理类类型名称                                 
        /// </summary>                                  
        private void initTypeName()
        {
            Assembly serviceAsm = Assembly.LoadFrom(this._assPath);
            Type[] types = serviceAsm.GetTypes();
            string objTypeName = "";
            foreach (Type t in types)
            {
                if (t.BaseType == typeof(SoapHttpClientProtocol))
                {
                    objTypeName = t.Name;
                    break;
                }
            }
            _typeName = serviceAsm.GetType(this._assName + "." + objTypeName);
        }

        /// <summary>                       
        /// 根据web   service文档架构向代理类添加ServiceDescription和XmlSchema                             
        /// </summary>                                  
        /// <param   name="baseWSDLUrl">web服务地址</param>                           
        /// <param   name="importer">代理类</param>                              
        private void checkForImports(string baseWsdlUrl, ServiceDescriptionImporter importer)
        {
            DiscoveryClientProtocol dcp = new DiscoveryClientProtocol();
            dcp.DiscoverAny(baseWsdlUrl);
            dcp.ResolveAll();
            foreach (object osd in dcp.Documents.Values)
            {
                if (osd is ServiceDescription) importer.AddServiceDescription((ServiceDescription)osd, null, null); ;
                if (osd is XmlSchema) importer.Schemas.Add((XmlSchema)osd);
            }
        }

        /// <summary>                           
        /// 复制程序集到指定路径                                
        /// </summary>                                  
        /// <param   name="pathToAssembly">程序集路径</param>                              
        private void copyTempAssembly(string pathToAssembly)
        {
            File.Copy(pathToAssembly, this._assPath);
        }

        private string getMd5Sum(string str)
        {
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>                           
        /// 是否已经存在该程序集                                
        /// </summary>                                  
        /// <returns>false:不存在该程序集,true:已经存在该程序集</returns>                                
        private bool checkCache()
        {
            if (File.Exists(this._assPath))
            {
                return true;
            }
            return false;
        }

        //私有方法，默认取url入口的文件名为类名  
        private static string getWsclassName(string wsdlUrl)
        {
            string[] parts = wsdlUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
        #endregion
    }
}
