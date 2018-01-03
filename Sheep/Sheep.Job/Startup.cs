using System;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using Sheep.Job;

[assembly: OwinStartup(typeof(Startup))]

namespace Sheep.Job
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("SheepHangfireDB", new SqlServerStorageOptions
                                                                                     {
                                                                                         QueuePollInterval = TimeSpan.FromSeconds(5)
                                                                                     });
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}