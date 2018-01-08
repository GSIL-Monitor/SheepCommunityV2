using System;
using System.Runtime.Serialization;

namespace ServiceStack.Quartz.Services.Models.Entities
{
    /// <summary>
    ///     作业执行历史信息。
    /// </summary>
    [DataContract]
    public class JobExecutionHistoryDto
    {
        /// <summary>
        ///     唯一编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     作业主键。
        /// </summary>
        [DataMember(Order = 2)]
        public JobKeyDto JobKey { get; set; }

        /// <summary>
        ///     作业运行的时间量。
        /// </summary>
        [DataMember(Order = 3)]
        public TimeSpan JobRunTime { get; set; }

        /// <summary>
        ///     保存作业实例的状态数据。
        /// </summary>
        [DataMember(Order = 4)]
        public JobDataMapDto JobDataMap { get; set; }

        /// <summary>
        ///     执行状态。
        /// </summary>
        [DataMember(Order = 5)]
        public string Status { get; set; }

        /// <summary>
        ///     执行的错误。
        /// </summary>
        [DataMember(Order = 6)]
        public Exception Exception { get; set; }
    }
}