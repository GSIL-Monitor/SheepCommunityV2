using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.AbuseReports.Entities;

namespace Sheep.ServiceModel.AbuseReports
{
    /// <summary>
    ///     新建一个举报的请求。
    /// </summary>
    [Route("/abusereports", HttpMethods.Post, Summary = "新建一个举报")]
    [DataContract]
    public class AbuseReportCreate : IReturn<AbuseReportCreateResponse>
    {
        /// <summary>
        ///     上级类型。（可选值：用户, 帖子, 评论, 回复）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级类型（可选值：用户, 帖子, 评论, 回复）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "上级编号（如帖子编号）")]
        public string ParentId { get; set; }

        /// <summary>
        ///     原因。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "原因")]
        public string Reason { get; set; }
    }

    /// <summary>
    ///     新建一个举报的响应。
    /// </summary>
    [DataContract]
    public class AbuseReportCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     举报信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "举报信息")]
        public AbuseReportDto AbuseReport { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}