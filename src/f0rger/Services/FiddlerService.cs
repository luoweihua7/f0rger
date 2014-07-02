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
        /// <summary>
        /// 是否启用插件功能.主开关
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEnable()
        {
            return false;
        }

        /// <summary>
        /// 是否限速
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSpeedLimit()
        {
            return false;
        }

        /// <summary>
        /// 每秒限速值
        /// <para>单位为KB</para>
        /// <para></para>
        /// </summary>
        /// <returns></returns>
        public virtual int GetSpeedLimitPerSecond()
        {
            return 1024000; //千兆
        }

        /// <summary>
        /// 获取设定的限速值
        /// <para>计算并返回每KB需要多少毫秒,最少为1ms</para>
        /// </summary>
        /// <returns></returns>
        private string GetSpeedLimitValue()
        {
            int speed = GetSpeedLimitPerSecond();
            if (speed <= 0) speed = 1; //容错
            return Math.Ceiling((double)(1000 / speed)).ToString(); //计算每KB需要多少ms
        }

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
            if (IsEnable())
            {
                if (IsSpeedLimit())
                {
                    string speed = GetSpeedLimitValue();
                    // Delay sends by 300ms per KB uploaded.
                    oSession["request-trickle-delay"] = "300";
                    // Delay receives by 150ms per KB downloaded.
                    oSession["response-trickle-delay"] = speed;
                }

                string filePath = new System.Uri("http://127.0.0.1" + oSession.url).AbsolutePath;
                string fileName = Path.GetFileName(filePath).ToLower();  // file.ext

                string path = FileManageService.GetMockFilePath(fileName);
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
