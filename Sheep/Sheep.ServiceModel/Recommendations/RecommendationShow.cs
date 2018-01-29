using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Recommendations.Entities;

namespace Sheep.ServiceModel.Recommendations
{
    /// <summary>
    ///     显示一个推荐的请求。
    /// </summary>
    [Route("/recommendations/{RecommendationId}", HttpMethods.Get, Summary = "显示一个推荐")]
    [DataContract]
    public class RecommendationShow : IReturn<RecommendationShowResponse>
    {
        /// <summary>
        ///     推荐的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "推荐编号")]
        public string RecommendationId { get; set; }
    }

    /// <summary>
    ///     显示一个推荐的响应。
    /// </summary>
    [DataContract]
    public class RecommendationShowResponse : IHasResponseStatus
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