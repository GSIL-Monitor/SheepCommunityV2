using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Quartz.Services.Models;

namespace ServiceStack.Quartz.Services
{
    /// <summary>
    ///     操作作业系统服务接口。
    /// </summary>
    public class OperateQuartzService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(OperateQuartzService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置操作作业系统的校验器。
        /// </summary>
        public IValidator<QuartzOperate> QuartzOperateValidator { get; set; }

        /// <summary>
        ///     获取及设置调度器。
        /// </summary>
        public IScheduler Scheduler { get; set; }

        #endregion

        #region 操作作业系统

        /// <summary>
        ///     操作作业系统。
        /// </summary>
        public async Task<object> Post(QuartzOperate request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    QuartzOperateValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            if (request.Standby.HasValue && request.Standby.Value)
            {
                await Scheduler.Standby(CancellationToken.None);
            }
            if (request.Shutdown.HasValue && request.Shutdown.Value)
            {
                await Scheduler.Shutdown(CancellationToken.None);
            }
            if (!request.PauseJobGroups.IsEmpty())
            {
                foreach (var pauseJobGroup in request.PauseJobGroups)
                {
                    await Scheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals(pauseJobGroup));
                }
            }
            if (!request.ResumeJobGroups.IsEmpty())
            {
                foreach (var resumeJobGroup in request.ResumeJobGroups)
                {
                    await Scheduler.ResumeJobs(GroupMatcher<JobKey>.GroupEquals(resumeJobGroup));
                }
            }
            if (!request.PauseTriggerGroups.IsEmpty())
            {
                foreach (var pauseTriggerGroup in request.PauseTriggerGroups)
                {
                    await Scheduler.PauseTriggers(GroupMatcher<TriggerKey>.GroupEquals(pauseTriggerGroup));
                }
            }
            if (!request.ResumeTriggerGroups.IsEmpty())
            {
                foreach (var resumeTriggerGroup in request.ResumeTriggerGroups)
                {
                    await Scheduler.ResumeTriggers(GroupMatcher<TriggerKey>.GroupEquals(resumeTriggerGroup));
                }
            }
            if (request.PauseAll.HasValue && request.PauseAll.Value)
            {
                await Scheduler.PauseAll();
            }
            if (request.ResumeAll.HasValue && request.ResumeAll.Value)
            {
                await Scheduler.ResumeAll();
            }
            if (!request.PauseJobs.IsEmpty())
            {
                foreach (var pauseJob in request.PauseJobs)
                {
                    await Scheduler.PauseJob(JobKey.Create(pauseJob), CancellationToken.None);
                }
            }
            if (!request.ResumeJobs.IsEmpty())
            {
                foreach (var resumeJob in request.ResumeJobs)
                {
                    await Scheduler.ResumeJob(JobKey.Create(resumeJob), CancellationToken.None);
                }
            }
            if (!request.PauseTriggers.IsEmpty())
            {
                foreach (var pauseTrigger in request.PauseTriggers)
                {
                    await Scheduler.PauseTrigger(new TriggerKey(pauseTrigger), CancellationToken.None);
                }
            }
            if (!request.ResumeTriggers.IsEmpty())
            {
                foreach (var resumeTrigger in request.ResumeTriggers)
                {
                    await Scheduler.ResumeTrigger(new TriggerKey(resumeTrigger), CancellationToken.None);
                }
            }
            var pausedTriggerGroups = await Scheduler.GetPausedTriggerGroups();
            return new QuartzOperateResponse
                   {
                       InStandbyMode = Scheduler.InStandbyMode,
                       IsShutdown = Scheduler.IsShutdown,
                       IsStarted = Scheduler.IsStarted,
                       PausedTriggerGroups = pausedTriggerGroups.ToList()
                   };
        }

        #endregion
    }
}