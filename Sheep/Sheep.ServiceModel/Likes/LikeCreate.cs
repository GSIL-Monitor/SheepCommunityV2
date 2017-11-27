using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Likes.Entities;

namespace Sheep.ServiceModel.Likes
{
    /// <summary>
    ///     新建一个点赞的请求。
    /// </summary>
    [Route("/likes", HttpMethods.Post, Summary = "新建一个点赞")]
    [DataContract]
    public class LikeCreate : IReturn<LikeCreateResponse>
    {
        /// <summary>
        ///     上级类型。（可选值：帖子）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级类型（可选值：帖子）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "上级编号（如帖子编号）")]
        public string ParentId { get; set; }
    }

    /// <summary>
    ///     新建一个点赞的响应。
    /// </summary>
    [DataContract]
    public class LikeCreateResponse : IHasResponseStatus
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