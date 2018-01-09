using System;
using Funq;
using Quartz;
using Quartz.Spi;
using ServiceStack.Logging;

namespace ServiceStack.Quartz
{
    /// <summary>
    ///     基于Funq IoC容器来解决作业内部依赖性的作业工厂。
    /// </summary>
    /// <seealso cref="T:Quartz.Spi.IJobFactory" />
    /// <seealso cref="T:Quartz.Simpl.PropertySettingJobFactory" />
    public class FunqJobFactory : IJobFactory
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(FunqJobFactory));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        private readonly Container _container;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="FunqJobFactory" />对象。
        /// </summary>
        /// <param name="container">Funq 的IoC容器。</param>
        public FunqJobFactory(Container container)
        {
            _container = container;
        }

        #endregion

        #region IJobFactory 接口实现

        /// <summary>
        ///     通过Quartz调度器在触发器触发时调用，以产生一个 IJob 的实例来调用执行。
        /// </summary>
        /// <remarks>
        ///     这种方法抛出异常应该非常少见--基本上, 只有在没有办法实例化和准备执行作业的情况下。
        ///     当引发异常时, 计划程序将将与该作业关联的所有触发器移到<see cref="F:Quartz.TriggerState.Error" />状态，
        ///     这将需要人工干预 （例如，在修复任何配置后重新启动应用程序问题导致了实例化作业的问题）。
        /// </remarks>
        /// <param name="bundle">可从其中获取与触发器触发有关的<see cref="T:Quartz.IJobDetail" />和其他信息的 TriggerFiredBundle。</param>
        /// <param name="scheduler">调度器。</param>
        /// <returns>新实例化的作业。</returns>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            bundle.ThrowIfNull(nameof(bundle));
            scheduler.ThrowIfNull(nameof(scheduler));
            var jobDetail = bundle.JobDetail;
            var job = (IJob) _container.TryResolve(jobDetail.JobType);
            if (job == null)
            {
                throw new SchedulerConfigException($"Failed to instantiate Job {jobDetail.Key} of type {jobDetail.JobType}");
            }
            Log.DebugFormat("Created new Job {0}:{1} of {2}", jobDetail.Key.Group, jobDetail.Key.Name, jobDetail.JobType);
            return job;
        }

        /// <summary>
        ///     允许作业工厂在需要时销毁/清理作业。
        /// </summary>
        /// <param name="job">作业。</param>
        public void ReturnJob(IJob job)
        {
            if (job is IDisposable disposableJob)
            {
                disposableJob.Dispose();
                Log.DebugFormat("Disposed Job {0}", job.GetType());
            }
        }

        #endregion
    }
}