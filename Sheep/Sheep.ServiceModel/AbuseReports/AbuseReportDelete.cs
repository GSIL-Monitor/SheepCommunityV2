using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.AbuseReports
{
    /// <summary>
    ///     删除一个举报的请求。
    /// </summary>
    [Route("/abusereports/{AbuseReportId}", HttpMethods.Delete, Summary = "删除一个举报")]
    [DataContract]
    public class AbuseReportDelete : IReturn<AbuseReportDeleteResponse>
    {
        /// <summary>
        ///     举报编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "举报编号")]
        public string ReportId { get; set; }
    }

    /// <summary>
    ///     删除一个举报的响应。
    /// </summary>
    [DataContract]
    public class AbuseReportDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}