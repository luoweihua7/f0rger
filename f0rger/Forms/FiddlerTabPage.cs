using System;
using System.Collections.Generic;
using System.Text;

using Fiddler;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace f0rger
{
    public class FiddlerTabPage : FileMockService
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

        private MiniTip frmTip;
        #endregion

        public new ConfigEntity config;

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
            this.lvMockFiles.ForeColor = SystemColors.WindowFrame;
            this.lvMockFiles.FullRowSelect = true;
            this.lvMockFiles.GridLines = true;
            this.lvMockFiles.Location = new Point(0, 0);
            this.lvMockFiles.Name = "lvMockFiles";
            this.lvMockFiles.ShowGroups = false;
            this.lvMockFiles.View = View.Details;
            this.lvMockFiles.KeyDown += new KeyEventHandler(keyDown);
            //this.lvMockFiles.MouseDown += new MouseEventHandler(listViewMouseDown);
            //this.lvMockFiles.DragEnter += new DragEventHandler(listViewDropEnter);
            //this.lvMockFiles.DragDrop += new DragEventHandler(listViewDragDrop);
            //this.lvMockFiles.MouseClick += new MouseEventHandler(listViewMouseClick);
            //this.lvMockFiles.ItemChecked += new ItemCheckedEventHandler(listViewChecked);

            #region 按钮
            // 添加按钮
            this.btnAdd.Location = new Point(3, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(69, 23);
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 删除按钮
            this.btnRemove.Location = new Point(78, 6);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(69, 23);
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 刷新按钮
            this.btnRefresh.Location = new Point(153, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(69, 23);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 清除按钮
            this.btnClear.Location = new Point(228, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(69, 23);
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
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

            foreach (var item in config.Files)
            {
                //实例化每一项
                AddToListView(item.Path, item.Enable);
            }

            ChangeEnableEventArgs(null, null); //触发一次变更事件,还原组件状态
        }

        public void Initialize()
        {
            config = ConfigService.Read(); //从配置读取,失败的话会自动new一个
            bool _enable = config.Enable;
            config.Enable = false; //先禁用,待所有数据初始化完成时再启用

            //TODO,方法有bug
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
                    if (e.KeyCode == Keys.V)
                    {
                        foreach (string filePath in Clipboard.GetFileDropList())
                        {
                            //TODO 修改到正常模式
                            if (true || FileManageService.Add(filePath, true, false))
                            {
                                AddToListView(filePath); //添加到列表

                                config.Files.Add(new FileMockEntity()
                                {
                                    Enable = true,
                                    Path = filePath
                                });
                            }
                        }

                        //不往下执行
                        e.Handled = true;
                        return;
                    }
                    else if (e.KeyCode == Keys.A)
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
                    foreach (ListViewItem item in lvMockFiles.SelectedItems)
                    {
                        //FileManageService.Remove(item.SubItems[1].Text, false);
                        lvMockFiles.Items.Remove(item);
                    }
                    //FileManageService.RefreshMockList();
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
                if (Directory.Exists(filePath)) continue; //忽略文件夹
                //handlerAdd(filePath.ToLower());
            }
        }
        void listViewMouseClick(object sender, EventArgs e)
        {
            if (lvMockFiles.SelectedItems.Count == 1)
            {
                ListViewItem lvi = lvMockFiles.SelectedItems[0];
                string fileName = Path.GetFileName(lvi.Text).ToLower();
                //handlerReplace(fileName);
            }
            else  //此处不会出现Count=0的情况，但会在按住Ctrl键点击ListViewItem时，Count>1的情况
            {
                //handlerRemove(string.Empty);
            }
        }
        void listViewMouseDown(object sender, MouseEventArgs e)
        {
            if (lvMockFiles.HitTest(e.X, e.Y).Item == null)
            {
                //handlerReplace(string.Empty); //在ListView空白处点击时，清空Fiddler2的选中
            }

            //此处是为了双击时不触发listView的CheckBox状态改变
            if (e.Clicks > 1)
            {
                ListViewItem lvi = lvMockFiles.GetItemAt(e.X, e.Y);
                if (null != lvi)
                {
                    lvi.Checked = !lvi.Checked;
                }
            }
        }
        void listViewChecked(object sender, ItemCheckedEventArgs e)
        {
            ListViewItem lvi = e.Item;

        }

        void AddToListView(string file, bool enable = true)
        {
            ListViewItem lvi = new ListViewItem();
            if (Directory.Exists(file))
            {
                // 文件夹
                lvi.Text = "[[Folder]]";
            }
            else
            {
                // 文件
                lvi.Text = Path.GetFileName(file);
            }

            lvi.Checked = enable;
            lvi.SubItems.Add(file);
            lvMockFiles.Items.Add(lvi);
        }

        #endregion

        #region 重载

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
            if (config.ShowTip)
            {
                frmTip.Show(fileName);
            }
        }
        #endregion
    }
}
