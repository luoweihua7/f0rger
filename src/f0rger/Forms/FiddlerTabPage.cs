using System;
using System.Collections.Generic;
using System.Text;

using Fiddler;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using f0rger.Properties;

namespace f0rger
{
    public class FiddlerTabPage : FiddlerService
    {
        #region 控件
        private MiniTip miniTip = new MiniTip();
        private TabPage tabPage;
        private ToolStrip toolBar;
        private CheckBox cbEnable;
        private CheckBox cbShowTip;
        private CheckBox cbProfile;
        private CheckBox cbSpeedLimit;
        private CheckBox cbStrictMode;
        private ListView listView;
        private List<Control> controls;
        private OpenFileDialog dialog = new OpenFileDialog();
        #endregion

        public void InitializeComponent()
        {
            #region 实例化控件

            TabPage tp = new TabPage() { Text = Configs.AppName };

            Panel pHead = new Panel() { Dock = DockStyle.Top, Location = new Point(0, 0), Height = 30, TabIndex = 0 };
            Panel pTool = new Panel() { Dock = DockStyle.Top, Location = new Point(0, 30), Height = 25 };
            Panel pMain = new Panel() { Dock = DockStyle.Fill, Location = new Point(0, 55) };
            Panel pBottom = new Panel() { Dock = DockStyle.Bottom, Height = 35 };

            CheckBox cbEnable = new CheckBox() { Text = "Enable", AutoSize = true, Location = new Point(3, 8), UseVisualStyleBackColor = true };
            CheckBox cbShowTip = new CheckBox() { Text = "Show Tips", AutoSize = true, Location = new Point(160, 8), UseVisualStyleBackColor = true };
            CheckBox cbProfile = new CheckBox() { Text = "Profiles", AutoSize = true, Location = new Point(250, 8), UseVisualStyleBackColor = true };
            CheckBox cbStrictMode = new CheckBox() { Text = "Strict Mode", AutoSize = true, Location = new Point(320, 8), UseVisualStyleBackColor = true };
            CheckBox cbSpeedLimit = new CheckBox() { Text = "Speed Limit", AutoSize = true, Location = new Point(410, 8), UseVisualStyleBackColor = true };

            ToolStrip toolStrip = new ToolStrip() { Location = new Point(0, 0), RenderMode = ToolStripRenderMode.System, Height = 25 };
            ToolStripButton tsbMgr = new ToolStripButton() { DisplayStyle = ToolStripItemDisplayStyle.Image, ImageTransparentColor = Color.Magenta, Image = Resources.edit };

            ListView lv = new ListView() { AllowDrop = true, CheckBoxes = true, Dock = DockStyle.Fill, FullRowSelect = true, GridLines = true, Location = new Point(0, 0), View = View.Details };

            Button btnAdd = new Button() { Location = new Point(3, 6), Size = new Size(69, 23), Text = "Add", UseVisualStyleBackColor = true };
            Button btnRemove = new Button() { Location = new Point(78, 6), Size = new Size(69, 23), Text = "Remove", UseVisualStyleBackColor = true };
            Button btnRefresh = new Button() { Location = new Point(153, 6), Size = new Size(69, 23), Text = "Refresh", UseVisualStyleBackColor = true };
            Button btnClear = new Button() { Location = new Point(228, 6), Size = new Size(69, 23), Text = "Clear", UseVisualStyleBackColor = true };

            #endregion

            #region 控件层级添加

            tp.SuspendLayout();
            tp.Controls.AddRange(new Control[] { pMain, pBottom, pTool, pHead }); //添加顺序为:Dock>Bottom>Top,有2个Top,则最顶上的最后加
            pHead.Controls.AddRange(new Control[] { cbEnable, cbShowTip, cbProfile, cbStrictMode, cbSpeedLimit });
            toolStrip.Items.AddRange(new ToolStripItem[] { tsbMgr, new ToolStripSeparator() { Size = new System.Drawing.Size(6, 25) } });
            pTool.Controls.Add(toolStrip);
            lv.Columns.AddRange(new ColumnHeader[] { new ColumnHeader() { Text = "File", Width = 150 }, new ColumnHeader() { Text = "Path", Width = 380 } });
            pMain.Controls.Add(lv);
            pBottom.Controls.AddRange(new Control[] { btnAdd, btnRemove, btnRefresh, btnClear });
            tp.ResumeLayout(false);

            #endregion

            #region 控件事件

            cbEnable.CheckedChanged += new EventHandler(ChangeEnableEventArgs);
            cbShowTip.CheckedChanged += new EventHandler(ChangeShowTipEventArgs);
            cbProfile.CheckedChanged += new EventHandler(ChangeProfileEventArgs);
            cbStrictMode.CheckedChanged += new EventHandler(ChangeStrictModeEventArgs);
            cbSpeedLimit.CheckedChanged += new EventHandler(ChangeSpeedLimitEventArgs);

            tsbMgr.Click += new EventHandler(OnManageProfileClick);

            lv.KeyDown += new KeyEventHandler(OnListViewKeyDown); //CTRL+C/V
            lv.DragEnter += new DragEventHandler(OnListViewDropEnter);
            lv.DragDrop += new DragEventHandler(OnListViewDragDrop); //拖进来
            lv.ItemChecked += new ItemCheckedEventHandler(OnListViewChecked); //点击勾选

            btnAdd.Click += new EventHandler(OnClickAddButton);
            btnRemove.Click += new EventHandler(OnClickRemoveButton);
            btnRefresh.Click += new EventHandler(OnClickRefreshButton);
            btnClear.Click += new EventHandler(OnClickClearButton);

            #endregion

            //保存到全局,方便调用
            this.controls = new List<Control>() { cbShowTip, cbProfile, cbStrictMode, cbSpeedLimit, toolStrip, lv, btnAdd, btnRemove, btnRemove, btnRefresh, btnClear };
            this.tabPage = tp;
            this.cbEnable = cbEnable;
            this.cbShowTip = cbShowTip;
            this.cbProfile = cbProfile;
            this.cbStrictMode = cbStrictMode;
            this.cbSpeedLimit = cbSpeedLimit;
            this.toolBar = toolStrip;
            this.listView = lv;

            //关联
            ListViewController.listView = listView;
            ToolStripController.toolbar = toolStrip;

            //加入到Fiddler中
            FiddlerApplication.UI.imglSessionIcons.Images.Add(Configs.AppName, f0rger.Properties.Resources.icon);
            FiddlerApplication.UI.tabsViews.TabPages.Add(this.tabPage);
            this.tabPage.ImageKey = Configs.AppName;

            cbStrictMode.Visible = false;
            cbSpeedLimit.Visible = false;
        }

