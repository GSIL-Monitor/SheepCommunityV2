using System;
using System.Threading.Tasks;
using Quartz;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Job.ServiceInterface.Groups;
using Sheep.Job.ServiceModel.Groups;

namespace Sheep.Job.ServiceJob.Groups
{
    /// <summary>
    ///     导入一组群组。
    /// </summary>
    [DisallowConcurrentExecution]
    public class ImportGroupJob : IJob
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ImportGroupJob));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置导入一组群组的服务。
        /// </summary>
        public ImportGroupService Service { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="ImportGroupJob" />对象。
        /// </summary>
        /// <param name="service">导入一组群组的服务。</param>
        public ImportGroupJob(ImportGroupService service)
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
                var request = new GroupImport
                              {
                                  CreatedSince = data.GetString("CreatedSinceDays").IsNullOrEmpty() ? (long?) null : DateTime.UtcNow.Date.AddDays(-data.GetIntValueFromString("CreatedSinceDays")).ToUnixTime(),
                                  ModifiedSince = data.GetString("ModifiedSinceDays").IsNullOrEmpty() ? (long?) null : DateTime.UtcNow.Date.AddDays(-data.GetIntValueFromString("ModifiedSinceDays")).ToUnixTime(),
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