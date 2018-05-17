using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.POIFS;
using NPOI.Util;
using NPOI.HSSF.Util;
using NPOI.HSSF.Extractor;
using NPOI.SS.Formula.Eval;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;

namespace BZ.Common
{
    public static class ExcelHelper
    {
        public static HSSFWorkbook workbook;

        /// <summary>
        /// 初始化Excel文件
        /// </summary>
        public static void InitializeWorkbook()
        {
            if (workbook == null)
            {
                workbook = new HSSFWorkbook();
            }

            //定义文件的属性信息
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "鼎泰";
            workbook.DocumentSummaryInformation = dsi;

            //定义文件的摘要信息
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "鼎泰公司文档";
            si.Title = "鼎泰公司文档";
            si.Author = "鼎泰";
            si.Comments = "鼎泰MES生成文档";
            si.ApplicationName = "MES";
            si.CreateDateTime = DateTime.Now;
            workbook.SummaryInformation = si;
        }

        /// <summary>
        /// 创建Sheet
        /// </summary>
        /// <param name="workbook">文档</param>
        /// <param name="sheetName">表名</param>
        public static void SetCreateSheet(string sheetName)
        {
            if (ValidateHelper.IsNullOrEmpty(sheetName))
            {
                workbook.CreateSheet();
            }
            else {
                workbook.CreateSheet(sheetName);
            }
        }

        public static void SetSumRow(string sheetName, string strSumText, int row, int col, int endrow, int endcol, int rowHeight, string value, bool isCreate)
        {
            ISheet sheet = workbook.GetSheet(sheetName);
            IRow sRow;
            if (isCreate)
            {
                sRow = sheet.CreateRow(row);
            }
            else
            {
                sRow = sheet.GetRow(row);
            }

            sRow.HeightInPoints = rowHeight;
            sRow.CreateCell(col).SetCellValue(strSumText);

            ICellStyle sumStyle = workbook.CreateCellStyle();
            sumStyle.Alignment = HorizontalAlignment.Right;

            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = ((short)10);
            font.Underline = FontUnderlineType.Single;
            font.Boldweight = 700;
            font.FontName = "宋体";
            sumStyle.SetFont(font);
            sumStyle.FillBackgroundColor = HSSFColor.Blue.Index;
            sumStyle.FillPattern = FillPattern.SolidForeground;
            sumStyle.FillForegroundColor = HSSFColor.Yellow.Index;
            sumStyle.BorderBottom = BorderStyle.Thin;
            sumStyle.BorderTop = BorderStyle.Thin;
            sumStyle.BorderLeft = BorderStyle.Thin;
            sumStyle.BorderRight = BorderStyle.Thin;

            sRow.GetCell(col).CellStyle = sumStyle;

            if (!ValidateHelper.IsNullOrEmpty(value))
            {
                ICell valCell = sRow.CreateCell(endcol + 1);
                //应用单元格样式
                valCell.CellStyle = sumStyle;
                //设置单元格标题
                valCell.SetCellValue(value);
            }
            //合并单元格
            CellRangeAddress region = new CellRangeAddress(row, endrow, col, endcol);
            sheet.AddMergedRegion(region);

            //给每个合并的单元格赋值样式 
            for (int i = region.FirstRow; i <= region.LastRow; i++)
            {
                IRow row_reg = HSSFCellUtil.GetRow(i, (HSSFSheet)sheet);
                for (int j = region.FirstColumn; j <= region.LastColumn; j++)
                {
                    ICell singleCell = HSSFCellUtil.GetCell(row_reg, (short)j);
                    singleCell.CellStyle = sumStyle;
                }
            }
        }

        /// <summary>
        /// 设置表头信息及样式
        /// </summary>
        /// <param name="sheetName">表明</param>
        /// <param name="strHeaderText">表头内容</param>
        /// <param name="row">起始行号</param>
        /// <param name="col">起始列号</param>
        /// <param name="endrow">终止行号</param>
        /// <param name="endcol">终止列号</param>
        public static void SetHeaderRow(string sheetName, string strHeaderText, int row, int col, int endrow, int endcol)
        {
            ISheet sheet = workbook.GetSheet(sheetName);
            IRow headerRow = sheet.CreateRow(row);
            headerRow.HeightInPoints = 25;
            headerRow.CreateCell(col).SetCellValue(strHeaderText);

            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;

            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = ((short)20);
            font.Boldweight = 700;
            font.FontName = "宋体";
            headStyle.SetFont(font);
            headStyle.FillBackgroundColor = HSSFColor.Blue.Index;
            headStyle.FillPattern = FillPattern.SolidForeground;
            headStyle.FillForegroundColor = HSSFColor.Yellow.Index;
            headStyle.BorderBottom = BorderStyle.Thin;
            headStyle.BorderTop = BorderStyle.Thin;
            headStyle.BorderLeft = BorderStyle.Thin;
            headStyle.BorderRight = BorderStyle.Thin;

            headerRow.GetCell(col).CellStyle = headStyle;


            //单元格合并 - 起始行号，终止行号， 起始列号，终止列号
            //起始行号 不可能大于 终止行号
            //起始列号 不可能大于 终止列号
            CellRangeAddress region = new CellRangeAddress(row, endrow, col, endcol);
            sheet.AddMergedRegion(region);
            //设置宽度
            //sheet.SetColumnWidth(row, strHeaderText.Length * 256 * 2);

            //给每个合并的单元格赋值样式 
            for (int i = region.FirstRow; i <= region.LastRow; i++)
            {
                IRow row_reg = HSSFCellUtil.GetRow(i, (HSSFSheet)sheet);
                for (int j = region.FirstColumn; j <= region.LastColumn; j++)
                {
                    ICell singleCell = HSSFCellUtil.GetCell(row_reg, (short)j);
                    singleCell.CellStyle = headStyle;
                    sheet.SetColumnWidth(i, ((strHeaderText.Length * 256) / endcol + 1) + 5 * 256);
                }
            }
        }

