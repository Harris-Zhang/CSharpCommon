using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BZ.Common
{
    /// <summary>
    /// 二维码生成 采用QrCodeNet
    /// <para>主要方法如下：</para>
    /// <para>01. CreateQr()    //创建二维码</para>
    /// <para>02. SaveQr()  //创建二维码并保存到指定文件中</para>
    /// </summary>
    public class QrCodeHelper
    {
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="text">待编码字符串</param>
        /// <param name="size">二维码大小</param>
        /// <param name="ms">输出流</param>
        /// <returns>返回是否成功</returns>
        public static bool Create(string text, int size, MemoryStream ms)
        {

            // 设置二维码排错率，可选L(7%)、M(15%)、Q(25%)、H(30%)，排错率越高可存储的信息越少，但对二维码清晰度的要求越小 
            ErrorCorrectionLevel el = ErrorCorrectionLevel.H;
            //空白区域 有zreo 也就是0 没有边框 此处还要乘以2才得到空白区域的宽度。
            QuietZoneModules qzm = QuietZoneModules.Two;
            //二维码大小
            FixedModuleSize fms = new FixedModuleSize(size, qzm);
            QrEncoder qrEncoder = new QrEncoder(el);
            QrCode qrCode = null;

            if (qrEncoder.TryEncode(text, out qrCode))//对内容进行编码，并保存生成的矩阵    
            {
                GraphicsRenderer render = new GraphicsRenderer(fms, Brushes.Black, Brushes.White);
                //.WriteToStream(matrix, ImageFormat.Png, stream, new Point(600, 600)); 是跟打印有关的DPI分辨率的参数，默认即可，调整对调整图片大小没有作用
                render.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建二维码并保存到指定文件中
        /// </summary>
        /// <param name="text">待编码字符串</param>
        /// <param name="size">二维码大小</param>
        /// <param name="fileName">输出路径(D:\123.png)</param>
        /// <returns>返回是否成功</returns>
        public static bool Save(string text, int size, string fileName)
        {
            // 设置二维码排错率，可选L(7%)、M(15%)、Q(25%)、H(30%)，排错率越高可存储的信息越少，但对二维码清晰度的要求越小 
            ErrorCorrectionLevel el = ErrorCorrectionLevel.H;
            //空白区域 有zreo 也就是0 没有边框 此处还要乘以2才得到空白区域的宽度。
            QuietZoneModules qzm = QuietZoneModules.Two;
            //二维码大小
            FixedModuleSize fms = new FixedModuleSize(size, qzm);
            QrEncoder qrEncoder = new QrEncoder(el);
            QrCode qrCode = null;
            try
            {
                if (qrEncoder.TryEncode(text, out qrCode))//对内容进行编码，并保存生成的矩阵    
                {
                    GraphicsRenderer render = new GraphicsRenderer(fms, Brushes.Black, Brushes.White);
                    //.WriteToStream(matrix, ImageFormat.Png, stream, new Point(600, 600)); 是跟打印有关的DPI分辨率的参数，默认即可，调整对调整图片大小没有作用
                    using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    {
                        render.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
