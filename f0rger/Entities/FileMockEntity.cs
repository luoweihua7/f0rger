using System;
using System.Collections.Generic;
using System.Text;

namespace f0rger
{
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
}
