using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace f0rger
{
    /// <summary>
    /// 控件控制层
    /// <para>从基础的Form中抽离出来管理界面内容显示,让Form中的代码更干净</para>
    /// </summary>
    public class ControlsService
    {
        private static ListView listView;
        private static ToolStrip toolStrip;

        public static void Use(ListView lv, ToolStrip ts)
        {
            listView = lv;
            toolStrip = ts;
        }


    }
}
