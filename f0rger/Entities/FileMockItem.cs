using System;
using System.Collections.Generic;
using System.Text;

namespace f0rger
{
    /// <summary>
    /// 挂载文件对象
    /// </summary>
    public class FileMockItem
    {
        /// <summary>
        /// 文件名称,包含扩展名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 完整的文件路径
        /// </summary>
        public string Path { get; set; }
    }
}
