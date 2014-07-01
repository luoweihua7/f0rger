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
        public virtual bool IsEnable()
        {
            return false;
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
