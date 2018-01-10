using System.Collections.Generic;
using System.Linq;
using Quartz;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Quartz.Services.Models;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services
{
    /// <summary>
    ///     列举一组作业执行历史服务接口。
    /// </summary>
    public class ListQuartzJobExecutionHistoryService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListQuartzJobExecutionHistoryService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组作业执行历史的校验器。
        /// </summary>
        public IValidator<QuartzJobExecutionHistoryList> QuartzJobExecutionHistoryListValidator { get; set; }

        /// <summary>
        ///     获取及设置调度器。
        /// </summary>
        public IScheduler Scheduler { get; set; }

        #endregion

        #region 列举一组作业执行历史

        /// <summary>
        ///     列举一组作业执行历史。
        /// </summary>
        public object Get(QuartzJobExecutionHistoryList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    QuartzJobExecutionHistoryListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var listener = Scheduler.ListenerManager.GetJobListener("In Memory Job Listener") as InMemoryJobListener;
            return new QuartzJobExecutionHistoryListResponse
                   {
                       Histories = listener?.Histories.ToList() ?? new List<JobExecutionHistoryDto>()
                   };
        }

        #endregion
    }
}