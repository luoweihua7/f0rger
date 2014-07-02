using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization;

namespace f0rger
{
    /// <summary>
    /// 功能设置参数
    /// </summary>
    [Serializable]
    public class ConfigEntity
    {
        /// <summary>
        /// 是否启用插件
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 是否显示右下角的挂载提示窗口
        /// </summary>
        public bool ShowTip { get; set; }

        /// <summary>
        /// 是否限速
        /// </summary>
        public bool SpeedLimit { get; set; }

        /// <summary>
        /// 是否显示日志信息
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        /// 挂载配置(如灰度,全网等)
        /// </summary>
        public ProfileEntityList Profiles { get; set; }

        /// <summary>
        /// 挂载的文件列表
        /// </summary>
        public FileMockEntityList Files { get; set; }

        /// <summary>
        /// 配置类
        /// </summary>
        public ConfigEntity()
        {
            Enable = true;
            ShowTip = true;
            DebugMode = false;
            Profiles = new ProfileEntityList();
            Files = new FileMockEntityList();
        }
    }
}
