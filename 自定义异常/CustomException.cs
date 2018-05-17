using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BZ.Common
{
    public class CustomException : ApplicationException//由用户程序引发，用于派生自定义的异常类型
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public CustomException() { }

        public CustomException(string message) : base(message) { }

        public CustomException(string message, Exception inner) : base(message, inner) { }

        public CustomException(int flag)
        {
            ResultFlag = flag;
        }

        public CustomException(int flag, string message)
            : base(message)
        {
            ResultFlag = flag;
        }

        public CustomException(int flag, string message, Exception inner)
            : base(message, inner)
        {
            ResultFlag = flag;
        }
        /// <summary>
        /// 异常标识位，用于标识错误信息
        /// </summary>
        public int ResultFlag = -1;


    }
}
