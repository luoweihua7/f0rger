using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;

namespace f0rger
{
    public partial class MiniTip : Form
    {
        private System.Threading.Timer timer;
        static int dueTime = 1500; //延迟隐藏窗口的时间
        static int infinite = System.Threading.Timeout.Infinite; //只一次,不重复
        static bool isMouseEnter = false; //鼠标是否在窗口内,如果在窗口内,定时器停止计时

        public MiniTip()
        {
            InitializeComponent();

            //显示区域
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            this.Location = new Point(workingArea.Width - this.Width, workingArea.Height - this.Height);
            //初始化定时器
            timer = new System.Threading.Timer(new TimerCallback(Hide), null, infinite, infinite);
            //不显示窗体
            this.Hide();
        }

        /// <summary>
        /// 添加要提示的文件
        /// <para>通过添加文件来触发提示窗口</para>
        /// </summary>
        /// <param name="string">文件名称</param>
        public void Add(string fileName)
        {
            listBox.Items.Add(fileName);
            this.Show();
            TimeTick(isMouseEnter);
        }

        private void Hide(object state)
        {
            //只隐藏,不关闭
            listBox.Items.Clear();
            this.Hide();
        }

        /// <summary>
        /// 启动定时器
        /// </summary>
        /// <param name="isInfinite"></param>
        private void TimeTick(bool isInfinite=false)
        {
            var due = dueTime;
            if (isInfinite) due = infinite;
            timer.Change(due, infinite);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            //鼠标移入到窗口内的时候.计时器停止,方便看列表信息
            isMouseEnter = true;
            TimeTick(true);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            isMouseEnter = false;
            TimeTick();
        }
    }
}
