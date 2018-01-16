using System;
using System.Threading.Tasks;
using Quartz;
using ServiceStack;
using ServiceStack.Logging;
using Sheep.Job.ServiceInterface.Users;
using Sheep.Job.ServiceModel.Users;

namespace Sheep.Job.ServiceJob.Users
{
    /// <summary>
    ///     计算一组用户声望。
    /// </summary>
    [DisallowConcurrentExecution]
    public class CalculateUserJob : IJob
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CalculateUserJob));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置计算一组用户声望的服务。
        /// </summary>
        public CalculateUserService Service { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="CalculateUserJob" />对象。
        /// </summary>
        /// <param name="service">计算一组用户声望的服务。</param>
        public CalculateUserJob(CalculateUserService service)
        {
            Service = service;
        }

        #endregion

        #region IJob 接口实现

        /// <inheritdoc />
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var data = context.MergedJobDataMap;
                var request = new UserCalculate
                              {
                                  UserNameFilter = data.GetString("UserNameFilter"),
                                  NameFilter = data.GetString("NameFilter"),
                                  CreatedSince = data.GetString("CreatedSinceDays").IsNullOrEmpty() ? (DateTime?) null : DateTime.UtcNow.Date.AddDays(-data.GetIntValueFromString("CreatedSinceDays")),
                                  ModifiedSince = data.GetString("ModifiedSinceDays").IsNullOrEmpty() ? (DateTime?) null : DateTime.UtcNow.Date.AddDays(-data.GetIntValueFromString("ModifiedSinceDays")),
                                  LockedSince = data.GetString("LockedSinceDays").IsNullOrEmpty() ? (DateTime?) null : DateTime.UtcNow.Date.AddDays(-data.GetIntValueFromString("LockedSinceDays")),
                                  OrderBy = data.GetString("OrderBy"),
                                  Descending = data.GetString("Descending").IsNullOrEmpty() ? (bool?) null : data.GetBooleanValueFromString("Descending"),
                                  Skip = data.GetString("Skip").IsNullOrEmpty() ? (int?) null : data.GetIntValueFromString("Skip"),
                                  Limit = data.GetString("Limit").IsNullOrEmpty() ? (int?) null : data.GetIntValueFromString("Limit")
                              };
                await Service.Put(request);
            }
            catch (Exception ex)
            {
                throw new JobExecutionException(string.Format("{0}", ex.Message), ex, false);
            }
        }

        #endregion
    }
}