using System;
using System.Collections.Generic;
using System.Text;

namespace f0rger
{
    public class LogService
    {
        /// <summary>
        /// 日志记录,日志信息显示在fiddler中的日志面板中
        /// </summary>
        /// <param name="msg">日志信息,自动在前面加上时间段</param>
        /// <param name="enable">可选,是否显示在fiddler中,默认为true</param>
        public static void Log(string msg, bool enable = true)
        {
            if (enable)
            {
                Fiddler.FiddlerApplication.Log.LogString("[" + Configs.AppName + "] " + msg);
            }
        }
    }
}
