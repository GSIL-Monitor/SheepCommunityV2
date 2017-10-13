using System;
using System.Web;
using ServiceStack.Logging;
using ServiceStack.Logging.EventLog;

namespace Sheep
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // 配置日志工厂。
            LogManager.LogFactory = new EventLogFactory("Sheep Community", "Application");
            // 初始化主机。
            new AppHost().Init();
        }
    }
}