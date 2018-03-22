using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Posts
{
    /// <summary>
    ///     推送一个帖子的请求。
    /// </summary>
    [Route("/posts/push/{PostId}", HttpMethods.Post, Summary = "推送一个帖子")]
    [DataContract]
    public class PostPush : IReturn<PostPushResponse>
    {
        /// <summary>
        ///     帖子的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "帖子编号")]
        public string PostId { get; set; }
    }

    /// <summary>
    ///     推送一个帖子的响应。
    /// </summary>
    [DataContract]
    public class PostPushResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}