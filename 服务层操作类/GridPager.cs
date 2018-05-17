using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// 分页操作
    /// </summary>
    public class GridPager
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// 当前页是第几页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// 总行数
        /// </summary>
        public int totalRows { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPages
        {
            get
            {
                return (int)Math.Ceiling((float)totalRows / (float)rows);
            }
        }
        /// <summary>
        /// 分页开始序号
        /// </summary>
        public int startIndex
        {
            get
            {
                return rows * (page - 1) + 1;
            }
        }
        /// <summary>
        /// 分页结束序号
        /// </summary>
        public int endIndex
        {
            get
            {
                return startIndex + rows - 1;
            }
        }
        /// <summary>
        /// 排序列+排序方式
        /// </summary>
        public string sortOrder
        {
            get
            {
                return sort + " " + order;
            }
        }

        public bool isPage
        {
            get
            {
                return page==0 && rows==0 ? false : true;
            }
        }
    }
}
