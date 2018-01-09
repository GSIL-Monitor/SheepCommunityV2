using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Quartz;
using ServiceStack.Extensions;
using ServiceStack.Quartz.Services;

namespace ServiceStack.Quartz
{
    /// <summary>
    ///     设置 Quartz.Net 计划程序并扫描程序集以在默认情况下注册 IJob 实现 Quartz.net 标准应用程序配置自动使用。
    /// </summary>
    public class QuartzFeature : IPreInitPlugin, IPlugin
    {
        #region 属性 

        /// <summary>
        ///     设置 Quartz 特定配置重写默认值。默认情况下, 所有以 "quartz." 开头的键都从配置中读取。
        /// </summary>
        public Dictionary<string, string> Config { get; } = new Dictionary<string, string>();

        /// <summary>
        ///     要扫描 IJob 实现的程序集。
        /// </summary>
        public Assembly[] JobAssemblies { get; set; }

        /// <summary>
        ///     检查导出的公共 IJob 具体类型的所有程序集。默认情况下为 True。
        /// </summary>
        public bool ScanAppHostAssemblies { get; set; } = true;

        /// <summary>
        ///     作业列表。
        /// </summary>
        internal Dictionary<JobKey, JobInstance> Jobs { get; } = new Dictionary<JobKey, JobInstance>();

        #endregion

        #region IPreInitPlugin 接口实现

        /// <inheritdoc />
        public void Configure(IAppHost appHost)
        {
        }

        #endregion

        #region IPlugin 接口实现

        /// <inheritdoc />
        public void Register(IAppHost appHost)
        {
            var quartzSettings = appHost.AppSettings.GetAllKeysStartingWith("quartz.");
            foreach (var setting in quartzSettings)
            {
                if (Config[setting.Key] == null)
                {
                    Config.Add(setting.Key, setting.Value);
                }
            }
            appHost.RegisterService<SummaryQuartzService>();
            appHost.RegisterService<ShowQuartzJobService>();
            appHost.AfterInitCallbacks.Add(RegisterAndStartScheduler);
            appHost.OnDisposeCallbacks.Add(ShutdownScheduler);
        }

        #endregion

        #region 回调方法

        /// <summary>
        ///     注册并启动调度程序。
        /// </summary>
        private void RegisterAndStartScheduler(IAppHost appHost)
        {
            var container = appHost.GetContainer();
            if (ScanAppHostAssemblies)
            {
                container.RegisterQuartzScheduler(Config.ToNameValueCollection());
            }
            else
            {
                container.RegisterQuartzScheduler(Config.ToNameValueCollection(), JobAssemblies);
            }
            var scheduler = container.Resolve<IScheduler>();
            foreach (var job in Jobs)
            {
                scheduler.ScheduleJob(job.Value.JobDetail, new ReadOnlyCollection<ITrigger>(job.Value.Triggers), true).Wait();
            }
            scheduler.Start().Wait();
            //scheduler.ListenerManager.AddJobListener();
        }

        /// <summary>
        ///     操作关闭调度程序。
        /// </summary>
        private void ShutdownScheduler(IAppHost appHost)
        {
            var container = appHost.GetContainer();
            var scheduler = container.Resolve<IScheduler>();
            if (!scheduler.IsShutdown)
            {
                scheduler.Shutdown().Wait();
            }
        }

        #endregion

        #region 添加作业

        /// <summary>
        ///     添加要运行的作业。
        /// </summary>
        /// <param name="jobDetail">作业明细。</param>
        /// <param name="triggers">触发器列表。</param>
        public void AddJob(IJobDetail jobDetail, params ITrigger[] triggers)
        {
            Jobs.Add(jobDetail.Key, new JobInstance
                                    {
                                        JobDetail = jobDetail,
                                        Triggers = triggers
                                    });
        }

        #endregion
    }
}