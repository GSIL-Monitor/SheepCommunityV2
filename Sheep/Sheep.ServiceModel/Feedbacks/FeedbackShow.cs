using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Feedbacks.Entities;

namespace Sheep.ServiceModel.Feedbacks
{
    /// <summary>
    ///     显示一个反馈的请求。
    /// </summary>
    [Route("/feedbacks/{FeedbackId}", HttpMethods.Get, Summary = "显示一个反馈")]
    [DataContract]
    public class FeedbackShow : IReturn<FeedbackShowResponse>
    {
        /// <summary>
        ///     反馈的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "反馈编号")]
        public string FeedbackId { get; set; }
    }

    /// <summary>
    ///     显示一个反馈的响应。
    /// </summary>
    [DataContract]
    public class FeedbackShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     反馈信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "反馈信息")]
        public FeedbackDto Feedback { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}