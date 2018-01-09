using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Quartz.Services.Mappers;
using ServiceStack.Quartz.Services.Models;

namespace ServiceStack.Quartz.Services
{
    /// <summary>
    ///     概述作业系统服务接口。
    /// </summary>
    public class SummaryQuartzService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(SummaryQuartzService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置概述作业系统的校验器。
        /// </summary>
        public IValidator<QuartzSummary> QuartzSummaryValidator { get; set; }

        /// <summary>
        ///     获取及设置调度器。
        /// </summary>
        public IScheduler Scheduler { get; set; }

        #endregion

        #region 概述作业系统

        /// <summary>
        ///     概述作业系统。
        /// </summary>
        public async Task<object> Get(QuartzSummary request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    QuartzSummaryValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingjobKeys = await Scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
            var existingScheduler = await Scheduler.GetMetaData();
            var existingCalendarNames = await Scheduler.GetCalendarNames();
            var existingJobGroups = await Scheduler.GetJobGroupNames();
            var existingTriggerKeys = await Scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());
            var existingTriggerGroups = await Scheduler.GetTriggerGroupNames();
            var existingPausedTriggerGroups = await Scheduler.GetPausedTriggerGroups();
            var existingCurrentlyExecutingJobs = await Scheduler.GetCurrentlyExecutingJobs();
            return new QuartzSummaryResponse
                   {
                       JobKeys = existingjobKeys.Select(jobKey => jobKey.MapToJobKeyDto()).ToList(),
                       IsInStandbyMode = Scheduler.InStandbyMode,
                       IsShutdown = Scheduler.IsShutdown,
                       IsStarted = Scheduler.IsStarted,
                       Scheduler = existingScheduler.MapToSchedulerDto(),
                       CalendarNames = existingCalendarNames.ToList(),
                       JobGroups = existingJobGroups.ToList(),
                       TriggerKeys = existingTriggerKeys.Select(triggerKey => triggerKey.MapToTriggerKeyDto()).ToList(),
                       TriggerGroups = existingTriggerGroups.ToList(),
                       PausedTriggerGroups = existingPausedTriggerGroups.ToList(),
                       CurrentlyExecutingJobs = existingCurrentlyExecutingJobs.Select(jobExecutionContext => jobExecutionContext.MapToJobExecutionDto()).ToList()
                   };
        }

        #endregion
    }
}