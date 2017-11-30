using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Replies.Entities;

namespace Sheep.ServiceModel.Replies
{
    /// <summary>
    ///     新建一个回复的请求。
    /// </summary>
    [Route("/replies", HttpMethods.Post, Summary = "新建一个回复")]
    [DataContract]
    public class ReplyCreate : IReturn<ReplyCreateResponse>
    {
        /// <summary>
        ///     上级类型。（可选值：评论）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级类型（可选值：评论）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如评论编号）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "上级编号（如评论编号）")]
        public string ParentId { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "正文内容")]
        public string Content { get; set; }
    }

    /// <summary>
    ///     新建一个回复的响应。
    /// </summary>
    [DataContract]
    public class ReplyCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     回复信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "回复信息")]
        public ReplyDto Reply { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}