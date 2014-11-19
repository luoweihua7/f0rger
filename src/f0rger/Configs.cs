using System;

namespace f0rger
{
    /// <summary>
    /// 各项系统设置参数
    /// </summary>
    public class Configs
    {
        #region 功能参数等配置项

        /// <summary>
        /// 是否启用挂载
        /// </summary>
        public static bool Enable = false;

        /// <summary>
        /// 是否启用挂载提示
        /// <para>挂载文件命中后的右下角弹窗</para>
        /// </summary>
        public static bool EnableTip = false;

        /// <summary>
        /// 是否限速
        /// </summary>
        public static bool EnableLimit = false;

        /// <summary>
        /// 严格模式
        /// <para>严格匹配路径,仅对目录挂载有效</para>
        /// </summary>
        public static bool StrictMode = false;

        /// <summary>
        /// 速度限制
        /// <para>单位为: KB/s</para>
        /// </summary>
        public static int LimitSpeed = 1024000;

        /// <summary>
        /// 是否启用配置项
        /// </summary>
        public static bool EnableProfile = false;

        /// <summary>
        /// 挂载配置项
        /// <para>如配置的灰度,全网等域名规则</para>
        /// </summary>
        public static ProfileEntityList Profiles = new ProfileEntityList();

        /// <summary>
        /// 挂载的文件(夹)列表
        /// </summary>
        public static FileMockEntityList Files = new FileMockEntityList();

        #endregion


        #region 程序内使用的可配置参数

        /// <summary>
        /// 提示的小窗口显示时间
        /// </summary>
        public static int dueTime = 2000;

        /// <summary>
        /// 配置的保存目录
        /// </summary>
        public static string ConfigDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Fiddler2\Config";
        /// <summary>
        /// 配置文件的保存路径
        /// </summary>
        public static string ConfigPath = ConfigDir + @"\config.dat";

        /// <summary>
        /// 命中规则后,列表中的请求项字体颜色
        /// </summary>
        public static string Color = "#000000";
        /// <summary>
        /// 命中规则后,列表中的请求项背景色
        /// </summary>
        public static string BgColor = "#e9e9e9";

        /// <summary>
        /// 应用名称
        /// </summary>
        public static string AppName = "F0rger"; //just a name

        public static string FolderName = "{{FOLDER}}";

        #endregion
    }
}
