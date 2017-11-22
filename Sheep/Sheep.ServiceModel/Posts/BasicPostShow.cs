using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Posts.Entities;

namespace Sheep.ServiceModel.Posts
{
    /// <summary>
    ///     显示一个帖子基本信息的请求。
    /// </summary>
    [Route("/posts/basic/{PostId}", HttpMethods.Get, Summary = "显示一个帖子基本信息")]
    [DataContract]
    public class BasicPostShow : IReturn<BasicPostShowResponse>
    {
        /// <summary>
        ///     帖子编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "帖子编号")]
        public string PostId { get; set; }
    }

    /// <summary>
    ///     显示一个帖子基本信息的响应。
    /// </summary>
    [DataContract]
    public class BasicPostShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     帖子基本信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "帖子基本信息")]
        public BasicPostDto Post { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}