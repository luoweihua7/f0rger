using System;
using System.Collections.Generic;
using System.Text;

namespace f0rger
{
    public class FileMockService : CoreService
    {
        public new bool Enable
        {
            get
            {
                return config.Enable;
            }
            set
            {
                config.Enable = value;
            }
        }

        protected ConfigEntity config;

        public virtual void OnMatchSession(string fileName)
        {
            //这个主要是用来显示右下角提示的
        }

        public override void HandlerRequest(Fiddler.Session session, string fileName = null)
        {
            string path = FileManageService.GetMockFilePath(fileName);
            if (!String.IsNullOrEmpty(path))
            {
                session.utilCreateResponseAndBypassServer();
                session.LoadRequestBodyFromFile(path);

                //命中文件
                OnMatchSession(fileName);
            }
        }
    }
}
