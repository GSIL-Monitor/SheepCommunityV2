using System.Threading;
using System.Threading.Tasks;
using Quartz;
using ServiceStack.Quartz.Services.Mappers;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz
{
    /// <summary>
    ///     内存中的作业执行监听器。
    /// </summary>
    public class InMemoryJobListener : IJobListener
    {
        #region 属性

        /// <summary>
        ///     作业执行的历史记录列表。
        /// </summary>
        public CircularBuffer<JobExecutionHistoryDto> Histories { get; } = new CircularBuffer<JobExecutionHistoryDto>(1000);

        #endregion

        #region IJobListener 接口实现

        /// <inheritdoc />
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            context.Put("HistoryIdKey", context.FireInstanceId);
            return context.AsTaskResult();
        }

        /// <inheritdoc />
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            Histories.PushFront(new JobExecutionHistoryDto
                                {
                                    Id = context.FireInstanceId,
                                    JobKey = context.JobDetail.Key.MapToJobKeyDto(),
                                    JobRunTime = context.JobRunTime,
                                    JobDataMap = context.MergedJobDataMap.MapToJobDataMapDto(),
                                    Status = "Vetoed"
                                });
            return context.AsTaskResult();
        }

        /// <inheritdoc />
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = new CancellationToken())
        {
            // Retrieve history id from context and update history record.
            Histories.PushFront(new JobExecutionHistoryDto
                                {
                                    Id = context.FireInstanceId,
                                    JobKey = context.JobDetail.Key.MapToJobKeyDto(),
                                    JobRunTime = context.JobRunTime,
                                    JobDataMap = context.MergedJobDataMap.MapToJobDataMapDto(),
                                    Exception = jobException.UnwrapIfSingleException(),
                                    Status = jobException == null ? "Success" : "Failed"
                                });
            return context.AsTaskResult();
        }

        /// <inheritdoc />
        public string Name { get; set; } = "In Memory Job Listener";

        #endregion
    }
}