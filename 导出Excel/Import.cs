using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Data;

namespace BZ.Common.Excel
{
    public class Import
    {
        /// <summary>
        /// 读取Excel(2003),并转为DataTable
        /// </summary>
        /// <Author>Ebbo.jiang</Author>
        /// <CreateDate>2017-5-24</CreateDate> 建立日期
        public DataTable ExcelToDataTable(Stream stream)
        {
            DataTable dt = new DataTable();
            HSSFWorkbook workBook = new HSSFWorkbook(stream);
            HSSFSheet sheet = workBook.GetSheetAt(0) as HSSFSheet;
            HSSFRow headRow = sheet.GetRow(0) as HSSFRow;
            int cellCount = headRow.LastCellNum;
            for (int i = headRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headRow.GetCell(i).StringCellValue);
                dt.Columns.Add(column);
            }
            int rowCount = sheet.LastRowNum + 1;
            for (int i = (sheet.FirstRowNum + 1); i < rowCount; i++)
            {
                HSSFRow row = sheet.GetRow(i) as HSSFRow;
                DataRow dtRow = dt.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        dtRow[j] = row.GetCell(j).ToString();
                    }
                }
                dt.Rows.Add(dtRow);
            }
            workBook = null;
            sheet = null;
            return dt;
        }
    }
}
