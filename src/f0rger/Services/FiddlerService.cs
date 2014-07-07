using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace f0rger
{
    /// <summary>
    /// 插件核心类
    /// <para>设计目的:一个dll中包含多个功能时,可以分别继承</para>
    /// </summary>
    public abstract class FiddlerService : IAutoTamper, IFiddlerExtension
    {
        #region 虚函数,作为开关和逻辑处理

        private static string limitSpeed { get; set; }

        /// <summary>
        /// 获取设定的限速值
        /// <para>计算并返回每KB需要多少毫秒,最少为1ms</para>
        /// </summary>
        /// <returns></returns>
        public void SetSpeedLimitValue()
        {
            //存在目的: 每次调整速率之后设置到静态量中,可以节省每次计算的损耗
            int speed = Configs.LimitSpeed;
            if (speed <= 0) speed = 1; //容错
            limitSpeed = Math.Ceiling((double)(1000 / speed)).ToString(); //计算每KB需要多少ms
        }

        /// <summary>
        /// 挂载命中处理后的逻辑
        /// <para>如弹窗提示挂载之类的</para>
        /// </summary>
        /// <param name="fileName">命中的文件名</param>
        public virtual void OnMatchSession(string fileName)
        {

        }

        #endregion

        #region Fiddler接口实现
        public virtual void AutoTamperRequestAfter(Session oSession)
        {

        }

        public virtual void AutoTamperRequestBefore(Session oSession)
        {
            if (Configs.Enable)
            {
                if (Configs.EnableLimit && !string.IsNullOrEmpty(limitSpeed))
                {
                    // Delay sends by 300ms per KB uploaded.
                    oSession["request-trickle-delay"] = "300";
                    // Delay receives by 150ms per KB downloaded.
                    oSession["response-trickle-delay"] = limitSpeed;
                }

                if (Configs.EnableProfile && !ProfileService.MatchHost(oSession.host))
                {
                    //enable profile and no profile match,
                    //do nothing;
                    return;
                }

                string filePath = new System.Uri("http://127.0.0.1" + oSession.url).AbsolutePath; //福拉的方法,好像比正则要快
                string fileName = Path.GetFileName(filePath).ToLower();  // file.ext

                string path = FileMockService.GetMockFilePath(fileName); //哈希表匹配文件名
                if (!string.IsNullOrEmpty(path))
                {
                    //标记颜色
                    oSession["ui-color"] = Configs.Color;
                    oSession["ui-backcolor"] = Configs.BgColor;

                    oSession.utilCreateResponseAndBypassServer();
                    oSession.LoadResponseFromFile(path);

                    //命中文件
                    OnMatchSession(fileName);
                }
            }
        }

        public virtual void AutoTamperResponseAfter(Session oSession)
        {

        }

        public virtual void AutoTamperResponseBefore(Session oSession)
        {

        }

        public virtual void OnBeforeReturningError(Session oSession)
        {

        }

        public virtual void OnBeforeUnload()
        {

        }

        public virtual void OnLoad()
        {

        }
        #endregion
    }
}
