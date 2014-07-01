using System;
using System.Collections.Generic;
using System.Text;

using Fiddler;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;

namespace f0rger
{
    public class FiddlerTabPage : FiddlerService
    {
        #region 控件
        private Panel pSwitch;
        private CheckBox cbShowTip;
        private CheckBox cbEnable;
        private Panel pProfile;
        private Panel pMock;
        private Panel pButton;
        private ListView lvMockFiles;
        private Button btnRefresh;
        private Button btnClear;
        private Button btnRemove;
        private Button btnAdd;
        private TabPage fiddlerTabPage;

        private OpenFileDialog openDialog;

        private MiniTip frmTip;
        #endregion

        public ConfigEntity config;

        private Hashtable fileList = new Hashtable();

        public FiddlerTabPage()
        {
            // 初始化信息(从WindowsApplication项目中copy过来的)
            this.pSwitch = new Panel();
            this.cbShowTip = new CheckBox();
            this.cbEnable = new CheckBox();
            this.pProfile = new Panel();
            this.pMock = new Panel();
            this.lvMockFiles = new ListView();
            this.pButton = new Panel();
            this.btnRefresh = new Button();
            this.btnClear = new Button();
            this.btnRemove = new Button();
            this.btnAdd = new Button();

            this.openDialog = new OpenFileDialog();

            //提示语窗口
            frmTip = new MiniTip();

            InitializeComponent();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        public void InitializeComponent()
        {
            #region 面板区域,用于自动适应大小
            // 主开关区域
            this.pSwitch.Controls.Add(this.cbShowTip);
            this.pSwitch.Controls.Add(this.cbEnable);
            this.pSwitch.Dock = DockStyle.Top;
            this.pSwitch.Location = new Point(0, 0);
            this.pSwitch.Name = "pSwitch";
            this.pSwitch.Height = 30;

            // 配置区域(未完成)
            this.pProfile.Dock = DockStyle.Top;
            this.pProfile.Location = new Point(0, 30);
            this.pProfile.Name = "pProfile";
            this.pProfile.Height = 10;
            this.pProfile.TabIndex = 1;

            // 挂载列表区域
            this.pMock.Controls.Add(this.lvMockFiles);
            this.pMock.Dock = DockStyle.Fill;
            this.pMock.Location = new Point(0, 40);
            this.pMock.Name = "pMock";
            this.pMock.Size = new Size(704, 397);
            this.pMock.TabIndex = 2;

            // 功能按钮区域
            this.pButton.Controls.Add(this.btnRefresh);
            this.pButton.Controls.Add(this.btnClear);
            this.pButton.Controls.Add(this.btnRemove);
            this.pButton.Controls.Add(this.btnAdd);
            this.pButton.Dock = DockStyle.Bottom;
            this.pButton.Location = new Point(0, 437);
            this.pButton.Name = "pButton";
            this.pButton.Size = new Size(704, 33);
            this.pButton.TabIndex = 3;
            #endregion

            #region 功能区
            // 主开关
            this.cbEnable.AutoSize = true;
            this.cbEnable.Location = new Point(3, 8);
            this.cbEnable.Name = "cbEnable";
            this.cbEnable.Size = new Size(60, 16);
            this.cbEnable.Text = "Enable";
            this.cbEnable.UseVisualStyleBackColor = true;
            this.cbEnable.CheckedChanged += ChangeEnableEventArgs;
            // 挂载提示选择框
            this.cbShowTip.AutoSize = true;
            this.cbShowTip.Location = new Point(92, 8);
            this.cbShowTip.Name = "cbShowTip";
            this.cbShowTip.Size = new Size(66, 16);
            this.cbShowTip.Text = "ShowTip";
            this.cbShowTip.UseVisualStyleBackColor = true;
            this.cbShowTip.CheckedChanged += ChangeShowTipEventArgs;
            // 挂载列表
            this.lvMockFiles.AllowDrop = true;
            this.lvMockFiles.CheckBoxes = true;
            this.lvMockFiles.Columns.AddRange(new ColumnHeader[] {
                new ColumnHeader(){Text="File",Width=150},
                new ColumnHeader(){Text="Path",Width=500}
            });
            this.lvMockFiles.Dock = DockStyle.Fill;
            this.lvMockFiles.FullRowSelect = true;
            this.lvMockFiles.GridLines = true;
            this.lvMockFiles.Location = new Point(0, 0);
            this.lvMockFiles.Name = "lvMockFiles";
            this.lvMockFiles.ShowGroups = false;
            this.lvMockFiles.View = View.Details;
            this.lvMockFiles.KeyDown += new KeyEventHandler(keyDown); //CTRL+C/V
            this.lvMockFiles.DragEnter += new DragEventHandler(listViewDropEnter);
            this.lvMockFiles.DragDrop += new DragEventHandler(listViewDragDrop); //拖进来
            this.lvMockFiles.ItemChecked += new ItemCheckedEventHandler(listViewChecked); //点击勾选

            #region 按钮
            // 添加按钮
            this.btnAdd.Location = new Point(3, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(69, 23);
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(OnClickAddButton);
            // 删除按钮
            this.btnRemove.Location = new Point(78, 6);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(69, 23);
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new EventHandler(OnClickRemoveButton);
            // 刷新按钮
            this.btnRefresh.Location = new Point(153, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(69, 23);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new EventHandler(OnClickRefreshButton);
            // 清除按钮
            this.btnClear.Location = new Point(228, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(69, 23);
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new EventHandler(OnClickClearButton);
            #endregion

            #endregion

            // 标签页卡
            this.fiddlerTabPage = new TabPage();
            this.fiddlerTabPage.Text = Configs.TabName;
            this.fiddlerTabPage.Controls.Add(this.pMock);
            this.fiddlerTabPage.Controls.Add(this.pButton);
            this.fiddlerTabPage.Controls.Add(this.pProfile);
            this.fiddlerTabPage.Controls.Add(this.pSwitch);

            //在fiddler中开辟新天地: 添加一个充满恶意的TabPage
            FiddlerApplication.UI.imglSessionIcons.Images.Add("f0rger", f0rger.Properties.Resources.icon);
            FiddlerApplication.UI.tabsViews.TabPages.Add(fiddlerTabPage);
            fiddlerTabPage.ImageKey = "f0rger"; //TabPage的图标
        }

        /// <summary>
        /// 还原控件状态
        /// <para>如设置,提示,列表等</para>
        /// </summary>
        public void RestoreControls()
        {
            this.cbEnable.Checked = config.Enable;
            this.cbShowTip.Checked = config.ShowTip;

            var files = config.Files;
            config.Files = new FileMockEntityList(); //重新初始化.
            foreach (var item in files)
            {
                //实例化每一项
                //这里对自动对config.Files添加数据,为的是跟fileList引用一致,方便移除
                AddToListView(item.Path, item.Enable);
            }
            FileManageService.RefreshMockList(); //刷新挂载列表

            ChangeEnableEventArgs(null, null); //触发一次变更事件,还原组件状态
        }

        public void Initialize()
        {
            config = ConfigService.Read(); //从配置读取,失败的话会自动new一个
            bool _enable = config.Enable;
            config.Enable = false; //先禁用,待所有数据初始化完成时再启用

            //FileManageService.Initialize(config.Files); //初始化挂载列表
            config.Enable = _enable; //数据初始化完成,还原开关配置

            RestoreControls();
        }

        #region 控件事件区域

        void ChangeEnableEventArgs(object sender, EventArgs e)
        {
            var enable = cbEnable.Checked;

            //保存
            config.Enable = enable;

            //控件的可用性
            cbShowTip.Enabled = enable;
            lvMockFiles.Enabled = enable;
            btnAdd.Enabled = enable;
            btnRemove.Enabled = enable;
            btnRefresh.Enabled = enable;
            btnClear.Enabled = enable;
        }

        void ChangeShowTipEventArgs(object sender, EventArgs e)
        {
            config.ShowTip = cbShowTip.Checked;
        }

        void keyDown(object sender, KeyEventArgs e)
        {
            if (FiddlerApplication.UI.tabsViews.SelectedTab == fiddlerTabPage)
            {
                if (e.Control && config.Enable)
                {
                    if (e.KeyCode == Keys.V) //粘贴
                    {
                        foreach (string filePath in Clipboard.GetFileDropList())
                        {
                            AddToListView(filePath); //添加到列表
                        }
                        FileManageService.RefreshMockList();

                        e.Handled = true;
                        return;
                    }
                    else if (e.KeyCode == Keys.A) //全选
                    {
                        foreach (ListViewItem item in lvMockFiles.Items)
                        {
                            item.Selected = true;
                        }

                        //不往下执行
                        e.Handled = true;
                        return;
                    }
                }

                if (e.KeyCode == Keys.Delete)
                {
                    if (lvMockFiles.SelectedItems.Count > 0)
                    {
                        foreach (ListViewItem item in lvMockFiles.SelectedItems)
                        {
                            RemoveFromListView(item, false);
                        }
                        FileManageService.RefreshMockList();
                    }
                }
            }
        }

        void listViewDropEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        void listViewDragDrop(object sender, DragEventArgs e)
        {
            string[] dropList = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filePath in dropList)
            {
                AddToListView(filePath);
            }
            FileManageService.RefreshMockList();
        }

        void listViewChecked(object sender, ItemCheckedEventArgs e)
        {
            ListViewItem lvi = e.Item;
            var enable = lvi.Checked;

            string path = lvi.SubItems[1].Text.ToLower();
            FileMockEntity fileMock = (FileMockEntity)fileList[path];
            if (fileMock != null) //初始化添加的时候也会出发checked
            {
                fileMock.Enable = enable;
                FileManageService.Update(path, enable, true);
            }
        }

        void OnClickAddButton(object sender, EventArgs e)
        {
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                string[] files = openDialog.FileNames;
                foreach (string file in files)
                {
                    AddToListView(file, false);
                }
                FileManageService.RefreshMockList();
            }
        }

        void OnClickRemoveButton(object sender, EventArgs e)
        {
            if (lvMockFiles.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lvMockFiles.SelectedItems)
                {
                    RemoveFromListView(item, false);
                }
                FileManageService.RefreshMockList();
            }
        }

        void OnClickRefreshButton(object sender, EventArgs e)
        {
            FileManageService.RefreshMockList();
        }

        void OnClickClearButton(object sender, EventArgs e)
        {
            if (lvMockFiles.Items.Count > 0)
            {
                foreach (ListViewItem lvi in lvMockFiles.Items)
                {
                    RemoveFromListView(lvi, false);
                }
                FileManageService.RefreshMockList();
            }
        }

        /// <summary>
        /// 添加到列表
        /// <para>包括ListView列表,fileList列表,FileManagerService列表中</para>
        /// <para>但是FileManagerService不刷新,为了循环调用此方法导致重复刷新</para>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="enable"></param>
        void AddToListView(string file, bool enable = true)
        {
            string str = file.ToLower();

            if (fileList[str] != null)
            {
                foreach (ListViewItem item in lvMockFiles.Items)
                {
                    //如果已经在列表中,则标记为勾选
                    if (item.SubItems[1].Text == str)
                    {
                        item.Checked = true;
                        break;
                    }
                }

                FileMockEntity fileMock = (FileMockEntity)fileList[str];
                fileMock.Enable = true; //引用类型
                FileManageService.Update(str, true, false);
            }
            else
            {
                FileManageService.Add(str, true, false);

                ListViewItem lvi = new ListViewItem();
                if (Directory.Exists(file))
                {
                    // 文件夹
                    lvi.Text = "[[Folder]]";
                }
                else
                {
                    // 文件
                    lvi.Text = Path.GetFileName(file).ToLower();
                }

                lvi.Checked = enable;
                lvi.SubItems.Add(file.ToLower());
                lvMockFiles.Items.Add(lvi);

                var fileMock = new FileMockEntity() { Enable = enable, Path = str };
                config.Files.Add(fileMock);
                fileList.Add(str, fileMock); //添加到维护列表
            }
        }

        void RemoveFromListView(ListViewItem lvi, bool refresh = true)
        {
            var filePath = lvi.SubItems[1].Text.ToLower();
            FileManageService.Remove(filePath, false);
            config.Files.Remove((FileMockEntity)fileList[filePath]);
            fileList.Remove(filePath);
            lvMockFiles.Items.Remove(lvi);

            if (refresh)
            {
                FileManageService.RefreshMockList();
            }
        }

        #endregion

        #region 重载

        public override bool IsEnable()
        {
            if (config != null)
            {
                return config.Enable;
            }
            else
            {
                return false;
            }
        }

        public override void OnLoad()
        {
            //初始化数据,设置等
            Initialize();
        }

        public override void OnBeforeUnload()
        {
            //退出时先保存配置
            ConfigService.Save(config);
        }

        public override void OnMatchSession(string fileName)
        {
            //弹窗
            if (config.ShowTip)
            {
                frmTip.Show(fileName);
            }
        }
        #endregion
    }
}
