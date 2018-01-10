using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Models
{
    /// <summary>
    ///     查询并列举一组作业执行历史的请求。
    /// </summary>
    [Route("/quartz/jobs/histories/query", HttpMethods.Get, Summary = "查询并列举一组作业执行历史信息")]
    [DataContract]
    public class QuartzJobExecutionHistoryList : IReturn<QuartzJobExecutionHistoryListResponse>
    {
    }

    /// <summary>
    ///     查询并列举一组作业执行历史的响应。
    /// </summary>
    [DataContract]
    public class QuartzJobExecutionHistoryListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     作业执行历史列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "作业执行历史列表")]
        public List<JobExecutionHistoryDto> Histories { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}