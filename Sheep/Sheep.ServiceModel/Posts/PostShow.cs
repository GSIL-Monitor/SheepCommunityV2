using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Posts.Entities;

namespace Sheep.ServiceModel.Posts
{
    /// <summary>
    ///     显示一个帖子的请求。
    /// </summary>
    [Route("/posts/{PostId}", HttpMethods.Get, Summary = "显示一个帖子")]
    [DataContract]
    public class PostShow : IReturn<PostShowResponse>
    {
        /// <summary>
        ///     帖子的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "帖子编号")]
        public string PostId { get; set; }
    }

    /// <summary>
    ///     显示一个帖子的响应。
    /// </summary>
    [DataContract]
    public class PostShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     帖子信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "帖子信息")]
        public PostDto Post { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}