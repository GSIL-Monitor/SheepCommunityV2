using System;
using System.Threading.Tasks;
using Quartz;
using ServiceStack;
using ServiceStack.Logging;
using Sheep.Job.ServiceInterface.Replies;
using Sheep.Job.ServiceModel.Replies;

namespace Sheep.Job.ServiceJob.Replies
{
    /// <summary>
    ///     计算一组回复分数。
    /// </summary>
    [DisallowConcurrentExecution]
    public class CalculateReplyJob : IJob
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CalculateReplyJob));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置计算一组回复分数的服务。
        /// </summary>
        public CalculateReplyService Service { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="CalculateReplyJob" />对象。
        /// </summary>
        /// <param name="service">计算一组回复分数的服务。</param>
        public CalculateReplyJob(CalculateReplyService service)
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
                var request = new ReplyCalculate
                              {
                                  ParentType = data.GetString("ParentType"),
                                  CreatedSince = data.GetString("CreatedSince").IsNullOrEmpty() ? (DateTime?) null : data.GetDateTimeValueFromString("CreatedSince"),
                                  ModifiedSince = data.GetString("ModifiedSince").IsNullOrEmpty() ? (DateTime?) null : data.GetDateTimeValueFromString("ModifiedSince"),
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