using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; 

namespace BZ.Common
{
    public class NPOIHelper
    {


        public static MemoryStream ExportDT(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;

            #region 右击文件 属性信息

            //{
            //    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            //    dsi.Company = "http://www.yongfa365.com/";
            //    workbook.DocumentSummaryInformation = dsi;

            //    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //    si.Author = "zzh"; //填加xls文件作者信息
            //    si.ApplicationName = "NPOI测试程序"; //填加xls文件创建程序信息
            //    si.LastAuthor = "zzh"; //填加xls文件最后保存者信息
            //    si.Comments = "说明信息"; //填加xls文件作者信息
            //    si.Title = "NPOI测试"; //填加xls文件标题信息
            //    si.Subject = "NPOI测试Demo"; //填加文件主题信息
            //    si.CreateDateTime = DateTime.Now;
            //    workbook.SummaryInformation = si;
            //}

            #endregion

            HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:MM:ss");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length + 1;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length + 1;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp + 1;
                    }
                }
            }
            int rowIndex = 0;

            //表头样式
            HSSFCellStyle titleStyle = SetCellStyle(workbook, BorderStyle.Thin, 55, VerticalAlignment.Center, HorizontalAlignment.Center, HSSFColor.White.Index);
            HSSFFont titleFont = SetFont(workbook, "宋体", 0, 700, 20);
            titleStyle.SetFont(titleFont);

            //列头样式
            HSSFCellStyle headStyle = SetCellStyle(workbook, BorderStyle.Thin, 55, VerticalAlignment.Center, HorizontalAlignment.Center, HSSFColor.White.Index);
            HSSFFont headfont = SetFont(workbook, "宋体", 0, 700, 10);
            headStyle.SetFont(headfont);

            //内容样式
            HSSFCellStyle bodyStyle = SetCellStyle(workbook, BorderStyle.Thin, 55, VerticalAlignment.Center, HorizontalAlignment.Left, HSSFColor.White.Index);
            HSSFFont bodyFont = SetFont(workbook, "宋体", 0, 400, 10);
            bodyStyle.SetFont(bodyFont);

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet() as HSSFSheet;
                    }

                    #region 表头及样式

                    {
                        HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        //HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        //headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        //HSSFFont font = workbook.CreateFont() as HSSFFont;
                        //font.FontHeightInPoints = 20;
                        //font.Boldweight = 700;
                        //headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = titleStyle;

                        sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                        //headerRow.Dispose();
                    }

                    #endregion


                    #region 列头及样式

                    {
                        HSSFRow headerRow = sheet.CreateRow(1) as HSSFRow;


                        //HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        //headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        //HSSFFont font = workbook.CreateFont() as HSSFFont;
                        //font.FontHeightInPoints = 10;
                        //font.Boldweight = 700;
                        //headStyle.SetFont(font);

                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);

                        }
                        //headerRow.Dispose();
                    }

                    #endregion

                    rowIndex = 2;
                }

                #endregion

                #region 填充内容
 

                HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;

                    newCell.CellStyle = bodyStyle;

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            double result;
                            if (isNumeric(drValue, out result))
                            {

                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            else
                            {
                                newCell.SetCellValue(drValue);
                                break;
                            }

                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            if(!DateTime.TryParse(drValue, out dateV))continue;//如果数据类型装换失败，直接不赋值 modified by lu 2017.7.21
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
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

                }

                #endregion

                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }

        public static bool isNumeric(String message, out double result)
        {
            Regex rex = new Regex(@"^[-]?d+[.]?d*$");
            result = -1;
            if (rex.IsMatch(message))
            {
                result = double.Parse(message);
                return true;
            }
            else
                return false;

        }


        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dt"></param>
        /// <param name="intCon"></param>
        public static void SetColumnWidth(ISheet sheet, DataTable dt, params int[] list)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sheet.SetColumnWidth(i, list[i] * 256);
            }
        }

        public void SetColumnWidth(ISheet sheet, params int[] list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                sheet.SetColumnWidth(i, list[i] * 256);
            }
        }

        public static HSSFCellStyle SetCellStyle(HSSFWorkbook book, BorderStyle border, short color, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, short bgcolor)
        {
            HSSFCellStyle cs = (HSSFCellStyle)book.CreateCellStyle();
            cs.BorderBottom = border;
            cs.BorderLeft = border;
            cs.BorderRight = border;
            cs.BorderTop = border;
            cs.BottomBorderColor = color;
            cs.LeftBorderColor = color;
            cs.RightBorderColor = color;
            cs.TopBorderColor = color;
            cs.VerticalAlignment = verticalAlignment;
            cs.Alignment = horizontalAlignment;
            cs.FillForegroundColor = cs.FillForegroundColor = bgcolor; ;
            cs.FillPattern = FillPattern.SolidForeground;
            return cs;
        }


        /// <summary>
        /// 字体样式
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public static HSSFFont SetFont(HSSFWorkbook book, string FontName, short fontColor, short fontWeight, short fontPoint)
        {
            HSSFFont font = (HSSFFont)book.CreateFont();
            font.FontName = FontName;
            font.Color = fontColor;
            font.Boldweight = fontWeight;
            font.FontHeightInPoints = fontPoint;
            return font;
        }

        /// <summary>
        /// 冻结列
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        public static void SetFreezePane(ISheet sheet, int a, int b, int c, int d)
        {
            sheet.CreateFreezePane(a, b, c, d);
        }
    }
}
