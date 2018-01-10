using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Models
{
    /// <summary>
    ///     查询并列举一组触发器的请求。
    /// </summary>
    [Route("/quartz/triggers/query", HttpMethods.Get, Summary = "查询并列举一组触发器信息")]
    [DataContract]
    public class QuartzTriggerList : IReturn<QuartzTriggerListResponse>
    {
        /// <summary>
        ///     分组的名称。
        /// </summary>
        [DataMember(Order = 1, Name = "groupname")]
        [ApiMember(Description = "分组的名称")]
        public string GroupName { get; set; }

        /// <summary>
        ///     作业的名称。
        /// </summary>
        [DataMember(Order = 2, Name = "jobname")]
        [ApiMember(Description = "作业的名称")]
        public string JobName { get; set; }
    }

    /// <summary>
    ///     查询并列举一组触发器的响应。
    /// </summary>
    [DataContract]
    public class QuartzTriggerListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     触发器列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "触发器列表")]
        public List<TriggerDto> Triggers { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}