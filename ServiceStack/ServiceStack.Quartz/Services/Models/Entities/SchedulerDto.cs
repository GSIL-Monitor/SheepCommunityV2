using System;
using System.Runtime.Serialization;

namespace ServiceStack.Quartz.Services.Models.Entities
{
    /// <summary>
    ///     调度程序信息。
    /// </summary>
    [DataContract]
    public class SchedulerDto
    {
        /// <summary>
        ///     报告调度程序是否处于待机模式。
        /// </summary>
        [DataMember(Order = 1)]
        public bool InStandbyMode { get; set; }

        /// <summary>
        ///     返回调度程序的作业存储区是否为群集。
        /// </summary>
        [DataMember(Order = 2)]
        public bool JobStoreClustered { get; set; }

        /// <summary>
        ///     返回调度程序的作业存储实例是否支持持久性。
        /// </summary>
        [DataMember(Order = 3)]
        public bool JobStoreSupportsPersistence { get; set; }

        /// <summary>
        ///     返回调度程序正在使用的作业存储实例的类名。
        /// </summary>
        [DataMember(Order = 4)]
        public string JobStoreType { get; set; }

        /// <summary>
        ///     返回自调度程序启动后执行的作业数。
        /// </summary>
        [DataMember(Order = 5)]
        public int NumberOfJobsExecuted { get; set; }

        /// <summary>
        ///     返回调度程序开始运行的 DateTimeOffset。
        /// </summary>
        [DataMember(Order = 6)]
        public DateTimeOffset? RunningSince { get; set; }

        /// <summary>
        ///     返回调度程序运行的时间。
        /// </summary>
        [DataMember(Order = 7)]
        public TimeSpan? RunTime
        {
            get
            {
                return RunningSince?.Subtract(DateTimeOffset.Now);
            }
        }

        /// <summary>
        ///     返回调度程序的实例编号。
        /// </summary>
        [DataMember(Order = 8)]
        public string SchedulerInstanceId { get; set; }

        /// <summary>
        ///     返回调度程序的名称。
        /// </summary>
        [DataMember(Order = 9)]
        public string SchedulerName { get; set; }

        /// <summary>
        ///     返回是否正在远程使用调度程序 (通过远程处理)。
        /// </summary>
        [DataMember(Order = 10)]
        public bool SchedulerRemote { get; set; }

        /// <summary>
        ///     返回调度程序实例的类名。
        /// </summary>
        [DataMember(Order = 11)]
        public string SchedulerType { get; set; }

        /// <summary>
        ///     报告调度程序是否已关闭。
        /// </summary>
        [DataMember(Order = 12)]
        public bool Shutdown { get; set; }

        /// <summary>
        ///     返回调度程序是否已启动。
        /// </summary>
        [DataMember(Order = 13)]
        public bool Started { get; set; }

        /// <summary>
        ///     返回调度程序中当前的线程数。
        /// </summary>
        [DataMember(Order = 14)]
        public int ThreadPoolSize { get; set; }

        /// <summary>
        ///     返回调度程序正在使用的线程池实例的类型名称。
        /// </summary>
        [DataMember(Order = 15)]
        public string ThreadPoolType { get; set; }

        /// <summary>
        ///     返回正在运行的Quartz的版本。
        /// </summary>
        [DataMember(Order = 16)]
        public string Version { get; set; }
    }
}