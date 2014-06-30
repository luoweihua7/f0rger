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
    public abstract class CoreService : IAutoTamper, IFiddlerExtension
    {
        public bool Enable = false;

        #region 虚函数,作为开关和逻辑处理
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="session"></param>
        public virtual void HandlerRequest(Session session, string fileName = null)
        {

        }

        /// <summary>
        /// 处理响应
        /// </summary>
        /// <param name="session"></param>
        public virtual void HandlerResponse(Session session, string fileName = null)
        {

        }
        #endregion

        #region Fiddler接口实现
        public virtual void AutoTamperRequestAfter(Session oSession)
        {

        }

        public virtual void AutoTamperRequestBefore(Session oSession)
        {
            if (Enable)
            {
                string path = new System.Uri("http://127.0.0.1" + oSession.url).AbsolutePath;
                string file = Path.GetFileName(path).ToLower();  // file.ext

                HandlerResponse(oSession, file);
            }
        }

        public virtual void AutoTamperResponseAfter(Session oSession)
        {
            ////暂时不需要处理响应体
            //if (Enable)
            //{
            //    string path = new System.Uri("http://127.0.0.1" + oSession.url).AbsolutePath;
            //    string file = Path.GetFileName(path);  // file.ext

            //    HandlerResponse(oSession, file);
            //}
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
