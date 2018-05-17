using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BZ.Common.Delegate
{
    public class TaskServerDisplayLog
    {
        public static RichTextBox richtxtServerInfo;

        private delegate void ServerLogAppendDelegate(Color color, string text);

        /// <summary> 
        /// 追加显示文本 
        /// </summary> 
        /// <param name="color">文本颜色</param> 
        /// <param name="text">显示文本</param> 
        private static void ServerLogAppend(Color color, string text)
        {
            if (richtxtServerInfo.Lines.Length > 10000)
            {
                richtxtServerInfo.Text = richtxtServerInfo.Text.Remove(0, richtxtServerInfo.Lines[0].Length + 1);
            }
            string NowDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ");
            richtxtServerInfo.SelectionColor = color;
            richtxtServerInfo.AppendText("●" + NowDate + "━━━≯" + text);
            richtxtServerInfo.AppendText("\n");

            richtxtServerInfo.SelectionStart = richtxtServerInfo.Text.Length;
            richtxtServerInfo.ScrollToCaret();
        }

        /// <summary> 
        /// 显示错误日志 
        /// </summary> 
        /// <param name="text"></param> 
        public static void ServerLogError(string text)
        {
            ServerLogAppendDelegate la = new ServerLogAppendDelegate(ServerLogAppend);
            richtxtServerInfo.Invoke(la, Color.Red, text);
        }
        /// <summary> 
        /// 显示警告信息 
        /// </summary> 
        /// <param name="text"></param> 
        public static void ServerLogWarning(string text)
        {
            ServerLogAppendDelegate la = new ServerLogAppendDelegate(ServerLogAppend);
            richtxtServerInfo.Invoke(la, Color.Violet, text);
        }
        /// <summary> 
        /// 显示信息 
        /// </summary> 
        /// <param name="text"></param> 
        public static void ServerLogMessage(string text)
        {
            ServerLogAppendDelegate la = new ServerLogAppendDelegate(ServerLogAppend);
            richtxtServerInfo.Invoke(la, Color.Black, text);
        }
    }
}
