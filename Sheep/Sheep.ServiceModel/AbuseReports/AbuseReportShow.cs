using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.AbuseReports.Entities;

namespace Sheep.ServiceModel.AbuseReports
{
    /// <summary>
    ///     显示一个举报的请求。
    /// </summary>
    [Route("/abusereports/{AbuseReportId}", HttpMethods.Get, Summary = "显示一个举报")]
    [DataContract]
    public class AbuseReportShow : IReturn<AbuseReportShowResponse>
    {
        /// <summary>
        ///     举报的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "举报编号")]
        public string ReportId { get; set; }
    }

    /// <summary>
    ///     显示一个举报的响应。
    /// </summary>
    [DataContract]
    public class AbuseReportShowResponse : IHasResponseStatus
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