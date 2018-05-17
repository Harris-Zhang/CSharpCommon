using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
//using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace BZ.Common
{
    public class Exporter
    {

        private DataTable _data;
        public Exporter Data(DataTable data)
        {
            _data = data;
            return this;
        }
        private List<Column> _colName;
        /// <summary>
        /// 列名
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public Exporter ColName(List<Column> colName)
        {
            _colName = colName;
            return this;
        }

        private string _titleName;
        /// <summary>
        /// 标题
        /// </summary>
        /// <param name="titleName"></param>
        /// <returns></returns>
        public Exporter TitleName(string titleName)
        {
            _titleName = titleName;
            return this;
        }

        private HttpResponse _response;
        public Exporter Response(HttpResponse res)
        {
            _response = res;
            return this;
        }
        private string _fileName = "ReportData.xls";
        public Exporter FileName(string filename)
        {
            _fileName = filename;
            return this;
        }
        private string _suffix = string.Empty;

        public static Exporter Instance(DataTable dt, HttpResponse resp)
        {
            var export = new Exporter();
            var context = HttpContext.Current;
            List<Column> list = new List<Column>();
            if (context.Request.Form["titles"] != null)
                //export.ColName((List<Column>)JsonToObject(context.Request.Form["titles"].ToString(), list));
                export.ColName(ConverHelper.JsonToListObj<Column>(context.Request.Form["titles"].ToString()).ToList());
            if (context.Request.Form["fileName"] != null)
                export.FileName(context.Request.Form["fileName"].ToString());
            if (context.Request.Form["titleName"] != null)
                export.TitleName(context.Request.Form["titleName"].ToString());
            export.Data(dt);
            export.Response(resp);

            return export;
        }
        //public static object JsonToObject(string jsonString, object obj)
        //{
        //    IList<Column> list = ConverHelper.JsonToListObj<Column>(jsonString);
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        //    MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        //    return serializer.ReadObject(mStream);
        //}

        public void Download()
        {
            List<Column> list_tmp = new List<Column>();
            int n = -1;
            //判断EasyUI DataGrid 需要导出的列
            for (int i = 0; i < _colName.Count; i++)
            {
                n++;
                var item = _colName[i];
                if (item.hidden || _colName[i].title == null)
                {
                    n--;
                    continue;
                }
                list_tmp.Add(item);
            }
            List<string> pos = new List<string>();
            //更改DataTable列名
            for (int i = 0; i < list_tmp.Count; i++)
            {
                for (int j = 0; j < _data.Columns.Count; j++)
                {
                    if (list_tmp[i].field == _data.Columns[j].ColumnName)
                    {//有field和DataTable名字相同的则修改列名，使之和DataGrid的Title一样
                        _data.Columns[j].ColumnName = list_tmp[i].title;
                        pos.Add(_data.Columns[j].ColumnName);
                        break;
                    }
                }
            }
            List<string> noPos = new List<string>();
            //查找 无用的列
            for (int i = 0; i < _data.Columns.Count; i++)
            {
                bool flag = false;
                for (int j = 0; j < pos.Count; j++)
                {
                    if (pos[j] == _data.Columns[i].ColumnName)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == false)
                {
                    noPos.Add(_data.Columns[i].ColumnName);
                }
            }
            //删除 无用的列
            for (int i = 0; i < noPos.Count; i++)
            {
                _data.Columns.Remove(noPos[i]);
            }
            System.IO.MemoryStream ms = NPOIHelper.ExportDT(_data, this._titleName);
            _response.Clear();
            _response.AddHeader("content-disposition", "attachment; filename=" + this._fileName);
            _response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            _response.BinaryWrite(ms.ToArray());
            ms.Close();
            ms.Dispose();
            _response.Flush();
            _response.End();

        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <Author>刘小贵</Author>
        /// <CreateDate>2017.08.18</CreateDate>
        public void DownloadExcel()
        {
            System.IO.MemoryStream ms = NPOIHelper.ExportDT(_data, this._titleName);
            _response.Clear();
            _response.AddHeader("content-disposition", "attachment; filename=" + this._fileName);
            _response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            _response.BinaryWrite(ms.ToArray());
            ms.Close();
            ms.Dispose();
            _response.Flush();
            _response.End();

        }
    }

    public class Column
    {
        public Column()
        {
        }
        public string field { get; set; }
        public string title { get; set; }
        public bool hidden { get; set; }
    }
}
