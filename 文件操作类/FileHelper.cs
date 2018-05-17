using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace BZ.Common
{
    /// <summary>
    /// 文件夹|文件操作类
    /// <para>主要方法如下：</para>
    /// <para>1.01 IsExistDir(string dirPath)    //检测文件夹是否存在</para>
    /// <para>1.02 GetDirSubDirs(string dirPath, string searchPattern = '*', bool searchOption = false)  //获取指定文件夹中所有子文件夹列表</para>
    /// <para>1.03 CreateDir(string dirPath) //创建文件夹</para>
    /// <para>1.04 DeleteDir(string dirPath) //删除指定文件夹</para>
    /// <para>1.05 ClearDir(string dirPath)  //清空指定文件夹</para>
    /// <para>2.01 IsExistFile(string filePath)  //检测指定文件是否存在</para>
    /// <para>2.02 GetDirFilesAttr(string dirPath, string searchPattern = '*', bool searchOption = false)    //获取指定文件夹中的文件列表</para>
    /// <para>2.03 CreateFile(string filePath)   //创建文件</para>
    /// <para>2.04 DeleteFile(string filePath)   //删除指定文件</para>
    /// <para></para>
    /// </summary>
    public class FileHelper
    {


        #region  文件夹

        #region 检测指定文件夹是否存在
        /// <summary>
        /// 检测文件夹是否存在
        /// </summary>
        /// <param name="dirPath">目录路径(绝对路径)</param>
        /// <returns>存在true</returns>
        public static bool IsExistDir(string dirPath)
        {
            return Directory.Exists(dirPath);
        }
        #endregion

        #region 获取指定文件夹中所有子文件夹列表

        /// <summary>
        /// 获取指定文件夹中所有子文件夹列表 
        /// </summary>
        /// <param name="dirPath">目录路径(绝对路径)</param>
        /// <param name="searchPattern">
        /// 搜索模式('*' 代码0或N个字符，'?' 代表1个字符)
        /// <para>列如："Log*.log" 表示搜索所有Log开头的log文件</para>
        /// </param>
        /// <param name="SearchOption">是否搜索子目录</param>
        /// <returns>文件夹列表</returns>
        public static List<FileAttrModel> GetDirSubDirs(string dirPath, string searchPattern = "*", bool searchOption = false)
        {
            if (!IsExistDir(dirPath))
            {
                throw new FileNotFoundException();
            }
            List<FileAttrModel> list = new List<FileAttrModel>();
            DirectoryInfo folder = new DirectoryInfo(dirPath);
            DirectoryInfo[] dirs;
            if (string.IsNullOrEmpty(searchPattern))
            {
                searchPattern = "*";
            }
            if (searchOption)
                dirs = folder.GetDirectories(searchPattern, SearchOption.AllDirectories);
            else
                dirs = folder.GetDirectories(searchPattern, SearchOption.TopDirectoryOnly);
            try
            {
                foreach (DirectoryInfo dir in dirs)
                {
                    FileAttrModel model = new FileAttrModel();
                    model.FileNmae = dir.Name;
                    model.FileFullName = dir.FullName;
                    model.Extension = dir.Extension;
                    model.CreateTime = dir.CreationTime;
                    model.LastAccessTime = dir.LastAccessTime;
                    model.LastWriteTime = dir.LastWriteTime;
                    model.IsReadOnly = (dir.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                    model.IsHidden = (dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                    list.Add(model);
                }
                return list;
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 创建文件夹
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirPath">目录的绝对路径</param>
        /// <returns>创建成功|已经存在 true</returns>
        public static bool CreateDir(string dirPath)
        {
            if (string.IsNullOrEmpty(dirPath))
                return false;
            try
            {
                if (!IsExistDir(dirPath))
                    Directory.CreateDirectory(dirPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;

        }
        #endregion

        #region 删除文件夹
        /// <summary>
        /// 删除指定文件夹
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns></returns>
        public static bool DeleteDir(string dirPath)
        {
            try
            {
                Directory.Delete(dirPath, true);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 清空指定文件夹
        /// <summary>
        /// 清空指定文件夹
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        public static void ClearDir(string dirPath)
        {
            if (IsExistDir(dirPath))
            {
                DeleteDir(dirPath);
                CreateDir(dirPath);
            }
        }

        #endregion

        #endregion

        #region 文件

        #region 检测文件是否存在
        /// <summary>
        /// 检测指定文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径(绝对路径)</param>
        /// <returns>存在true</returns>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region 获取指定文件夹中的文件列表 
        /// <summary>
        /// 获取指定文件夹中的文件列表 
        /// </summary>
        /// <param name="dirPath">目录路径(绝对路径)</param>
        /// <param name="searchPattern">
        /// 搜索模式('*' 代码0或N个字符，'?' 代表1个字符)
        /// <para>列如："Log*.log" 表示搜索所有Log开头的log文件</para>
        /// </param>
        /// <param name="SearchOption">是否搜索子目录</param>
        /// <returns>文件列表</returns>
        public static List<FileAttrModel> GetDirFilesAttr(string dirPath, string searchPattern = "*", bool searchOption = false)
        {
            if (!IsExistDir(dirPath))
            {
                throw new FileNotFoundException();
            }
            List<FileAttrModel> list = new List<FileAttrModel>();
            DirectoryInfo folder = new DirectoryInfo(dirPath);
            FileInfo[] files;
            if (string.IsNullOrEmpty(searchPattern))
            {
                searchPattern = "*";
            }
            if (searchOption)
                files = folder.GetFiles(searchPattern, SearchOption.AllDirectories);
            else
                files = folder.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);

            foreach (FileInfo file in files)
            {
                FileAttrModel model = new FileAttrModel();
                model.FileNmae = file.Name;
                model.FileFullName = file.FullName;
                model.Extension = file.Extension;
                model.FileSize = file.Length;
                model.CreateTime = file.CreationTime;
                model.LastAccessTime = file.LastAccessTime;
                model.LastWriteTime = file.LastWriteTime;
                model.IsReadOnly = file.IsReadOnly;
                model.IsHidden = (file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                //model.ParentPath = file.DirectoryName;
                list.Add(model);
            }
            return list;
        }
        #endregion

        #region 创建文件
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath">文件路径(绝对路径)</param>
        /// <returns></returns>
        public static bool CreateFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;
            try
            {
                if(!IsExistFile(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    FileStream fs = file.Create();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        #endregion

        #region 删除文件
        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 读取文件 转换byte[] 数组
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] ReadFile(string fileName)
        {
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                return pReadByte;
            }
            catch
            {
                return pReadByte;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }

        /// <summary>
        /// 获取文件的绝对路径,针对window程序和web程序都可使用
        /// </summary>
        /// <param name="relativePath">相对路径地址</param>
        /// <returns>绝对路径地址</returns>
        public static string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("参数relativePath空异常！");
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }
            //判断是Web程序还是window程序
            if (HttpContext.Current != null)
            {
                return Path.Combine(HttpRuntime.AppDomainAppPath, relativePath);
            }
            else
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            }
        }
        #endregion
    }

    #region 文件实体属性
    [Serializable]
    public class FileAttrModel
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileNmae { get; set; }
        /// <summary>
        /// 文件全名(包含路径)
        /// </summary>
        public string FileFullName { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 上次访问时间
        /// </summary>
        public DateTime LastAccessTime { get; set; }
        /// <summary>
        /// 上次修改/添加时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsHidden { get; set; }
        /// <summary>
        /// 父级路径
        /// </summary>
        //public string ParentPath { get; set; }
        /// <summary>
        /// 根路径
        /// </summary>
        //public string Root { get; set; }

    }
    #endregion
}
