using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Feedbacks
{
    /// <summary>
    ///     删除一个反馈的请求。
    /// </summary>
    [Route("/feedbacks/{FeedbackId}", HttpMethods.Delete, Summary = "删除一个反馈")]
    [DataContract]
    public class FeedbackDelete : IReturn<FeedbackDeleteResponse>
    {
        /// <summary>
        ///     反馈编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "反馈编号")]
        public string FeedbackId { get; set; }
    }

    /// <summary>
    ///     删除一个反馈的响应。
    /// </summary>
    [DataContract]
    public class FeedbackDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}