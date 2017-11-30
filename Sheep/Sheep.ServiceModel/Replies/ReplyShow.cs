using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Replies.Entities;

namespace Sheep.ServiceModel.Replies
{
    /// <summary>
    ///     显示一个回复的请求。
    /// </summary>
    [Route("/replies/{ReplyId}", HttpMethods.Get, Summary = "显示一个回复")]
    [DataContract]
    public class ReplyShow : IReturn<ReplyShowResponse>
    {
        /// <summary>
        ///     回复的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "回复编号")]
        public string ReplyId { get; set; }
    }

    /// <summary>
    ///     显示一个回复的响应。
    /// </summary>
    [DataContract]
    public class ReplyShowResponse : IHasResponseStatus
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