using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Recommendations.Entities;

namespace Sheep.ServiceModel.Recommendations
{
    /// <summary>
    ///     新建一个推荐的请求。
    /// </summary>
    [Route("/recommendations", HttpMethods.Post, Summary = "新建一个推荐")]
    [DataContract]
    public class RecommendationCreate : IReturn<RecommendationCreateResponse>
    {
        /// <summary>
        ///     内容类型。（可选值：帖子）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "内容类型（可选值：帖子）")]
        public string ContentType { get; set; }

        /// <summary>
        ///     内容编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "内容编号（如帖子编号）")]
        public string ContentId { get; set; }

        /// <summary>
        ///     位置。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "位置")]
        public int Position { get; set; }
    }

    /// <summary>
    ///     新建一个推荐的响应。
    /// </summary>
    [DataContract]
    public class RecommendationCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     推荐信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "推荐信息")]
        public RecommendationDto Recommendation { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}