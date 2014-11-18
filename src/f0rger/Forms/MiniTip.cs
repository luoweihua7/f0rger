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
        static int infinite = System.Threading.Timeout.Infinite; //只一次,不重复
        static bool isMouseEnter = false; //鼠标是否在窗口内,如果在窗口内,定时器停止计时
        static int iTop = 0;

        public MiniTip()
        {
            InitializeComponent();

            //鼠标飘入,就暂时不隐藏了
            this.listBox.MouseEnter += OnMouseEnter;
            this.listBox.MouseLeave += OnMouseLeave;

            //显示区域
            //说明一下:ListBox比较变态,高度必须是ItemHeight的整数倍,然后加上Border之类,否则会自动缩减高度
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            this.Location = new Point(workingArea.Width - this.Width, workingArea.Height - this.Height);

            //可见的项 数量
            iTop = (int)(this.listBox.Height / this.listBox.ItemHeight);

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
        public void Show(string fileName)
        {
            this.listBox.Items.Add(fileName);
            //滚到最下面
            this.listBox.TopIndex = this.listBox.Items.Count - iTop;

            this.Show();
            TimeTick(isMouseEnter);
        }

        private void Hide(object state)
        {
            //只隐藏,不关闭
            this.listBox.Items.Clear();
            this.Hide();
        }

        /// <summary>
        /// 启动定时器
        /// </summary>
        /// <param name="isInfinite"></param>
        private void TimeTick(bool isInfinite = false)
        {
            var due = Configs.dueTime;
            if (isInfinite) due = infinite;
            timer.Change(due, infinite);
        }

        void OnMouseEnter(object sender, EventArgs e)
        {
            //鼠标移入到窗口内的时候.计时器停止,方便看列表信息
            isMouseEnter = true;
            TimeTick(isMouseEnter);
        }

        void OnMouseLeave(object sender, EventArgs e)
        {
            isMouseEnter = false;
            TimeTick();
        }
    }
}
