using System;
using System.Collections.Generic;
using System.Text;

namespace f0rger
{
    /// <summary>
    /// 各项系统设置参数
    /// </summary>
    public class Configs
    {
        /// <summary>
        /// 提示的小窗口显示时间
        /// </summary>
        public static int dueTime = 1500;

        private static string MyDocumentsDir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static string AppName = "Fiddler2";
        private static string ConfigName = "config.dat";

        /// <summary>
        /// 配置的保存目录
        /// </summary>
        public static string ConfigDir = MyDocumentsDir + @"\" + AppName + @"\Config";
        /// <summary>
        /// 配置文件的保存路径
        /// </summary>
        public static string ConfigPath = ConfigDir + @"\" + ConfigName;

        /// <summary>
        /// 命中规则后的颜色显示
        /// </summary>
        public static string Color = "#000000";
        public static string BgColor = "#f0f0f0";

        public static string TabName = "f0rger";
    }
}
