using System.Linq;
using System.Threading.Tasks;
using Quartz;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Quartz.Properties;
using ServiceStack.Quartz.Services.Mappers;
using ServiceStack.Quartz.Services.Models;

namespace ServiceStack.Quartz.Services
{
    /// <summary>
    ///     显示一个作业服务接口。
    /// </summary>
    public class ShowQuartzJobService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowQuartzJobService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个作业的校验器。
        /// </summary>
        public IValidator<QuartzJobShow> QuartzJobShowValidator { get; set; }

        /// <summary>
        ///     获取及设置调度器。
        /// </summary>
        public IScheduler Scheduler { get; set; }

        #endregion

        #region 显示一个作业

        /// <summary>
        ///     显示一个作业。
        /// </summary>
        public async Task<object> Get(QuartzJobShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    QuartzJobShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var jobKey = JobKey.Create(request.JobName, request.GroupName);
            var existingJob = await Scheduler.GetJobDetail(jobKey);
            if (existingJob == null)
            {
                throw HttpError.NotFound(string.Format(Resources.JobNotFound, string.Format("{0}:{1}", jobKey.Name, jobKey.Group)));
            }
            var existingTriggers = await Scheduler.GetTriggersOfJob(jobKey);
            var jobDto = existingJob.MapToJobDto(existingTriggers.Select(trigger => trigger.MapToTriggerDto()).ToArray());
            return new QuartzJobShowResponse
                   {
                       Job = jobDto
                   };
        }

        #endregion
    }
}