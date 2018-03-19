using System;
using System.Threading.Tasks;
using Quartz;
using ServiceStack.Logging;
using Sheep.Job.ServiceInterface.Groups;
using Sheep.Job.ServiceModel.Groups;

namespace Sheep.Job.ServiceJob.Groups
{
    /// <summary>
    ///     统计一组群组排行。
    /// </summary>
    [DisallowConcurrentExecution]
    public class CountGroupRankJob : IJob
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CountGroupRankJob));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置统计一组群组排行的服务。
        /// </summary>
        public CountGroupRankService Service { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="CountGroupRankJob" />对象。
        /// </summary>
        /// <param name="service">统计一组群组排行的服务。</param>
        public CountGroupRankJob(CountGroupRankService service)
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
                var request = new GroupRankCount();
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