        /// <summary>
        /// restore from config saved
        /// </summary>
        public void Restore()
        {
            //load configs
            ConfigEntity config = ConfigService.Read();

            //initialize profiles and files
            ToolStripController.Add(config.Profiles);
            ListViewController.Add(config.Files);

            //restore switches
            this.cbShowTip.Checked = config.EnableTip;
            this.cbProfile.Checked = config.EnableProfile;
            this.cbSpeedLimit.Checked = config.EnableLimit;
            this.cbStrictMode.Checked = config.StrictMode;
            this.cbEnable.Checked = config.Enable; //last set, fire checked change event

            if (!config.EnableProfile)
            {
                //fire event when disabled
                ChangeProfileEventArgs(this.cbProfile, null);
            }

            if (!config.Enable)
            {
                //fire event when disabled
                ChangeEnableEventArgs(this.cbEnable, null);
            }
        }

        #region 控件事件实现

        void ChangeEnableEventArgs(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            var enable = cb.Checked;
            foreach (Control control in controls)
            {
                control.Enabled = enable;
            }
            Configs.Enable = enable;
        }
        void ChangeShowTipEventArgs(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Configs.EnableTip = cb.Checked;
        }
        void ChangeProfileEventArgs(object sender, EventArgs e)
        {
            var enable = ((CheckBox)sender).Checked;
            ToolStripController.Set(enable);
            Configs.EnableProfile = enable;
        }
        void ChangeStrictModeEventArgs(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Configs.StrictMode = cb.Checked;
        }
        void ChangeSpeedLimitEventArgs(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Configs.EnableLimit = cb.Checked;
        }

        void OnManageProfileClick(object sender, EventArgs e)
        {
            var frm = new ProfileMgr();
            frm.ShowDialog();
        }

        void OnListViewKeyDown(object sender, KeyEventArgs e)
        {
            if (FiddlerApplication.UI.tabsViews.SelectedTab == this.tabPage)
            {
                if (e.Control && Configs.Enable)
                {
                    if (e.KeyCode == Keys.V) //paste
                    {
                        List<string> list = new List<string>();
                        foreach (string file in Clipboard.GetFileDropList())
                        {
                            list.Add(file);
                        }
                        ListViewController.Add(list);

                        e.Handled = true;
                        return;
                    }
                    else if (e.KeyCode == Keys.A) //select all
                    {
                        foreach (ListViewItem item in this.listView.Items)
                        {
                            item.Selected = true;
                        }

                        e.Handled = true;
                        return;
                    }
                }

                if (e.KeyCode == Keys.Delete) //remove selected items
                {
                    if (this.listView.SelectedItems.Count > 0)
                    {
                        ListViewController.Remove();
                    }
                }
            }
        }
        void OnListViewDropEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        void OnListViewDragDrop(object sender, DragEventArgs e)
        {
            string[] dropList = (string[])e.Data.GetData(DataFormats.FileDrop);
            List<string> list = new List<string>();
            foreach (string file in dropList)
            {
                list.Add(file);
            }
            ListViewController.Add(list);
        }
        void OnListViewChecked(object sender, ItemCheckedEventArgs e)
        {
            var enable = e.Item.Checked;
            var file = e.Item.SubItems[1].Text;
            ListViewController.Update(file, enable);
        }

        void OnClickAddButton(object sender, EventArgs e)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                List<string> list = new List<string>();
                foreach (string file in dialog.FileNames)
                {
                    list.Add(file);
                }
                ListViewController.Add(list);
            }
        }
        void OnClickRemoveButton(object sender, EventArgs e)
        {
            ListViewController.Remove();
        }
        void OnClickRefreshButton(object sender, EventArgs e)
        {
            ListViewController.Refresh();
        }
        void OnClickClearButton(object sender, EventArgs e)
        {
            ListViewController.Clear();
        }

        #endregion

        #region 功能实现

        private void Initialize()
        {
            this.InitializeComponent();
            this.Restore();
        }

        #endregion

        #region 重载,实现Fiddler功能

        public override void OnLoad()
        {
            Initialize(); //功能入口
        }

        public override void OnBeforeUnload()
        {
            ConfigService.Save();
        }

        public override void OnMatchSession(string fileName)
        {
            if (Configs.EnableTip)
            {
                miniTip.Show(fileName);
            }
        }

        #endregion
    }
}