        /// <summary>
        /// 设置列头信息及样式
        /// </summary>
        /// <param name="sheetName">表名</param>
        /// <param name="colNames">列集合</param>
        /// <param name="row">行号</param>
        public static void SetLineRow(string sheetName, string[] colNames, int row)
        {
            ISheet sheet = workbook.GetSheet(sheetName);
            IRow headerRow = sheet.CreateRow(row);

            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = ((short)10);
            font.Boldweight = 700;
            font.FontName = "宋体";
            headStyle.SetFont(font);
            //headStyle.FillBackgroundColor = HSSFColor.Blue.Index;
            headStyle.FillPattern = FillPattern.SolidForeground;
            headStyle.FillForegroundColor = HSSFColor.Orange.Index;
            headStyle.BorderBottom = BorderStyle.Thin;
            headStyle.BorderTop = BorderStyle.Thin;
            headStyle.BorderLeft = BorderStyle.Thin;
            headStyle.BorderRight = BorderStyle.Thin;

            for (int i = 0; i < colNames.Length; i++)
            {
                headerRow.CreateCell(i).SetCellValue(colNames[i]);
                headerRow.GetCell(i).CellStyle = headStyle;

                //设置列宽  
                //setColumnWidth（colindex, width）：设置第（colindex+1）列宽度为width个字符
                //*@param colindex - 要设置的列（从0开始）
                //*@param width - 以字符宽度的1 / 256为单位的宽度
                //sheet.SetColumnWidth(i, (colNames[i].Length + 1) * 256);

                int colWidth = sheet.GetColumnWidth(i) / 256; //获取当前列宽度
                int length = Encoding.UTF8.GetBytes(colNames[i].ToString()).Length; //获取当前单元格的内容宽度

                //若当前单元格内容宽度大于列宽，则调整列宽为当前单元格宽度，后面的+1是我人为的将宽度增加一个字符 
                if (colWidth < length + 1)
                {
                    colWidth = length + 1;
                }
                sheet.SetColumnWidth(i, colWidth * 256);
            }
        }

        /// <summary>
        /// 设置单元格内容
        /// </summary>
        public static void SetCellTitleStyle(string sheetName, string colName, int row, int col, int rowHeight, string value,bool isCreate)
        {
            ISheet sheet = workbook.GetSheet(sheetName);
            IRow dataRow;
            if (isCreate)
            {
                dataRow = sheet.CreateRow(row);
            }
            else
            {
                dataRow = sheet.GetRow(row);
            }

            ICell colCell = dataRow.CreateCell(col);
            //应用单元格样式
            ICellStyle titleStyle = NPOIHelper.SetCellStyle(workbook, BorderStyle.Thin, HSSFColor.Black.Index, VerticalAlignment.Center, HorizontalAlignment.Center, HSSFColor.Orange.Index);
            IFont titleFont = NPOIHelper.SetFont(workbook, "宋体", 0, 700, 10);
            titleStyle.SetFont(titleFont);
            colCell.CellStyle = titleStyle;
            //设置单元格标题
            colCell.SetCellValue(colName);

            if (!ValidateHelper.IsNullOrEmpty(value))
            {
                ICell valCell = dataRow.CreateCell(col + 1);
                //应用单元格样式
                valCell.CellStyle = GetCellStyle();;
                //设置单元格标题
                valCell.SetCellValue(value);
            }

            dataRow.HeightInPoints = rowHeight;
        }

