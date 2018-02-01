using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Feedbacks.Entities;

namespace Sheep.ServiceModel.Feedbacks
{
    /// <summary>
    ///     更新一个反馈的请求。
    /// </summary>
    [Route("/feedbacks/{FeedbackId}", HttpMethods.Put, Summary = "更新一个反馈")]
    [DataContract]
    public class FeedbackUpdate : IReturn<FeedbackUpdateResponse>
    {
        /// <summary>
        ///     反馈编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "反馈编号")]
        public string FeedbackId { get; set; }

        /// <summary>
        ///     内容。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "内容")]
        public string Content { get; set; }
    }

    /// <summary>
    ///     更新一个反馈的状态的请求。
    /// </summary>
    [Route("/feedbacks/{FeedbackId}/status", HttpMethods.Put, Summary = "更新一个反馈的状态")]
    [DataContract]
    public class FeedbackUpdateStatus : IReturn<FeedbackUpdateResponse>
    {
        /// <summary>
        ///     反馈编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "反馈编号")]
        public string FeedbackId { get; set; }

        /// <summary>
        ///     状态。（可选值：待处理, 提交技术, 提交产品, 提交运营, 等待删除）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "状态（可选值：待处理, 提交技术, 提交产品, 提交运营, 等待删除）")]
        public string Status { get; set; }
    }

    /// <summary>
    ///     更新一个反馈的响应。
    /// </summary>
    [DataContract]
    public class FeedbackUpdateResponse : IHasResponseStatus
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