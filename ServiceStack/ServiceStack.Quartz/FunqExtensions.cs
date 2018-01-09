using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Funq;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace ServiceStack.Quartz
{
    /// <summary>
    ///     使用 Funq 容器注册 Quartz 调度程序和作业。
    /// </summary>
    public static class FunqExtensions
    {
        #region 注册调度器

        /// <summary>
        ///     使用指定的配置从指定的程序集注册 Quartz 调度程序和作业。
        /// </summary>
        /// <param name="container">Funq 的IoC容器。</param>
        /// <param name="config">Quartz的配置。</param>
        public static void RegisterQuartzScheduler(this Container container, NameValueCollection config)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.IsDynamic).ToArray();
            container.RegisterQuartzScheduler(config, assemblies);
        }

        /// <summary>
        ///     使用指定的配置从指定的程序集注册 Quartz 调度程序和作业。
        /// </summary>
        /// <param name="container">Funq 的IoC容器。</param>
        /// <param name="config">Quartz的配置。</param>
        /// <param name="jobsAssemblies">作业所在的程序集。</param>
        public static void RegisterQuartzScheduler(this Container container, NameValueCollection config, Assembly[] jobsAssemblies)
        {
            jobsAssemblies.ThrowIfNull(nameof(jobsAssemblies));
            container.RegisterAs<FunqJobFactory, IJobFactory>();
            var jobTypes = jobsAssemblies.SelectMany(assembly => assembly.GetExportedTypes()).Where(type => !type.IsAbstract && typeof(IJob).IsAssignableFrom(type)).ToArray();
            jobTypes.Each(jobType => container.RegisterAutoWiredType(jobType));
            ISchedulerFactory schedulerFactory = config != null ? new StdSchedulerFactory(config) : new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.JobFactory = container.Resolve<IJobFactory>();
            container.Register(scheduler);
        }

        #endregion
    }
}