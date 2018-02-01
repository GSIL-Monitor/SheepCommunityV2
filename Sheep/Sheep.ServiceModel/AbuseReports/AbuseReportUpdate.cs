using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.AbuseReports.Entities;

namespace Sheep.ServiceModel.AbuseReports
{
    /// <summary>
    ///     更新一个举报的请求。
    /// </summary>
    [Route("/abusereports/{AbuseReportId}", HttpMethods.Put, Summary = "更新一个举报")]
    [DataContract]
    public class AbuseReportUpdate : IReturn<AbuseReportUpdateResponse>
    {
        /// <summary>
        ///     举报编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "举报编号")]
        public string ReportId { get; set; }

        /// <summary>
        ///     原因。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "原因")]
        public string Reason { get; set; }
    }

    /// <summary>
    ///     更新一个举报的响应。
    /// </summary>
    [DataContract]
    public class AbuseReportUpdateResponse : IHasResponseStatus
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