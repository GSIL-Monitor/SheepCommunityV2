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
    ///     显示一个触发器服务接口。
    /// </summary>
    public class ShowQuartzTriggerService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowQuartzTriggerService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个触发器的校验器。
        /// </summary>
        public IValidator<QuartzTriggerShow> QuartzTriggerShowValidator { get; set; }

        /// <summary>
        ///     获取及设置调度器。
        /// </summary>
        public IScheduler Scheduler { get; set; }

        #endregion

        #region 显示一个触发器

        /// <summary>
        ///     显示一个触发器。
        /// </summary>
        public async Task<object> Get(QuartzTriggerShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    QuartzTriggerShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var triggerKey = new TriggerKey(request.TriggerName, request.GroupName);
            var existingTrigger = await Scheduler.GetTrigger(triggerKey);
            if (existingTrigger == null)
            {
                throw HttpError.NotFound(string.Format(Resources.TriggerNotFound, string.Format("{0}:{1}", triggerKey.Name, triggerKey.Group)));
            }
            var triggerDto = existingTrigger.MapToTriggerDto();
            return new QuartzTriggerShowResponse
                   {
                       Trigger = triggerDto
                   };
        }

        #endregion
    }
}