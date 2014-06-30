using System;
using System.Collections.Generic;
using System.Text;

using Fiddler;
using System.Windows.Forms;

namespace f0rger
{
    public class FiddlerTabPage : FileMockService
    {
        public new ConfigEntity config;

        public FiddlerTabPage()
        {
            config = ConfigService.Read(); //从配置读取,失败的话会自动new一个

            FileManageService.Initialize(config.Files); //初始化挂载列表


            
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        public override void InitializeComponent()
        {
            FiddlerApplication.UI.tabsViews.TabPages.Add(new TabPage("f0rger"));
        }
    }
}
