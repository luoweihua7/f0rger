using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.IO;

namespace f0rger
{
    /// <summary>
    /// 添加到列表的文件对象
    /// </summary>
    public class FileHookItem
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 文件或文件夹的路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 对应路径下的所有文件
        /// </summary>
        public List<string> Files { get; set; }

        /// <summary>
        /// 安插一个监听类,在文件发生变动时,可以快速的刷新列表
        /// <para>TODO</para>
        /// </summary>
        private FileSystemWatcher fsw = new FileSystemWatcher();

        public FileHookItem(string path, bool enable = true)
        {
            FilePath = path;
            Enable = enable;
            Files = new List<string>();

            Initialize();
        }

        /// <summary>
        /// 根据路径Path初始化文件列表Files
        /// </summary>
        private void Initialize()
        {
            if (Directory.Exists(FilePath))
            {
                //如果是目录,遍历
                string[] files = Directory.GetFiles(FilePath, "*.*", SearchOption.AllDirectories);
                for (int i = 0, len = files.Length; i < len; i++)
                {
                    string file = files[i];
                    Files.Add(file);
                }
            }
            else
            {
                //文件,或者文件根本不存在
                Files.Add(FilePath);
            }
        }

        /// <summary>
        /// 刷新文件列表
        /// </summary>
        public void Refresh()
        {
            Files.Clear();
            Initialize();
        }
    }
}
