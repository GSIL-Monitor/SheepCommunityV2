using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Models
{
    /// <summary>
    ///     查询并列举一组作业的请求。
    /// </summary>
    [Route("/quartz/jobs/query", HttpMethods.Get, Summary = "查询并列举一组作业信息")]
    [DataContract]
    public class QuartzJobList : IReturn<QuartzJobListResponse>
    {
        /// <summary>
        ///     分组的编号。
        /// </summary>
        [DataMember(Order = 1, Name = "groupname")]
        [ApiMember(Description = "分组的编号")]
        public string GroupName { get; set; }

        /// <summary>
        ///     作业的编号。
        /// </summary>
        [DataMember(Order = 2, Name = "jobname")]
        [ApiMember(Description = "作业的编号")]
        public string JobName { get; set; }

        /// <summary>
        ///     是否正在执行。
        /// </summary>
        [DataMember(Order = 3, Name = "executing")]
        [ApiMember(Description = "是否正在执行")]
        public bool? Executing { get; set; }
    }

    /// <summary>
    ///     查询并列举一组作业的响应。
    /// </summary>
    [DataContract]
    public class QuartzJobListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     作业列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "作业列表")]
        public List<JobDto> Jobs { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}