        /// <summary>
        /// 写入表数据
        /// </summary>
        public static int SetCellListValue(DataTable dt, string sheetName, int row)
        {
            int rowIndex = row;
            int i = 1;
            ISheet sheet = workbook.GetSheet(sheetName);
            var dateStyle = workbook.CreateCellStyle();
            var format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            foreach (DataRow r in dt.Rows)
            {
                var dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dt.Columns)
                {
                    var newCell = dataRow.CreateCell(column.Ordinal);

                    var drValue = r[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            double result;
                            if (IsNumeric(drValue, out result))
                            {

                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            bool boolV;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                    
                    if(dt.Rows.Count - i == 0)
                    {
                        ICellStyle sStyle = NPOIHelper.SetCellStyle(workbook, BorderStyle.Thin, HSSFColor.Black.Index, VerticalAlignment.Center, HorizontalAlignment.Center, HSSFColor.SkyBlue.Index);
                        newCell.CellStyle = GetModificationCellStyle(sStyle);
                    }
                    else
                    {
                        newCell.CellStyle = GetCellStyle();
                    }

                    //单元格宽度自适应属性
                    int colWidth = sheet.GetColumnWidth(column.Ordinal) / 256; //获取当前列宽度
                    int length = Encoding.UTF8.GetBytes(r[column].ToString()).Length; //获取当前单元格的内容宽度

                    //若当前单元格内容宽度大于列宽，则调整列宽为当前单元格宽度，后面的+1是我人为的将宽度增加一个字符 
                    if (colWidth < length + 1)
                    {
                        colWidth = length + 1;
                    }

                    sheet.SetColumnWidth(column.Ordinal, colWidth * 256);
                }

                //行高自适应
                ICell currCell = dataRow.GetCell(1);
                int len = Encoding.UTF8.GetBytes(currCell.ToString()).Length;
                dataRow.HeightInPoints = 20 * (len / 60 + 1);

                rowIndex++;
                i++;
            }

            return rowIndex;
        }

        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <returns></returns>
        public static ICellStyle GetCellStyle()
        {
            //创建样式是基于HSSFWorkbook，而不是ISheet
            ICellStyle style = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.FontName = "宋体";
            //font.Underline = FontUnderlineType.Single;
            font.Color = HSSFColor.Black.Index;

            style.SetFont(font);
            //设置单元格上下左右边框线
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            //文字水平和垂直对齐方式 
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            //是否换行 
            style.WrapText = true;
            //缩小字体填充
            //style.ShrinkToFit = true;
            return style;
        }

        /// <summary>
        /// 修改样式
        /// </summary>
        /// <param name="cellStyle"></param>
        /// <returns></returns>
        public static ICellStyle GetModificationCellStyle(ICellStyle cellStyle)
        {
            //ICellStyle的创建是有数量限制的,
            //换句话说，一个Excel文件里负责保存样式的空间是有限的，样式也占用一定的文件大小。
            //那么我们就应该注意样式的重复利用，相同的样式尽量不要创建两份，直接赋值同一个就可以了.

            ICellStyle newCellStyle = workbook.CreateCellStyle();
            newCellStyle.CloneStyleFrom(cellStyle);

            //修改克隆后需要变更的样式信息
            //添加字体样式
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.FontName = "宋体";
            font.Underline = FontUnderlineType.Single;
            font.Color = HSSFColor.Black.Index;

            newCellStyle.SetFont(font);

            return newCellStyle;
        }

        public static MemoryStream ApplyFileToMs()
        {
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            //sheet = null;
            workbook = null;
            //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet  

            return ms;
        }

        public static void ExportByWeb(string strFileName)
        {
            HttpContext curContext = HttpContext.Current;
            // 设置编码和附件格式
            curContext.Response.Clear();
            curContext.Response.AppendHeader("Content-Disposition",
                            "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8).ToString());
            curContext.Response.ContentType = "application/ms-excel";
            //curContext.Response.ContentType = "application/vnd.ms-excel";
            //curContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "utf-8";

            //调用导出具体方法Export()
            var buff = ApplyFileToMs().GetBuffer();
            curContext.Response.BinaryWrite(buff);
            curContext.Response.Flush();
            curContext.Response.End();
        }

        public static bool IsNumeric(String message, out double result)
        {
            var rex = new Regex(@"^[-]?\d+[.]?\d*$");
            result = -1;
            if (rex.IsMatch(message))
            {
                result = double.Parse(message);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取Sheet个数
        /// </summary>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static int GetSheetCount(string outputFile)
        {
            var count = 0;
            try
            {
                var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);
                var workbook = WorkbookFactory.Create(readfile);
                count = workbook.NumberOfSheets;

            }
            catch (Exception exception)
            {
                //wl.WriteLogs(exception.ToString());
            }
            return count;
        }
        /// <summary>
        /// 获取Sheet名称列表
        /// </summary>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static IList<string> GetSheetNames(string outputFile)
        {
            IList<string> sheetNameList = new List<string>();
            try
            {
                var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);
                var workbook = WorkbookFactory.Create(readfile);
                for (var i = 0; i < workbook.NumberOfSheets; i++)
                    sheetNameList.Add(workbook.GetSheetName(i));
            }
            catch (Exception exception)
            {
                //wl.WriteLogs(exception.ToString());
            }
            return sheetNameList;
        }

        /// <summary>
        /// 根据表对象获得列名
        /// </summary>
        /// <param name="dt">表对象</param>
        /// <returns></returns>
        public static string[] GetColumnsByDataTable(DataTable dt)
        {
            string[] strColumns = null;

            if (dt.Rows.Count > 0)
            {
                if (dt.Columns.Count > 0)
                {
                    int columnNum = 0;
                    columnNum = dt.Columns.Count;
                    strColumns = new string[columnNum];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        strColumns[i] = dt.Columns[i].ColumnName;
                    }
                }
            }

            return strColumns;
        }
    }
}
