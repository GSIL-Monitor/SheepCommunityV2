using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Feedbacks.Entities;

namespace Sheep.ServiceModel.Feedbacks
{
    /// <summary>
    ///     新建一个反馈的请求。
    /// </summary>
    [Route("/feedbacks", HttpMethods.Post, Summary = "新建一个反馈")]
    [DataContract]
    public class FeedbackCreate : IReturn<FeedbackCreateResponse>
    {
        /// <summary>
        ///     内容。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "内容")]
        public string Content { get; set; }
    }

    /// <summary>
    ///     新建一个反馈的响应。
    /// </summary>
    [DataContract]
    public class FeedbackCreateResponse : IHasResponseStatus
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