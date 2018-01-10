using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ServiceStack.Quartz.Services.Models
{
    /// <summary>
    ///     操作作业系统的请求。
    /// </summary>
    [Route("/quartz/operate", HttpMethods.Post, Summary = "操作作业系统")]
    [DataContract]
    public class QuartzOperate : IReturn<QuartzOperateResponse>
    {
        /// <summary>
        ///     待机。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "待机")]
        public bool? Standby { get; set; }

        /// <summary>
        ///     停止。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "停止")]
        public bool? Shutdown { get; set; }

        /// <summary>
        ///     暂停所有作业。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "暂停所有作业")]
        public bool? PauseAll { get; set; }

        /// <summary>
        ///     恢复所有作业。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "恢复所有作业")]
        public bool? ResumeAll { get; set; }

        /// <summary>
        ///     暂停作业组列表。
        /// </summary>
        [DataMember(Order = 5)]
        [ApiMember(Description = "暂停作业组列表")]
        public List<string> PauseJobGroups { get; set; }

        /// <summary>
        ///     暂停触发器组列表。
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "暂停触发器组列表")]
        public List<string> PauseTriggerGroups { get; set; }

        /// <summary>
        ///     恢复作业组列表。
        /// </summary>
        [DataMember(Order = 7)]
        [ApiMember(Description = "恢复作业组列表")]
        public List<string> ResumeJobGroups { get; set; }

        /// <summary>
        ///     恢复触发器组列表。
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "恢复触发器组列表")]
        public List<string> ResumeTriggerGroups { get; set; }

        /// <summary>
        ///     暂停作业列表。
        /// </summary>
        [DataMember(Order = 9)]
        [ApiMember(Description = "暂停作业列表")]
        public List<string> PauseJobs { get; set; }

        /// <summary>
        ///     暂停触发器列表。
        /// </summary>
        [DataMember(Order = 10)]
        [ApiMember(Description = "暂停触发器列表")]
        public List<string> PauseTriggers { get; set; }

        /// <summary>
        ///     恢复作业列表。
        /// </summary>
        [DataMember(Order = 11)]
        [ApiMember(Description = "恢复作业列表")]
        public List<string> ResumeJobs { get; set; }

        /// <summary>
        ///     恢复触发器列表。
        /// </summary>
        [DataMember(Order = 12)]
        [ApiMember(Description = "恢复触发器列表")]
        public List<string> ResumeTriggers { get; set; }
    }

    /// <summary>
    ///     操作作业系统的响应。
    /// </summary>
    [DataContract]
    public class QuartzOperateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     是否处于待机模式。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "是否处于待机模式")]
        public bool InStandbyMode { get; set; }

        /// <summary>
        ///     是否已停止。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "是否已停止")]
        public bool IsShutdown { get; set; }

        /// <summary>
        ///     是否已启动。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "是否已启动")]
        public bool IsStarted { get; set; }

        /// <summary>
        ///     暂停的触发器组列表。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "暂停的触发器组列表")]
        public List<string> PausedTriggerGroups { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 5)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}