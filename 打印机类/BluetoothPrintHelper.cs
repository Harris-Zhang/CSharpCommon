using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// 蓝牙打印类
    /// </summary>
    public class BluetoothPrintHelper
    {
        #region
        /// <summary>
        /// 初始化打印机
        /// 清除打印缓存，各参数恢复默认值
        /// </summary>
        /// <returns></returns>
        public string InitializationPinter()
        {
            string data = "0x1b 0x40 ";
            return data;
        }

        /// <summary>
        /// 打印测试页
        /// 打印一张测试页，上面包含打印机的程序版本，通讯借口类型，代码页和其他一些数据
        /// </summary>
        /// <returns></returns>
        public string PrintTestPage()
        {
            string data = "0x12 0x54 ";
            return data;
        }

        /// <summary>
        /// 打印&进纸
        /// 打印缓存里的内容，之后根据当前的行间距 进纸一行，并调整打印位置到下一行的起始位置
        /// </summary>
        /// <returns></returns>
        public string PrintInPager()
        {
            string data = "0x0a ";
            return data;
        }

        /// <summary>
        /// 打印位置调整至本行起始位置，不换行
        /// </summary>
        /// <returns></returns>
        public string ReturnStartPosit()
        {
            string data = "0x0d";
            return data;
        }

        /// <summary>
        /// 打印&进纸N点
        /// 打印缓存里的内容   进纸N点
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string PrintInPager_N_Point(int n)
        {
            string data = "0x1b 0x4a " + Convert.ToString(n, 16) + " ";
            return data;
        }

        /// <summary>
        /// 打印&进纸N行
        /// 打印缓存里的内容， 进纸N行
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string PrintInPager_N_Rows(int n)
        {
            string data = "0x1b 0x64 " + Convert.ToString(n, 16) + " ";
            return data;
        }

        /// <summary>
        /// 设置行间距为N点
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string SetRowSpacing_N_Point(int n)
        {
            string data = "0x1b 0x33 " + Convert.ToString(n, 16) + " ";
            return data;
        }

        /// <summary>
        /// 设置行间距为默认值
        /// </summary>
        /// <returns></returns>
        public string SetRowSpacingDefault()
        {
            string data = "0x1b 0x32 ";
            return data;
        }
        /// <summary>
        /// 设置打印位置
        /// </summary>
        /// <param name="nL"></param>
        /// <param name="nH"></param>
        /// <returns></returns>
        public string SetPrintPosition(int nL, int nH)
        {
            string data = "0x1b 0x24 " + Convert.ToString(nL, 16) + " " + Convert.ToString(nH, 16);
            return data;
        }
        /// <summary>
        /// 设置字符打印样式
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string SetPrintCharStyle(int n)
        {
            string data = "0x1b 0x21 0x01 ";
            return data;
        }

        /// <summary>
        /// 打印二维码
        /// </summary>
        /// <param name="v">二维码规格()</param>
        /// <param name="r">纠错等级</param>
        /// <param name="nL">数据长度</param>
        /// <param name="nH"></param>
        /// <returns></returns>
        public string PrintDrawQRCode(int v, int r, int nL, int nH)
        {
            string data = "0x1d 0x6b 0x61 " + Convert.ToString(v, 16) + " " + Convert.ToString(r, 16) + " " + Convert.ToString(nL, 16) + " " + Convert.ToString(nH, 16) + " ";
            return data;
        }
        #endregion

        #region
        /**
	 * 描述: 指示一个 Page 页面的开始，并设置 Page 页的大小，参考点坐标和页面旋转角度。
	 * @param startx 页面起始点x坐标
	 * @param starty 页面起始点y坐标
	 * @param width 页面页宽 startx + width 的范围为[1,384]。编写SDK的时候，该打印机一行的打印点数为384点。如果你不确定每行打印点数，请参考打印机规格书。一般来说有384,576,832这三种规格。
	 * @param height 页面页高 starty + height 的范围[1,936]。编写SDK的时候，限制是936，但是这个值并不确定，这和打印机的资源有关。即便如此，也不建议把页宽设置过大。建议页宽和页高设置和标签纸匹配即可。
	 * @param rotate 页面旋转。 rotate的取值范围为{0,1}。为0，页面不旋转打印，为1，页面旋转90度打印。
	 */
        //public static byte[] PageBegin(int startx, int starty, int width, int height, int rotate)
        //{
        //    byte[] data = new byte[12];

        //    data[0] = 0x1A;
        //    data[1] = 0x5B;
        //    data[2] = 0x01;

        //    data[3] = (byte)(startx & 0xFF);
        //    data[4] = (byte)((startx >> 8) & 0xFF);
        //    data[5] = (byte)(starty & 0xFF);
        //    data[6] = (byte)((starty >> 8) & 0xFF);

        //    data[7] = (byte)(width & 0xFF);
        //    data[8] = (byte)((width >> 8) & 0xFF);
        //    data[9] = (byte)(height & 0xFF);
        //    data[10] = (byte)((height >> 8) & 0xFF);

        //    data[11] = (byte)(rotate & 0xFF);

        //    return data;

        //}
        public string PageBegin(int startx, int starty, int width, int height, int rotate)
        {
            string data = "0x1A 0x5B 0x01 ";
            data += (startx & 0xFF) + " ";
            data += ((startx >> 8) & 0xFF) + " ";
            data += (starty & 0xFF) + " ";
            data += ((starty >> 8) & 0xFF) + " ";

            data += (width & 0xFF) + " ";
            data += ((width >> 8) & 0xFF) + " ";
            data += (height & 0xFF) + " ";
            data += ((height >> 8) & 0xFF) + " ";

            data += (rotate & 0xFF) + " ";
            return data;
        }

        /**
         * 描述: 只是一个Page页面的结束。
         */
        public static byte[] PageEnd()
        {
            byte[] data = new byte[] { 0x1A, 0x5D, 0x00 };

            return data;
        }

        /**
        * 描述: 将 Page 页上的内容打印到标签纸上。
        * num: 打印的次数，1-255。
        */
        public static byte[] PagePrint(int num)
        {
            byte[] data = new byte[] { 0x1A, 0x4F, 0x01, 0x01 };

            data[3] = (byte)(num & 0xFF);

            return data;
        }

        /**
        * 描述: 在 Page 页面上指定位置绘制文本。只在一行展开，打印机返回占用的区域的坐标值。
        * startx: 定义文本起始位置 x 坐标，取值范围：[0, Page_Width-1]
        * starty: 定义文本起始位置 y 坐标，取值范围：[0, Page_Height-1]
        * font: 选择字体，有效值范围为{16, 24, 32, 48, 64, 80, 96}
        * style: 字符风格。
        *			数据位	定义
        *			0		加粗标志位：置 1 字体加粗，清零则字体不加粗。
        *			1		下划线标志位：置 1 文本带下划线，清零则无下划线。
        *			2		反白标志位：置 1 文本反白(黑底白字)，清零不反白。
        *			3		删除线标志位：置 1 文本带删除线，清零则无删除线。
        *			[5,4]	旋转标志位：00 旋转 0° ； 01 旋转 90°； 10 旋转 180°； 11 旋转 270°；
        *			[11,8]	字体宽度放大倍数；
        *			[15,12]	字体高度放大倍数；
        * str: 以00结尾的字符串数据流
        */
        public static byte[] DrawPlainText(int startx, int starty, int font, int style, byte[] str)
        {
            int datalen = 11 + str.Length + 1;
            byte[] data = new byte[datalen];

            data[0] = 0x1A;
            data[1] = 0x54;
            data[2] = 0x01;

            data[3] = (byte)(startx & 0xFF);
            data[4] = (byte)((startx >> 8) & 0xFF);
            data[5] = (byte)(starty & 0xFF);
            data[6] = (byte)((starty >> 8) & 0xFF);

            data[7] = (byte)(font & 0xFF);
            data[8] = (byte)((font >> 8) & 0xFF);
            data[9] = (byte)(style & 0xFF);
            data[10] = (byte)((style >> 8) & 0xFF);

            copyBytes(str, 0, data, 11, str.Length);
            data[datalen - 1] = 0;

            return data;
        }

        /**
        * 描述: 在 Page 页指定两点间绘制一条直线段。
        * startx: 直线段起始点 x 坐标值，取值范围：[0, Page_Width-1]。
        * starty: 直线段起始点 y 坐标值，取值范围：[0，Page_Height-1]。
        * endx: 直线段终止点 x 坐标值，取值范围：[0, Page_Width-1]。
        * endy: 直线段终止点 y 坐标值，取值范围：[0,Page_Height-1]。
        * width: 直线段线宽，取值范围：[1，Page_Height-1]。
        * color: 直线段颜色，取值范围：{0, 1}。当 Color 为 1 时，线段为黑色。当 Color 为 0 时，线段为白色。
        */
        public static byte[] DrawLine(int startx, int starty, int endx, int endy, int width, int color)
        {
            byte[] data = new byte[14];

            data[0] = 0x1A;
            data[1] = 0x5C;
            data[2] = 0x01;

            data[3] = (byte)(startx & 0xFF);
            data[4] = (byte)((startx >> 8) & 0xFF);
            data[5] = (byte)(starty & 0xFF);
            data[6] = (byte)((starty >> 8) & 0xFF);

            data[7] = (byte)(endx & 0xFF);
            data[8] = (byte)((endx >> 8) & 0xFF);
            data[9] = (byte)(endy & 0xFF);
            data[10] = (byte)((endy >> 8) & 0xFF);

            data[11] = (byte)(width & 0xFF);
            data[12] = (byte)((width >> 8) & 0xFF);

            data[13] = (byte)(color & 0xFF);

            return data;
        }

        /**
        * 描述: 在 Page 页指定位置绘制指定大小的矩形框。
        * left: 矩形框左上角 x 坐标值，取值范围：[0, Page_Width-1]。
        * top: 矩形框左上角 y 坐标值。取值范围：[0, Page_Height-1]。
        * right: 矩形框右下角 x 坐标值。取值范围：[0, Page_Width-1]。
        * bottom: 矩形框右下角 y 坐标值。取值范围：[0, Page_Height-1]。
        * boardwidth: 矩形框线宽。
        * bordercolor: 矩形框线颜色，曲直范围{0，1}。当 Color = 1 时，绘制黑色矩形宽，Color = 0 时，绘制白色矩形框。
        */
        public static byte[] DrawBox(int left, int top, int right, int bottom, int borderwidth, int bordercolor)
        {
            byte[] data = new byte[14];

            data[0] = 0x1A;
            data[1] = 0x26;
            data[2] = 0x01;

            data[3] = (byte)(left & 0xFF);
            data[4] = (byte)((left >> 8) & 0xFF);
            data[5] = (byte)(top & 0xFF);
            data[6] = (byte)((top >> 8) & 0xFF);

            data[7] = (byte)(right & 0xFF);
            data[8] = (byte)((right >> 8) & 0xFF);
            data[9] = (byte)(bottom & 0xFF);
            data[10] = (byte)((bottom >> 8) & 0xFF);

            data[11] = (byte)(borderwidth & 0xFF);
            data[12] = (byte)((borderwidth >> 8) & 0xFF);

            data[13] = (byte)(bordercolor & 0xFF);

            return data;
        }

        /**
        * 描述: 在 Page 页指定位置绘制矩形块。
        * left: 矩形块左上角 x 坐标值，取值范围：[0, Page_Width-1]。
        * top: 矩形块左上角 y 坐标值。取值范围：[0, Page_Height-1]。
        * right: 矩形块右下角 x 坐标值。取值范围：[0, Page_Width-1]。
        * bottom: 矩形块右下角 y 坐标值。取值范围：[0, Page_Height-1]。
        * color: 矩形块颜色，取值范围：{0, 1}。当 Color 为 1 时，矩形块为黑色。当 Color 为 0时，矩形块为白色。
        */
        public static byte[] DrawRectangel(int left, int top, int right, int bottom, int color)
        {
            byte[] data = new byte[12];

            data[0] = 0x1A;
            data[1] = 0x2A;
            data[2] = 0x00;

            data[3] = (byte)(left & 0xFF);
            data[4] = (byte)((left >> 8) & 0xFF);
            data[5] = (byte)(top & 0xFF);
            data[6] = (byte)((top >> 8) & 0xFF);

            data[7] = (byte)(right & 0xFF);
            data[8] = (byte)((right >> 8) & 0xFF);
            data[9] = (byte)(bottom & 0xFF);
            data[10] = (byte)((bottom >> 8) & 0xFF);

            data[11] = (byte)(color & 0xFF);

            return data;
        }

        /**
        * 描述: 在 Page 页指定位置绘制一维条码。
        * startx: 条码左上角 x 坐标值，取值范围：[0, Page_Width-1]。
        * starty: 条码左上角 y 坐标值，取值范围：[0, Page_Height-1]。
        * type: 标识条码类型，取值范围：[0, 29]。各值定义如下：
        *		type	类型	长度	条码值范围（十进制）
        0 UPC-A     11          48-57
        1 UPC-E     6           48-57
        2 EAN13     12          48-57
        3 EAN8      7           48-57
        4 CODE39     1-         48-57,65-90,32,36,37,43,45,46,47
        5 I25        1-偶数     48-57
        6 CODABAR    1-         48-57,65-68,36,43,45,46,47,58
        7 CODE93     1-255      0-127
        8 CODE128    2-255      0-127
        9 CODE11
        10 MSI
        11 "128M",     // 可以根据数据切换编码模式-> !096 - !105
        12 "EAN128",   // 自动切换编码模式
        13 "25C",      // 25C Check use mod 10-> 奇数先在前面补0， 10的倍数-[(奇数位的数字之和<从左至右)+(偶数位数字之和)*3]
        14 "39C",      // 39碼的檢查碼必須搭配「檢查碼相對值對照表」，如表所示，將查出的相對值累加後再除以43，得到的餘數再查出相對的編碼字元，即為檢查碼字元。
        15 "39",       // Full ASCII 39 Code, 特殊字符用两个可表示的字来表示, 39C 同样是包含Full ASCII, 注意宽窄比处理
        16 "EAN13+2",  // 附加码与主码间隔 7-12 单位，起始为 1011 间隔为 01 ，(_0*10+_1) Mod 4-> 0--AA 1--AB 2--BA 3--BB
        17 "EAN13+5",  // 附加码部分同上，模式((_0+_2+_4)*3+(_1+_3)*9) mod 10 ->"bbaaa", "babaa", "baaba", "baaab", "abbaa", "aabba", "aaabb", "ababa", "abaab", "aabab
        18 "EAN8+2",   // 同 EAN13+2
        19 "EAN8+5",   // 同 EAN13+5
        20 "POST",     // 详见规格说明，是高低条码，不是宽窄条码
        21 "UPCA+2",   // 附加码见 EAN
        22 "UPCA+5",   // 附加码见 EAN
        23 "UPCE+2",   // 附加码见 EAN
        24 "UPCE+5",   // 附加码见 EAN
        25 "CPOST",    // 测试不打印。。。
        26 "MSIC",     // 将检查码作为数据再计算一次检查码
        27 "PLESSEY",  // 测试不打印。。。
        28 "ITF14",    // 25C 变种， 第一个数前补0，检查码计算时需扣除最后一个数，但仍填充为最尾端
        29 "EAN14"
        * height: 定义条码高度。
        * uniwidth: 定义条码码宽。取值范围：[1, 4]。各值定义如下：
        *		Width取值	多级条码单位宽度（mm）	二进制条码窄线条宽度	二进制条码宽线条宽度
        1			0.125					0.125					0.25
        2			0.25					0.25					0.50
        3			0.375					0.375					0.75
        4			0.50					0.50					1.0
        * rotate: 表示条码旋转角度。取值范围：[0, 3]。各值定义如下：
        *		Rotate取值	定义
        0			条码不旋转绘制。
        1			条码旋转 90°绘制。
        2			条码旋转 180°绘制。
        3			条码旋转 270°绘制。
        * str: 以 0x00 结尾的文本字符数据流。
        */
        public static byte[] DrawBarcode(int startx, int starty, int type, int height, int unitwidth, int rotate, byte[] str)
        {
            int datalen = 11 + str.Length + 1;
            byte[] data = new byte[datalen];

            data[0] = 0x1A;
            data[1] = 0x30;
            data[2] = 0x00;

            data[3] = (byte)(startx & 0xFF);
            data[4] = (byte)((startx >> 8) & 0xFF);
            data[5] = (byte)(starty & 0xFF);
            data[6] = (byte)((starty >> 8) & 0xFF);

            data[7] = (byte)(type & 0xFF);
            data[8] = (byte)(height & 0xFF);
            data[9] = (byte)(unitwidth & 0xFF);
            data[10] = (byte)(rotate & 0xFF);

            copyBytes(str, 0, data, 11, str.Length);
            data[datalen - 1] = 0;

            return data;
        }

        /**
        * 描述: 在 Page 页指定位置绘制 QRCode 码。
        * startx: QRCode 码左上角 x 坐标值，取值范围：[0，Page_Width-1]。
        * starty: QRCode 码左上角 y 坐标值，取值范围：[0, Page_Height-1]。
        * version: 指定字符版本。取值范围：[0,20]。当 version 为 0 时，打印机根据字符串长度自动计算版本号。
        * ecc: 指定纠错等级。取值范围：[1, 4]。各值定义如下：
        *		ECC	纠错等级
        1	L：7%，低纠错，数据多。
        2	M：15%，中纠错
        3	Q：优化纠错
        4	H：30%，最高纠错，数据少。
        * unitwidth: QRCode 码码块，取值范围：[1, 4]。各值定义与一维条码指令输入参数UniWidth相同。
        * rotate: QRCode 码旋转角度，取值范围：[0, 3]。各值定义与一维条码指令输入参数Rotate 相同。
        * str: 以 0x00 终止的 QRCode 文本字符数据流。
        */
        public static byte[] DrawQRCode(int startx, int starty, int version, int ecc, int unitwidth, int rotate, byte[] str)
        {
            int datalen = 11 + str.Length + 1;
            byte[] data = new byte[datalen];

            data[0] = 0x1A;
            data[1] = 0x31;
            data[2] = 0x00;

            data[3] = (byte)(version & 0xFF);
            data[4] = (byte)(ecc & 0xFF);

            data[5] = (byte)(startx & 0xFF);
            data[6] = (byte)((startx >> 8) & 0xFF);
            data[7] = (byte)(starty & 0xFF);
            data[8] = (byte)((starty >> 8) & 0xFF);

            data[9] = (byte)(unitwidth & 0xFF);
            data[10] = (byte)(rotate & 0xFF);

            copyBytes(str, 0, data, 11, str.Length);
            data[datalen - 1] = 0;

            return data;
        }

        /**
        * 描述:  在 Page 页指定位置绘制 PDF417 条码 。
        * startx: PDF417 码左上角 x 坐标值，取值范围：[0，Page_Width-1]。
        * starty: PDF417 码左上角 y 坐标值，取值范围：[0, Page_Height-1]。
        * colnum: ColNum 为列数，表述每行容纳多少码字。一个码字为 17*UnitWidth 个点。行数由打印机自动产生，行数范围限定为 3~90。ColNum 的取值范围：[1,30]。
        * lwratio: 宽高比。取值范围：[3,5]。
        * ecc: 纠错等级，取值范围：[0. 8]。
        *		ecc取值	纠错码数	可存资料量（字节）
        0		2			1108
        1		4			1106
        2		8			1101
        3		16			1092
        4		32			1072
        5		64			1024
        6		128			957
        7		256			804
        8		512			496
        * unitwidth: PDF417 码码块，取值范围：[1, 3]。各值定义与一维条码指令输入参数 UniWidth 相同。
        * rotate: PDF417 码旋转角度，取值范围：[0, 3]。各值定义与一维条码指令输入参数 Rotate 相同。
        * str: 以 0x00 终止的 PDF417 文本字符数据流。
        */
        public static byte[] DrawPDF417(int startx, int starty, int colnum, int lwratio, int ecc, int unitwidth, int rotate, byte[] str)
        {
            int datalen = 12 + str.Length + 1;
            byte[] data = new byte[datalen];

            data[0] = 0x1A;
            data[1] = 0x31;
            data[2] = 0x01;

            data[3] = (byte)(colnum & 0xFF);
            data[4] = (byte)(ecc & 0xFF);
            data[5] = (byte)(lwratio & 0xFF);

            data[6] = (byte)(startx & 0xFF);
            data[7] = (byte)((startx >> 8) & 0xFF);
            data[8] = (byte)(starty & 0xFF);
            data[9] = (byte)((starty >> 8) & 0xFF);

            data[10] = (byte)(unitwidth & 0xFF);
            data[11] = (byte)(rotate & 0xFF);

            copyBytes(str, 0, data, 12, str.Length);
            data[datalen - 1] = 0;

            return data;
        }

        /**
        * 描述: 在 Page 页指定位置绘制位图。
        * startx: 位图左上角 x 坐标值，取值范围：[0, Page_Width]。
        * starty: 位图左上角 y 坐标值，取值范围：[0, Page_Height]。
        * width: 位图的像素宽度。
        * height: 位图的像素高度。
        * style: 位图打印特效，各位定义如下：
        *		位		定义
        0		反白标志位，置 1 位图反白打印，清零正常打印。
        [2:1]	旋转标志位： 00 旋转 0° ； 01 旋转 90°； 10 旋转 180°； 11 旋转 270°
        [7:3]	保留。
        [11:8]	位图宽度放大倍数。
        [15:16]	位图高度放大倍数。
        * pdata: 位图的点阵数据。
        */
        public static byte[] DrawBitmap(int startx, int starty, int width, int height, int style, byte[] pdata)
        {
            int datalen = 13 + width * height / 8;
            byte[] data = new byte[datalen];

            data[0] = 0x1A;
            data[1] = 0x21;
            data[2] = 0x01;

            data[3] = (byte)(startx & 0xFF);
            data[4] = (byte)((startx >> 8) & 0xFF);
            data[5] = (byte)(starty & 0xFF);
            data[6] = (byte)((starty >> 8) & 0xFF);

            data[7] = (byte)(width & 0xFF);
            data[8] = (byte)((width >> 8) & 0xFF);
            data[9] = (byte)(height & 0xFF);
            data[10] = (byte)((height >> 8) & 0xFF);

            data[11] = (byte)(style & 0xFF);
            data[12] = (byte)((style >> 8) & 0xFF);

            copyBytes(pdata, 0, data, 13, pdata.Length);

            return data;
        }

        public static void copyBytes(byte[] orgdata, int orgstart, byte[] desdata,
            int desstart, int copylen)
        {
            for (int i = 0; i < copylen; i++)
            {
                desdata[desstart + i] = orgdata[orgstart + i];
            }
        }
        #endregion
    }
}
