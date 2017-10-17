using System;
using System.Web;
using ServiceStack.Logging;
using ServiceStack.Logging.EventLog;

namespace Sheep
{
    public class Global : HttpApplication
    {
        #region 常量

        public const string EventLogName = "Sheep Community V2";
        public const string EventLogSource = "Application";

        #endregion

        #region 应用程序事件处理

        protected void Application_Start(object sender, EventArgs e)
        {
            // 配置日志工厂。
            LogManager.LogFactory = new EventLogFactory(EventLogName, EventLogSource);
            // 初始化主机。
            new AppHost().Init();
        }

        #endregion
    }
}