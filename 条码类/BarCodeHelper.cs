using BarcodeLib;
using System;
using System.Drawing;

namespace BZ.Common
{
    /// <summary>
    /// 创建条形码类
    /// <para>主要方法如下：</para>
    /// <para>01. Create()  //创建条形码</para>
    /// </summary>
    public class BarCodeHelper
    {
        /// <summary>
        /// 创建条形码
        /// </summary>
        /// <param name="text">条形码内容</param>
        /// <returns></returns>
        public static Image Create(string text)
        {
            return Create(text, 300, 50, true);
        }
        /// <summary>
        /// 创建条形码
        /// </summary>
        /// <param name="text">条形码内容</param>
        /// <param name="width">宽度(默认300)</param>
        /// <param name="height">高度(默认50)</param>
        /// <param name="isLable">是否现在下面显示内容(默认true)</param>
        /// <returns></returns>
        public static Image Create(string text, int width, int height, bool isLable)
        {
            return Create(text, width, height, isLable, Color.White, Color.Black);
        }
        /// <summary>
        /// 创建条形码
        /// </summary>
        /// <param name="text">条形码内容</param>
        /// <param name="width">宽度(默认300)</param>
        /// <param name="height">高度(默认50)</param>
        /// <param name="isLable">是否现在下面显示内容(默认true)</param>
        /// <param name="backColor">背景色</param>
        /// <param name="foreColor">前景色</param>
        /// <returns></returns>
        public static Image Create(string text, int width, int height, bool isLable, Color backColor, Color foreColor)
        {
            Image img = null;
            try
            {
                using (var barcode = new Barcode()
                {
                    IncludeLabel = isLable,
                    Alignment = AlignmentPositions.CENTER,
                    Width = width,
                    Height = height,
                    RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                    BackColor = backColor,
                    ForeColor = foreColor
                })
                {
                    img = barcode.Encode(TYPE.CODE128B, text);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return img;
        }
    }

}
