using System.Runtime.Serialization;

namespace ServiceStack.Quartz.Services.Models.Entities
{
    /// <summary>
    ///     作业信息。
    /// </summary>
    [DataContract]
    public class JobDto
    {
        /// <summary>
        ///     作业主键。
        /// </summary>
        [DataMember(Order = 1)]
        public JobKeyDto Key { get; set; }

        /// <summary>
        ///     由其创建者 (如果有的话) 给作业实例的描述。
        /// </summary>
        [DataMember(Order = 2)]
        public string Description { get; set; }

        /// <summary>
        ///     将执行的作业的实例的类型。
        /// </summary>
        [DataMember(Order = 3)]
        public string JobType { get; set; }

        /// <summary>
        ///     关联的作业类是否带有 DisallowConcurrentExecutionAttribute。
        /// </summary>
        [DataMember(Order = 4)]
        public bool ConcurrentExecutionDisallowed { get; set; }

        /// <summary>
        ///     作业是否应在其为孤立状态后继续存储 (没有触发器指向它)。
        /// </summary>
        [DataMember(Order = 5)]
        public bool Durable { get; set; }

        /// <summary>
        ///     关联的作业类是否带有 PersistJobDataAfterExecutionAttribute。
        /// </summary>
        [DataMember(Order = 6)]
        public bool PersistJobDataAfterExecution { get; set; }

        /// <summary>
        ///     设置在遇到 "恢复" 或 "故障转移" 情况时, 调度器是否应重新执行作业。
        /// </summary>
        [DataMember(Order = 7)]
        public bool RequestsRecovery { get; set; }

        /// <summary>
        ///     保存作业实例的状态数据。
        /// </summary>
        [DataMember(Order = 8)]
        public JobDataMapDto JobDataMap { get; set; }

        /// <summary>
        ///     触发器列表。
        /// </summary>
        [DataMember(Order = 9)]
        public TriggerDto[] Triggers { get; set; }
    }
}