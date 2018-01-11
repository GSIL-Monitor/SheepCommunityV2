using System;
using System.Threading.Tasks;
using Quartz;
using ServiceStack;
using ServiceStack.Logging;
using Sheep.Job.ServiceInterface.Comments;
using Sheep.Job.ServiceModel.Comments;

namespace Sheep.Job.ServiceJob.Comments
{
    /// <summary>
    ///     计算一组评论分数。
    /// </summary>
    [DisallowConcurrentExecution]
    public class CalculateCommentJob : IJob
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CalculateCommentJob));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置计算一组评论分数的服务。
        /// </summary>
        public CalculateCommentService Service { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="CalculateCommentJob" />对象。
        /// </summary>
        /// <param name="service">计算一组评论分数的服务。</param>
        public CalculateCommentJob(CalculateCommentService service)
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
                var request = new CommentCalculate
                              {
                                  ParentType = data.GetString("ParentType"),
                                  CreatedSince = data.GetString("CreatedSince").IsNullOrEmpty() ? (DateTime?) null : data.GetDateTimeValueFromString("CreatedSince"),
                                  ModifiedSince = data.GetString("ModifiedSince").IsNullOrEmpty() ? (DateTime?) null : data.GetDateTimeValueFromString("ModifiedSince"),
                                  IsFeatured = data.GetString("IsFeatured").IsNullOrEmpty() ? (bool?) null : data.GetBooleanValueFromString("IsFeatured"),
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