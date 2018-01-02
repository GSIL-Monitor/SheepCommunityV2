using System;
using Hangfire;
using Hangfire.SqlServer;
using Topshelf;

namespace Sheep.Job
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("SheepHangfireDB", new SqlServerStorageOptions
                                                                                     {
                                                                                         QueuePollInterval = TimeSpan.FromSeconds(5)
                                                                                     });
            HostFactory.Run(x =>
                            {
                                x.Service<HangfireService>(configurator =>
                                                           {
                                                               configurator.ConstructUsing(settings => new HangfireService());
                                                               configurator.WhenStarted(service => service.Start());
                                                               configurator.WhenStopped(service => service.Stop());
                                                           });
                                x.RunAsLocalSystem();
                                x.SetDescription("羊群公社后台任务服务，为数据延时或定时计算、排序提供了可靠的基础架构。");
                                x.SetDisplayName("Sheep Community V2 Hangfire Job Service");
                                x.SetServiceName("SheepJob");
                            });
        }
    }
}