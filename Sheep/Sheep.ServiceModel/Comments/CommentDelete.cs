using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Comments
{
    /// <summary>
    ///     删除一个评论的请求。
    /// </summary>
    [Route("/comments/{CommentId}", HttpMethods.Delete, Summary = "删除一个评论")]
    [DataContract]
    public class CommentDelete : IReturn<CommentDeleteResponse>
    {
        /// <summary>
        ///     评论编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "评论编号")]
        public string CommentId { get; set; }
    }

    /// <summary>
    ///     删除一个评论的响应。
    /// </summary>
    [DataContract]
    public class CommentDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}