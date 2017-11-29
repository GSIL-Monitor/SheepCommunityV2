using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Comments.Entities;

namespace Sheep.ServiceModel.Comments
{
    /// <summary>
    ///     显示一个评论的请求。
    /// </summary>
    [Route("/comments/{CommentId}", HttpMethods.Get, Summary = "显示一个评论")]
    [DataContract]
    public class CommentShow : IReturn<CommentShowResponse>
    {
        /// <summary>
        ///     评论的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "评论编号")]
        public string CommentId { get; set; }
    }

    /// <summary>
    ///     显示一个评论的响应。
    /// </summary>
    [DataContract]
    public class CommentShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     评论信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "评论信息")]
        public CommentDto Comment { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}