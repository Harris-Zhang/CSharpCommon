using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// ZPL帮助类
    /// </summary>
    public class ZebraHelper
    {
        /*
         *  打印中文先引用Fnthex32.dll
            dll控件常规安装方法（仅供参考）：
            下面是32系统的注册bat文件
            可将下面的代码保存为“注册.bat“，放到dll目录，就会自动完成dll注册(win98不支持)。
            @echo 开始注册
            copy Fnthex32.dll %windir%\system32\
            regsvr32 %windir%\system32\Fnthex32.dll /s
            @echo Fnthex32.dll注册成功
            @pause

            下面是64系统的注册bat文件
            @echo 开始注册
            copy Fnthex32.dll %windir%\SysWOW64\
            regsvr32 %windir%\SysWOW64\Fnthex32.dll /s
            @echo Fnthex32.dll注册成功
            @pause
         * 
         * 
            ZebraHelper zh = new ZebraHelper();
            StringBuilder builder = new StringBuilder();            
            builder.AppendLine(zh.ZPL_Start());
            builder.AppendLine(zh.ZPL_PageSet(40, 80));
            builder.AppendLine(zh.ZPL_DrawCHText("上善若水 厚德载物", "宋体", 40, 40, 0, 32, 0, 1, 0));
            builder.AppendLine(zh.ZPL_DrawBarcode(40, 150, 3, 2, 40, "111112222233333"));
            builder.AppendLine(zh.ZPL_DrawENText("111112222233333", "A", 30, 205, "N", 30, 50));
            builder.AppendLine(zh.ZPL_DrawRectangle(20,20,2,700,700));
            builder.AppendLine(zh.ZPL_End());
            string a = builder.ToString();
            //打印
            zh.CmdDos("c:\\c.txt", a);         
         */
        public string ZPL_Start()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("^XA"); //指令块的开始
            //builder.AppendLine("^MD30"); //MD是设置色带颜色的深度
            return builder.ToString();
        }

        public string ZPL_End()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("^XZ"); //指令块的结束
            return builder.ToString();
        }

        /// <summary>
        ///  设置打印标签纸边距
        /// </summary>
        /// <param name="printX">标签纸边距x坐标</param>
        /// <param name="printY">标签纸边距y坐标</param>
        /// <returns></returns>
        public string ZPL_PageSet(int printX, int printY)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("^LH" + printX + "," + printY); //定义条码纸边距  80 10
            return builder.ToString();
        }

        /// <summary>
        /// 打印凭条设置
        /// </summary>
        /// <param name="width">凭条宽度</param>
        /// <param name="height">凭条高度</param>
        /// <returns>返回ZPL命令</returns>
        public string ZPL_SetLabel(int width, int height)
        {
            //ZPL条码设置命令：^PW640^LL480
            string sReturn = "^PW{0}^LL{1}";
            return string.Format(sReturn, width, height);
        }

        /// <summary>
        ///  打印矩形
        /// </summary>
        /// <param name="px">起点X坐标</param>
        /// <param name="py">起点Y坐标</param>
        /// <param name="thickness">边框宽度</param>
        /// <param name="width">矩形宽度，0表示打印一条竖线</param>
        /// <param name="height">矩形高度，0表示打印一条横线</param>
        /// <returns>返回ZPL命令</returns>
        public string ZPL_DrawRectangle(int px, int py, int thickness, int width, int height)
        {
            //ZPL矩形命令：^FO50,50^GB300,200,2^FS
            string sReturn = "^FO{0},{1}^GB{3},{4},{2}^FS";
            return string.Format(sReturn, px, py, thickness, width, height);
        }

        /// <summary>
        /// 打印英文
        /// </summary>
        /// <param name="EnText">待打印文本</param>
        /// <param name="ZebraFont">打印机字体 A-Z</param>
        /// <param name="px">起点X坐标</param>
        /// <param name="py">起点Y坐标</param>
        /// <param name="Orient">旋转角度N = normal，R = rotated 90 degrees (clockwise)，I = inverted 180 degrees，B = read from bottom up, 270 degrees</param>
        /// <param name="Height">字体高度</param>
        /// <param name="Width">字体宽度</param>
        /// <returns>返回ZPL命令</returns>
        public string ZPL_DrawENText(string EnText, string ZebraFont, int px, int py, string Orient, int Height, int Width)
        {
            //ZPL打印英文命令：^FO50,50^A0N,32,25^FDZEBRA^FS
            string sReturn = "^FO{1},{2}^A" + ZebraFont + "{3},{4},{5}^FD{0}^FS";
            return string.Format(sReturn, EnText, px, py, Orient, Height, Width);
        }


        /// <summary>
        /// 中文处理,返回ZPL命令
        /// </summary>
        /// <param name="ChineseText">待转变中文内容</param>
        /// <param name="FontName">字体名称</param>
        /// <param name="startX">X坐标</param>
        /// <param name="startY">Y坐标</param>
        /// <param name="Orient">旋转角度0,90,180,270</param>
        /// <param name="Height">字体高度</param>
        /// <param name="Width">字体宽度，通常是0</param>
        /// <param name="IsBold">1 变粗,0 正常</param>
        /// <param name="IsItalic">1 斜体,0 正常</param>
        /// <returns></returns>
        public string ZPL_DrawCHText(string ChineseText, string FontName, int startX, int startY, int Orient, int Height, int Width, int IsBold, int IsItalic)
        {
            StringBuilder sResult = new StringBuilder();
            StringBuilder hexbuf = new StringBuilder(21 * 1024);
            int count = ZebraHelper.GETFONTHEX(ChineseText, FontName, Orient, Height, Width, IsBold, IsItalic, hexbuf);
            if (count > 0)
            {
                string sEnd = "^FO" + startX.ToString() + "," + startY.ToString() + "^XGOUTSTR" + ",1,2^FS ";
                sResult.AppendLine(hexbuf.ToString().Replace("OUTSTR01", "OUTSTR") + sEnd);
            }
            return sResult.ToString();
        }

        /// <summary>
        /// 打印条形码（128码）
        /// </summary>
        /// <param name="px">起点X坐标</param>
        /// <param name="py">起点Y坐标</param>
        /// <param name="width">基本单元宽度 1-10</param>
        /// <param name="ratio">宽窄比 2.0-3.0 增量0.1</param>
        /// <param name="barheight">条码高度</param>
        /// <param name="barcode">条码内容</param>
        /// <returns>返回ZPL命令</returns>
        public string ZPL_DrawBarcode(int px, int py, int width, int ratio, int barheight, string barcode)
        {
            //ZPL打印英文命令：^FO50,260^BY1,2^BCN,100,Y,N^FDSMJH2000544610^FS
            string sReturn = "^FO{0},{1}^BY{2},{3}^BCN,{4},N,N,N,A^FD{5}^FS";
            return string.Format(sReturn, px, py, width, ratio, barheight, barcode);
        }
        /// <summary>
        /// 二维码列印
        /// </summary>
        /// <param name="px">做边距</param>
        /// <param name="py">上边距</param>
        /// <param name="barcode">条码内容</param>
        /// <returns></returns>
        public string ZPL_Draw2DBarcode(int px, int py, string barcode)
        {
            //5^FDQA  二维码大小
            string sReturn = "^FO{0},{1}^BQ,2,6^FDHA,{2}^FS";
            return string.Format(sReturn, px, py, barcode);
        }


        /// <summary>
        /// 打印图片
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="s_FilePath"></param>
        /// <returns></returns>
        public string ZPL_DrawImage(int px, int py, string s_FilePath)
        {
            int b = 0;
            long n = 0;
            long clr;
            StringBuilder sb = new StringBuilder();
            sb.Append("~DGR:ZONE.GRF,");
            Bitmap bm = new Bitmap(s_FilePath);
            int w = ((bm.Size.Width / 8 + ((bm.Size.Width % 8 == 0) ? 0 : 1)) * bm.Size.Height);
            int h = (bm.Size.Width / 8 + ((bm.Size.Width % 8 == 0) ? 0 : 1));

            sb.Append(w.ToString().PadLeft(5, '0') + "," + h.ToString().PadLeft(3, '0') + ", ");
            using (Bitmap bmp = new Bitmap(bm.Size.Width, bm.Size.Height))
            {
                for (int y = 0; y < bm.Size.Height; y++)
                {
                    for (int x = 0; x < bm.Size.Width; x++)
                    {
                        b = b * 2;
                        clr = bm.GetPixel(x, y).ToArgb();
                        string s = clr.ToString("X");

                        if (s.Substring(s.Length - 6, 6).CompareTo("BBBBBB") < 0)
                        {
                            bmp.SetPixel(x, y, bm.GetPixel(x, y));
                            b++;
                        }
                        n++;
                        if (x == (bm.Size.Width - 1))
                        {
                            if (n < 8)
                            {
                                b = b * (2 ^ (8 - (int)n));

                                sb.Append(b.ToString("X").PadLeft(2, '0'));
                                b = 0;
                                n = 0;
                            }
                        }
                        if (n >= 8)
                        {
                            sb.Append(b.ToString("X").PadLeft(2, '0'));
                            b = 0;
                            n = 0;
                        }
                    }
                }
                sb.Append(string.Format("^LH0,0^FO{0},{1}^XGR:ZONE.GRF^FS", px, py));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把文字转换才Bitmap
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="rect">用于输出的矩形，文字在这个矩形内显示，为空时自动计算</param>
        /// <param name="fontcolor">字体颜色</param>
        /// <param name="backColor">背景颜色</param>
        /// <returns></returns>
        private Bitmap TextToBitmap(string text, Font font, Rectangle rect, Color fontcolor, Color backColor)
        {
            Graphics g;
            Bitmap bmp;
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            if (rect == Rectangle.Empty)
            {
                bmp = new Bitmap(1, 1);
                g = Graphics.FromImage(bmp);
                //计算绘制文字所需的区域大小（根据宽度计算长度），重新创建矩形区域绘图
                SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);

                int width = (int)(sizef.Width + 1);
                int height = (int)(sizef.Height + 1);
                rect = new Rectangle(0, 0, width, height);
                bmp.Dispose();

                bmp = new Bitmap(width, height);
            }
            else
            {
                bmp = new Bitmap(rect.Width, rect.Height);
            }

            g = Graphics.FromImage(bmp);

            //使用ClearType字体功能
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.FillRectangle(new SolidBrush(backColor), rect);
            g.DrawString(text, font, Brushes.Black, rect, format);
            return bmp;
        }

        /*
             中文或其它复杂设计成图片，然后用ZPL命令发送给条码打印机打印             
            //定义字体  
            Font drawFont = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Millimeter);
            //生成图片
            Bitmap img = CreateImage("出厂日期：" + DateTime.Now, drawFont);
            var imgCode = ConvertImageToCode(img);
            var t = ((img.Size.Width / 8 + ((img.Size.Width % 8 == 0) ? 0 : 1)) * img.Size.Height).ToString(); //图形中的总字节数
            var w = (img.Size.Width / 8 + ((img.Size.Width % 8 == 0) ? 0 : 1)).ToString(); //每行的字节数
            string zpl = string.Format("~DGR:imgName.GRF,{0},{1},{2}", t, w, imgCode); //发送给打印机
        */
        /// <summary>
        /// 序列化图片
        /// </summary>
        /// <param name="img">Bitmap</param>
        /// <returns></returns>
        public string ConvertImageToCode(Bitmap img)
        {
            var sb = new StringBuilder();
            long clr = 0, n = 0;
            int b = 0;
            for (int i = 0; i < img.Size.Height; i++)
            {
                for (int j = 0; j < img.Size.Width; j++)
                {
                    b = b * 2;
                    clr = img.GetPixel(j, i).ToArgb();
                    string s = clr.ToString("X");

                    if (s.Substring(s.Length - 6, 6).CompareTo("BBBBBB") < 0)
                    {
                        b++;
                    }
                    n++;
                    if (j == (img.Size.Width - 1))
                    {
                        if (n < 8)
                        {
                            b = b * (2 ^ (8 - (int)n));

                            sb.Append(b.ToString("X").PadLeft(2, '0'));
                            b = 0;
                            n = 0;
                        }
                    }
                    if (n >= 8)
                    {
                        sb.Append(b.ToString("X").PadLeft(2, '0'));
                        b = 0;
                        n = 0;
                    }
                }
                sb.Append(System.Environment.NewLine);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 中文处理
        /// </summary>
        /// <param name="ChineseText">待转变中文内容</param>
        /// <param name="FontName">字体名称</param>
        /// <param name="Orient">旋转角度0,90,180,270</param>
        /// <param name="Height">字体高度</param>
        /// <param name="Width">字体宽度，通常是0</param>
        /// <param name="IsBold">1 变粗,0 正常</param>
        /// <param name="IsItalic">1 斜体,0 正常</param>
        /// <param name="ReturnPicData">返回的图片字符</param>
        /// <returns></returns>
        [DllImport("fnthex32.dll")]
        public static extern int GETFONTHEX(
            string ChineseText,
            string FontName,
            int Orient,
            int Height,
            int Width,
            int IsBold,
            int IsItalic,
            StringBuilder ReturnPicData);

        /// <summary>
        /// 中文处理
        /// </summary>
        /// <param name="ChineseText">待转变中文内容</param>
        /// <param name="FontName">字体名称</param>
        /// <param name="FileName">返回的图片字符重命</param>
        /// <param name="Orient">旋转角度0,90,180,270</param>
        /// <param name="Height">字体高度</param>
        /// <param name="Width">字体宽度，通常是0</param>
        /// <param name="IsBold">1 变粗,0 正常</param>
        /// <param name="IsItalic">1 斜体,0 正常</param>
        /// <param name="ReturnPicData">返回的图片字符</param>
        /// <returns></returns>
        [DllImport("fnthex32.dll")]
        public static extern int GETFONTHEX(
                              string ChineseText,
                              string FontName,
                              string FileName,
                              int Orient,
                              int Height,
                              int Width,
                              int IsBold,
                              int IsItalic,
                              StringBuilder ReturnPicData);

        #region 扩展
        /// <summary>
        /// 毫米转像素 取整
        /// </summary>
        /// <param name="mm">毫米</param>
        /// <param name="dpi">打印DPI 如300</param>
        /// <returns></returns>
        public double mm2px(double mm, double dpi)
        {
            double px = (mm / 25.4) * dpi;
            return Math.Round(px, 0, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 像素转毫米 取整
        /// </summary>
        /// <param name="px">像素</param>
        /// <param name="dpi">打印DPI 如300</param>
        /// <returns></returns>
        public double px2mm(double px, double dpi)
        {
            //像素转换成毫米公式：(宽度像素/水平DPI)*25.4;
            double mm = (px / dpi) * 25.4;
            return Math.Round(mm, 0, MidpointRounding.AwayFromZero);
        }
        #endregion
    }
}
