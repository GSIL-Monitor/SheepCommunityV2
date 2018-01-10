using ServiceStack.Logging;
using ServiceStack.Logging.EventLog;
using Topshelf;

namespace Sheep.Job
{
    internal class Program
    {
        #region 常量

        public const string EventLogName = "Sheep Community V2 Job Service";
        public const string EventLogSource = "Application";
        public const string Endpoint = "http://localhost:54321/";

        #endregion

        public static void Main(string[] args)
        {
            // 配置日志工厂。
            LogManager.LogFactory = new EventLogFactory(EventLogName, EventLogSource);
            HostFactory.Run(x =>
                            {
                                x.Service<AppHost>(configurator =>
                                                   {
                                                       configurator.ConstructUsing(settings => new AppHost());
                                                       configurator.WhenStarted(service =>
                                                                                {
                                                                                    service.Init();
                                                                                    service.Start(Endpoint);
                                                                                });
                                                       configurator.WhenStopped(service => service.Stop());
                                                   });
                                x.RunAsLocalSystem();
                                x.SetDescription("羊群公社后台任务服务，为数据延时或定时计算、排序提供了可靠的基础架构。");
                                x.SetDisplayName("Sheep Community V2 Quartz Job Service");
                                x.SetServiceName("SheepJob");
                            });
        }
    }
}