using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.IO;

namespace f0rger
{
    /// <summary>
    /// 添加到列表的文件对象
    /// <para>主要用于程序内的文件列表获取等</para>
    /// </summary>
    public class FileHookEntity
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
        /// 添加到列表的文件对象
        /// </summary>
        /// <param name="path">文件路径或者文件夹路径</param>
        /// <param name="enable">是否挂载,默认挂载</param>
        /// <param name="refresh">是否立即刷新文件列表数据,默认立即刷新</param>
        public FileHookEntity(string path, bool enable = true, bool refresh = true)
        {
            FilePath = path;
            Enable = enable;
            Files = new List<string>();

            Initialize(refresh); //初始化时,可能不需要刷新文件列表
        }

        /// <summary>
        /// 根据路径Path初始化文件列表Files
        /// </summary>
        private void Initialize(bool refresh = true)
        {
            if (!refresh) return;

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
