using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Models
{
    /// <summary>
    ///     作业系统概要的请求。
    /// </summary>
    [Route("/quartz/summary", HttpMethods.Get, Summary = "作业系统概要")]
    [DataContract]
    public class QuartzSummary : IReturn<QuartzSummaryResponse>
    {
    }

    /// <summary>
    ///     作业系统概要的响应。
    /// </summary>
    [DataContract]
    public class QuartzSummaryResponse : IHasResponseStatus
    {
        /// <summary>
        ///     作业主键。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "作业主键")]
        public List<JobKeyDto> JobKeys { get; set; }

        /// <summary>
        ///     触发器主键。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "触发器主键")]
        public List<TriggerKeyDto> TriggerKeys { get; set; }

        /// <summary>
        ///     是否处于待机模式。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "是否处于待机模式")]
        public bool IsInStandbyMode { get; set; }

        /// <summary>
        ///     是否已关闭。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "是否已关闭")]
        public bool IsShutdown { get; set; }

        /// <summary>
        ///     是否已启动。
        /// </summary>
        [DataMember(Order = 5)]
        [ApiMember(Description = "是否已启动")]
        public bool IsStarted { get; set; }

        /// <summary>
        ///     调度程序。
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "调度程序")]
        public SchedulerDto Scheduler { get; set; }

        /// <summary>
        ///     日历名称列表。
        /// </summary>
        [DataMember(Order = 7)]
        [ApiMember(Description = "日历名称列表")]
        public List<string> CalendarNames { get; set; }

        /// <summary>
        ///     作业分组列表。
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "作业分组列表")]
        public List<string> JobGroups { get; set; }

        /// <summary>
        ///     触发器分组列表。
        /// </summary>
        [DataMember(Order = 9)]
        [ApiMember(Description = "触发器分组列表")]
        public List<string> TriggerGroups { get; set; }

        /// <summary>
        ///     已暂停的触发器分组列表。
        /// </summary>
        [DataMember(Order = 10)]
        [ApiMember(Description = "已暂停的触发器分组列表")]
        public List<string> PausedTriggerGroups { get; set; }

        /// <summary>
        ///     当前执行的作业。
        /// </summary>
        [DataMember(Order = 11)]
        [ApiMember(Description = "当前执行的作业")]
        public List<JobExecutionDto> CurrentlyExecutingJobs { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 12)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}