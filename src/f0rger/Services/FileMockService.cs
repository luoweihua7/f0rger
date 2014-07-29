using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.IO;

namespace f0rger
{
    public class FileMockService
    {
        /// <summary>
        /// 从配置中读取挂载列表
        /// <para>引用类型</para>
        /// </summary>
        private static FileMockEntityList Files = Configs.Files;

        /// <summary>
        /// 挂载中的配置文件列表
        /// <para>用于快速索引</para>
        /// </summary>
        private static Hashtable htFiles = new Hashtable();

        /// <summary>
        /// 需要挂载的文件列表,已去重
        /// <para>文件是否需要挂载,主要看这个哈希表</para>
        /// </summary>
        private static Hashtable fileMockList = new Hashtable();

        /// <summary>
        /// 重复列表
        /// <para>文件名重复,路径不重复</para>
        /// </summary>
        private static Hashtable duplicateList = new Hashtable();

        /// <summary>
        /// ListView中的文件列表
        /// </summary>
        private static Hashtable listviewFiles = new Hashtable();

        #region 供界面调用的方法

        /// <summary>
        /// 批量添加挂载文件到列表
        /// </summary>
        /// <param name="files">挂载的文件列表</param>
        public static void Add(FileMockEntityList files)
        {
            if (files != null && files.Count > 0)
            {
                foreach (FileMockEntity entity in files)
                {
                    Add(entity.Path, entity.Enable, false);
                }

                Refresh();
            }
        }

        /// <summary>
        /// 添加文件或者文件夹到挂载列表
        /// </summary>
        /// <param name="file">文件或者文件夹的完整路径</param>
        /// <param name="enable">是否启用挂载</param>
        /// <param name="refresh">是否刷新挂载列表.初始化时,不需要刷新.在完成之后才刷新</param>
        /// <returns></returns>
        public static void Add(string file, bool enable = true, bool refresh = true)
        {
            if (listviewFiles.ContainsKey(file))
            {
                //如果已经在目录中,则标记为挂载
                Update(file, true, refresh);
            }
            else
            {
                var fileHookItem = new FileHookEntity(file, enable, refresh);
                listviewFiles.Add(file, fileHookItem);

                var fileMock = new FileMockEntity() { Enable = enable, Path = file };
                Files.Add(fileMock);
                htFiles.Add(file, fileMock); //添加到索引

                if (enable && refresh)
                {
                    Refresh();
                }
            }
        }

        /// <summary>
        /// 从挂载列表中删除文件或者文件夹
        /// </summary>
        /// <param name="file"></param>
        public static void Remove(string file, bool refresh = true)
        {
            if (listviewFiles.ContainsKey(file))
            {
                var fileMock = (FileMockEntity)htFiles[file];

                listviewFiles.Remove(file);
                htFiles.Remove(file);
                Files.Remove(fileMock);

                if (refresh)
                {
                    //批量删除时,最后再手工刷新
                    Refresh();
                }
            }
        }

        /// <summary>
        /// 设置挂载列表中的文件是否需要挂载
        /// </summary>
        /// <param name="file"></param>
        /// <param name="enable"></param>
        public static void Update(string file, bool enable, bool refresh = true)
        {
            if (listviewFiles.ContainsKey(file))
            {
                var item = (FileHookEntity)listviewFiles[file];
                item.Enable = enable;

                var fileMock = (FileMockEntity)htFiles[file];
                fileMock.Enable = enable;

                if (refresh)
                {
                    Refresh();
                }
            }
        }

        /// <summary>
        /// 重新计算需要挂载的文件列表
        /// <para>此方法会被重复调用,应该有性能问题,暂时想不到好的办法</para>
        /// </summary>
        public static void Refresh()
        {
            lock (fileMockList)
            {
                fileMockList.Clear(); //清理索引列表
                duplicateList.Clear(); //清理文件名重复列表
                if (listviewFiles != null)
                {
                    foreach (DictionaryEntry file in listviewFiles)
                    {
                        var subitem = (FileHookEntity)file.Value;
                        if (!subitem.Enable) continue; //没勾选的跳过

                        subitem.Refresh();

                        foreach (string path in subitem.Files)
                        {
                            if (File.Exists(path)) //不存在的话就无视了
                            {
                                var fileName = Path.GetFileName(path).ToLower(); //统一小写

                                //不存在列表再添加
                                if (!fileMockList.ContainsKey(fileName))
                                {
                                    //不在索引列表中,则添加到索引列表中
                                    fileMockList.Add(fileName, path);
                                }
                                else
                                {
                                    //如果是同一文件,跳过
                                    string oldpath = (string)fileMockList[fileName];
                                    if (oldpath == path) continue;

                                    //文件不同,添加到重复列表
                                    var list = (List<string>)duplicateList[fileName];
                                    if (list == null)
                                    {
                                        list = new List<string>();
                                        list.Add(oldpath); //把旧的文件添加到列表
                                    }

                                    list.Add(path);
                                }
                            }
                        }
                    }
                }
            }

            LogService.Log("list loaded, hooking " + fileMockList.Count + " files.");
        }
        #endregion

        #region 供服务调用的方法

        public static string GetMockFilePath(string fileName)
        {
            string path = null;
            if (fileMockList == null || fileMockList.Count == 0) return path;

            if (fileMockList.ContainsKey(fileName))
            {
                path = (string)fileMockList[fileName];
                if (!File.Exists(path))
                {
                    path = null; //不存在

                    //索引列表中的文件不存在,查询重复列表,取得一个文件名相同的并且存在的文件.
                    //这里可能会造成错误挂载
                    List<string> list = (List<string>)duplicateList[fileName];
                    foreach (var str in list)
                    {
                        if (File.Exists(str))
                        {
                            path = str;
                            break;
                        }
                    }
                }
            }

            return path;
        }

        #endregion
    }
}
