using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Quartz.Services.Mappers;
using ServiceStack.Quartz.Services.Models;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services
{
    /// <summary>
    ///     列举一组作业服务接口。
    /// </summary>
    public class ListQuartzJobService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListQuartzJobService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组作业的校验器。
        /// </summary>
        public IValidator<QuartzJobList> QuartzJobListValidator { get; set; }

        /// <summary>
        ///     获取及设置调度器。
        /// </summary>
        public IScheduler Scheduler { get; set; }

        #endregion

        #region 列举一组作业

        /// <summary>
        ///     列举一组作业。
        /// </summary>
        public async Task<object> Get(QuartzJobList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    QuartzJobListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var groupMatcher = !request.GroupName.IsNullOrEmpty() ? GroupMatcher<JobKey>.GroupEquals(request.GroupName) : GroupMatcher<JobKey>.AnyGroup();
            var existingJobKeys = await Scheduler.GetJobKeys(groupMatcher);
            var jobsDto = new List<JobDto>(existingJobKeys.Count);
            foreach (var jobKey in existingJobKeys)
            {
                var job = await Scheduler.GetJobDetail(jobKey);
                if (job != null)
                {
                    var triggers = await Scheduler.GetTriggersOfJob(jobKey);
                    var jobDto = job.MapToJobDto(triggers.Select(trigger => trigger.MapToTriggerDto()).ToArray());
                    jobsDto.Add(jobDto);
                }
            }
            return new QuartzJobListResponse
                   {
                       Jobs = jobsDto
                   };
        }

        #endregion
    }
}