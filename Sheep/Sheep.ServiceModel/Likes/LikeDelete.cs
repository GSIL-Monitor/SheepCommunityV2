using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Likes
{
    /// <summary>
    ///     取消一个点赞的请求。
    /// </summary>
    [Route("/likes", HttpMethods.Delete, Summary = "取消一个点赞")]
    [DataContract]
    public class LikeDelete : IReturn<LikeDeleteResponse>
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
    ///     取消一个点赞的响应。
    /// </summary>
    [DataContract]
    public class LikeDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}