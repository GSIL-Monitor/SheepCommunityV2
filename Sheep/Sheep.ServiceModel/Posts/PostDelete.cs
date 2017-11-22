using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Posts
{
    /// <summary>
    ///     删除一个帖子的请求。
    /// </summary>
    [Route("/posts/{PostId}", HttpMethods.Delete, Summary = "删除一个帖子")]
    [DataContract]
    public class PostDelete : IReturn<PostDeleteResponse>
    {
        /// <summary>
        ///     帖子编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "帖子编号")]
        public string PostId { get; set; }
    }

    /// <summary>
    ///     删除一个帖子的响应。
    /// </summary>
    [DataContract]
    public class PostDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}