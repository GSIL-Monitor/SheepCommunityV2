using Topshelf;

namespace Sheep.Job
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
                            {
                                x.Service<QuartzService>(configurator =>
                                                           {
                                                               configurator.ConstructUsing(settings => new QuartzService());
                                                               configurator.WhenStarted(service => service.Start());
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