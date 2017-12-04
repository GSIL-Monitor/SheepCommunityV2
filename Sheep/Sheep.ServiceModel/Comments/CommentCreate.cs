using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Comments.Entities;

namespace Sheep.ServiceModel.Comments
{
    /// <summary>
    ///     新建一个评论的请求。
    /// </summary>
    [Route("/comments", HttpMethods.Post, Summary = "新建一个评论")]
    [DataContract]
    public class CommentCreate : IReturn<CommentCreateResponse>
    {
        /// <summary>
        ///     上级类型。（可选值：帖子, 章, 节）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级类型（可选值：帖子, 章, 节）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "上级编号（如帖子编号）")]
        public string ParentId { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "正文内容")]
        public string Content { get; set; }
    }

    /// <summary>
    ///     新建一个评论的响应。
    /// </summary>
    [DataContract]
    public class CommentCreateResponse : IHasResponseStatus
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