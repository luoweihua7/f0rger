using System;
using System.Collections.Generic;
using System.Text;

namespace f0rger
{
    /// <summary>
    /// 文件挂载列表，与界面显示列表对应
    /// </summary>
    [Serializable]
    public class FileMockEntity
    {
        /// <summary>
        /// 是否挂载
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 文件目录或地址
        /// </summary>
        public string Path { get; set; }
    }

    [Serializable]
    public class FileMockEntityList : List<FileMockEntity>
    {
        //继承自泛型List,并标记为可序列化
    }
}
