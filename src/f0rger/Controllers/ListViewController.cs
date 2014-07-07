using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace f0rger
{
    public class ListViewController
    {
        public static ListView listView = new ListView();

        private static Hashtable htItems = new Hashtable();

        #region ListView Create/Remove/Update
        /// <summary>
        /// Add file to ListView
        /// </summary>
        /// <param name="file">file/directory path</param>
        public static void Add(string file)
        {
            List<string> list = new List<string>() { file };
            Add(list);
        }

        /// <summary>
        /// Add file to ListView
        /// <para>for file add</para>
        /// </summary>
        /// <param name="list">file list</param>
        public static void Add(List<string> list)
        {
            listView.SuspendLayout();
            foreach (string file in list)
            {
                if (htItems.ContainsKey(file))
                {
                    //enable when contain
                    ListViewItem lvi = (ListViewItem)htItems[file];
                    lvi.Checked = true;

                    FileMockService.Update(file, true, false);
                }
                else
                {
                    //add to list
                    var fileName = Configs.FolderName;
                    if (File.Exists(file))
                    {
                        fileName = Path.GetFileName(file);
                    }
                    ListViewItem lvi = new ListViewItem()
                    {
                        Text = fileName,
                        Checked = true
                    };
                    lvi.SubItems.Add(file);

                    listView.Items.Add(lvi);
                    htItems.Add(file, lvi);

                    FileMockService.Add(file, true, false);
                }
            }
            listView.ResumeLayout();

            FileMockService.Refresh();
        }

        /// <summary>
        /// Add file to ListView
        /// <para>for initialize</para>
        /// </summary>
        /// <param name="list">mock file list</param>
        public static void Add(FileMockEntityList list)
        {
            listView.SuspendLayout();
            foreach (FileMockEntity entity in list)
            {
                string file = entity.Path;
                if (htItems.ContainsKey(file))
                {
                    //enable when contain
                    ListViewItem lvi = (ListViewItem)htItems[file];
                    lvi.Checked = entity.Enable;

                    FileMockService.Update(file, true, false);
                }
                else
                {
                    //add to list
                    var fileName = Configs.FolderName;
                    if (File.Exists(file))
                    {
                        fileName = Path.GetFileName(file);
                    }
                    ListViewItem lvi = new ListViewItem()
                    {
                        Text = fileName,
                        Checked = entity.Enable
                    };
                    lvi.SubItems.Add(file);

                    listView.Items.Add(lvi);
                    htItems.Add(file, lvi);

                    FileMockService.Add(file, true, false);
                }
            }
            listView.ResumeLayout();

            FileMockService.Refresh();
        }

        /// <summary>
        /// Update checked list
        /// </summary>
        /// <param name="file">checked file/directory path</param>
        /// <param name="enable">enable</param>
        public static void Update(string file, bool enable)
        {
            if (htItems.ContainsKey(file))
            {
                FileMockService.Update(file, enable); //update list
            }
        }

        /// <summary>
        /// Remove selected items
        /// </summary>
        public static void Remove()
        {
            foreach (ListViewItem lvi in listView.SelectedItems)
            {
                string file = lvi.SubItems[1].Text;
                FileMockService.Remove(file, false);
                htItems.Remove(file);
                listView.Items.Remove(lvi);
            }
            FileMockService.Refresh();

        }

        /// <summary>
        /// Clean Up
        /// </summary>
        public static void Clear()
        {
            foreach (ListViewItem lvi in listView.Items)
            {
                string file = lvi.SubItems[1].Text;
                FileMockService.Remove(file, false);
                htItems.Remove(file);
                listView.Items.Remove(lvi);
            }
            FileMockService.Refresh();
        }

        public static void Refresh()
        {
            FileMockService.Refresh();
        }

        #endregion
    }
}
