using System.Collections.Generic;
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
    ///     列举一组触发器服务接口。
    /// </summary>
    public class ListQuartzTriggerService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListQuartzTriggerService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组触发器的校验器。
        /// </summary>
        public IValidator<QuartzTriggerList> QuartzTriggerListValidator { get; set; }

        /// <summary>
        ///     获取及设置调度器。
        /// </summary>
        public IScheduler Scheduler { get; set; }

        #endregion

        #region 列举一组触发器

        /// <summary>
        ///     列举一组触发器。
        /// </summary>
        public async Task<object> Get(QuartzTriggerList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    QuartzTriggerListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var groupMatcher = !request.GroupName.IsNullOrEmpty() ? GroupMatcher<TriggerKey>.GroupEquals(request.GroupName) : GroupMatcher<TriggerKey>.AnyGroup();
            var existingTriggerKeys = await Scheduler.GetTriggerKeys(groupMatcher);
            var triggersDto = new List<TriggerDto>(existingTriggerKeys.Count);
            foreach (var triggerKey in existingTriggerKeys)
            {
                var trigger = await Scheduler.GetTrigger(triggerKey);
                if (trigger != null)
                {
                    var triggerDto = trigger.MapToTriggerDto();
                    triggersDto.Add(triggerDto);
                }
            }
            return new QuartzTriggerListResponse
                   {
                       Triggers = triggersDto
                   };
        }

        #endregion
    }
}