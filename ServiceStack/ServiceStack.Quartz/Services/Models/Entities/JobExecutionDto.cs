using System;
using System.Runtime.Serialization;

namespace ServiceStack.Quartz.Services.Models.Entities
{
    /// <summary>
    ///     作业执行信息。
    /// </summary>
    [DataContract]
    public class JobExecutionDto
    {
        /// <summary>
        ///     标识触发此作业执行的触发器的特定触发实例的唯一编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string FireInstanceId { get; set; }

        /// <summary>
        ///     作业运行的时间量。返回的值将为 MinValue，直到作业实际完成（或抛出异常），因此通常只对 IJobListeners 和 ITriggerListeners 有用。
        /// </summary>
        [DataMember(Order = 2)]
        public TimeSpan JobRunTime { get; set; }

        /// <summary>
        ///     触发器触发的实际时间。例如，计划的时间可能是 10:00:00，但如果调度程序太忙，实际的触发时间可能是10:00:03。
        /// </summary>
        [DataMember(Order = 3)]
        public DateTimeOffset FireTimeUtc { get; set; }

        /// <summary>
        ///     触发器为其触发的计划时间。例如，计划的时间可能是 10:00:00，但如果调度程序太忙，实际的触发时间可能是10:00:03。
        /// </summary>
        [DataMember(Order = 4)]
        public DateTimeOffset? ScheduledFireTimeUtc { get; set; }

        /// <summary>
        ///     返回作业执行计划触发的下一次时间。如果作业执行不会再次触发，则返回 null。
        ///     请注意，如果为作业执行计算的时间已经到达，但调度程序还没有能够触发作业执行 （这很可能是由于缺少资源，如线程），返回的时间可能在过去。
        /// </summary>
        [DataMember(Order = 5)]
        public DateTimeOffset? NextFireTimeUtc { get; set; }

        /// <summary>
        ///     返回上次触发作业执行的时间。如果作业执行尚未触发，则返回 null。
        /// </summary>
        [DataMember(Order = 6)]
        public DateTimeOffset? PreviousFireTimeUtc { get; set; }

        /// <summary>
        ///     如果作业由于 "恢复" 情况而被重新执行，则此方法将返回 true。
        /// </summary>
        [DataMember(Order = 7)]
        public bool Recovering { get; set; }

        /// <summary>
        ///     返回最初计划的和现在正在恢复的作业的触发器键。
        /// </summary>
        [DataMember(Order = 8)]
        public TriggerKeyDto RecoveringTriggerKey { get; set; }

        /// <summary>
        ///     重新触发的次数
        /// </summary>
        [DataMember(Order = 9)]
        public int RefireCount { get; set; }

        /// <summary>
        ///     激发该作业的触发器实例。
        /// </summary>
        [DataMember(Order = 10)]
        public TriggerDto Trigger { get; set; }
    }
}