using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Likes.Entities;

namespace Sheep.ServiceModel.Likes
{
    /// <summary>
    ///     显示一个点赞的请求。
    /// </summary>
    [Route("/likes/{ContentId}/{UserId}", HttpMethods.Get, Summary = "显示一个点赞")]
    [DataContract]
    public class LikeShow : IReturn<LikeShowResponse>
    {
        /// <summary>
        ///     内容编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "内容编号（如帖子编号）")]
        public string ContentId { get; set; }

        /// <summary>
        ///     点赞者编号。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "点赞者编号")]
        public int UserId { get; set; }
    }

    /// <summary>
    ///     显示一个点赞的响应。
    /// </summary>
    [DataContract]
    public class LikeShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     点赞信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "点赞信息")]
        public LikeDto Like { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}