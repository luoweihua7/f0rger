using System;
using System.Collections.Generic;
using System.Text;

using Fiddler;
using System.Windows.Forms;
using System.Drawing;

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
            //
            // 初始化信息(从WindowsApplication项目中copy过来的)
            //
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
            // 
            // pSwitch
            // 
            this.pSwitch.Controls.Add(this.cbShowTip);
            this.pSwitch.Controls.Add(this.cbEnable);
            this.pSwitch.Dock = DockStyle.Top;
            this.pSwitch.Location = new Point(0, 0);
            this.pSwitch.Name = "pSwitch";
            this.pSwitch.Height = 30;
            // 
            // cbShowTip
            // 
            this.cbShowTip.AutoSize = true;
            this.cbShowTip.Location = new Point(92, 8);
            this.cbShowTip.Name = "cbShowTip";
            this.cbShowTip.Size = new Size(66, 16);
            this.cbShowTip.Text = "ShowTip";
            this.cbShowTip.UseVisualStyleBackColor = true;
            // 
            // cbEnable
            // 
            this.cbEnable.AutoSize = true;
            this.cbEnable.Location = new Point(3, 8);
            this.cbEnable.Name = "cbEnable";
            this.cbEnable.Size = new Size(60, 16);
            this.cbEnable.Text = "Enable";
            this.cbEnable.UseVisualStyleBackColor = true;
            // 
            // pProfile
            // 
            this.pProfile.Dock = DockStyle.Top;
            this.pProfile.Location = new Point(0, 30);
            this.pProfile.Name = "pProfile";
            this.pProfile.Size = new Size(704, 10);
            this.pProfile.TabIndex = 1;
            // 
            // pMock
            // 
            this.pMock.Controls.Add(this.lvMockFiles);
            this.pMock.Dock = DockStyle.Fill;
            this.pMock.Location = new Point(0, 40);
            this.pMock.Name = "pMock";
            this.pMock.Size = new Size(704, 397);
            this.pMock.TabIndex = 2;
            // 
            // lvMockFiles
            // 
            this.lvMockFiles.AllowDrop = true;
            this.lvMockFiles.CheckBoxes = true;
            this.lvMockFiles.Columns.AddRange(new ColumnHeader[] {
                new ColumnHeader(){Text="File",Width=150},
                new ColumnHeader(){Text="Path",Width=300},
                new ColumnHeader(){Text="Status",Width=50}
            });
            this.lvMockFiles.Dock = DockStyle.Fill;
            this.lvMockFiles.ForeColor = SystemColors.WindowFrame;
            this.lvMockFiles.FullRowSelect = true;
            this.lvMockFiles.GridLines = true;
            this.lvMockFiles.Location = new Point(0, 0);
            this.lvMockFiles.Name = "lvMockFiles";
            this.lvMockFiles.ShowGroups = false;
            this.lvMockFiles.View = View.Details;

            // 
            // pButton
            // 
            this.pButton.Controls.Add(this.btnRefresh);
            this.pButton.Controls.Add(this.btnClear);
            this.pButton.Controls.Add(this.btnRemove);
            this.pButton.Controls.Add(this.btnAdd);
            this.pButton.Dock = DockStyle.Bottom;
            this.pButton.Location = new Point(0, 437);
            this.pButton.Name = "pButton";
            this.pButton.Size = new Size(704, 33);
            this.pButton.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new Point(153, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(69, 23);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new Point(228, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(69, 23);
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new Point(78, 6);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(69, 23);
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new Point(3, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(69, 23);
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // TabPage
            // 
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

        public void Initialize()
        {
            config = ConfigService.Read(); //从配置读取,失败的话会自动new一个

            FileManageService.Initialize(config.Files); //初始化挂载列表
        }

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
            if (config.EnableTip)
            {
                frmTip.Show(fileName);
            }
        }
        #endregion
    }
}
