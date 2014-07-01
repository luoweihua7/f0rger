using System;
using System.Collections.Generic;
using System.Text;

namespace f0rger
{
    /// <summary>
    /// 挂载的站点配置
    /// </summary>
    [Serializable]
    public class ProfileEntity
    {
        /// <summary>
        /// 配置文件的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否启用此配置
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 域名列表
        /// </summary>
        public List<string> Hosts { get; set; }
    }

    [Serializable]
    public class ProfileEntityList : List<ProfileEntity>
    {
        //继承自泛型List,并标记为可序列化
    }
}
