using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Recommendations.Entities;

namespace Sheep.ServiceModel.Recommendations
{
    /// <summary>
    ///     更新一个推荐的请求。
    /// </summary>
    [Route("/recommendations/{RecommendationId}", HttpMethods.Put, Summary = "更新一个推荐")]
    [DataContract]
    public class RecommendationUpdate : IReturn<RecommendationUpdateResponse>
    {
        /// <summary>
        ///     推荐编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "推荐编号")]
        public string RecommendationId { get; set; }

        /// <summary>
        ///     位置。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "位置")]
        public int Position { get; set; }
    }

    /// <summary>
    ///     更新一个推荐的响应。
    /// </summary>
    [DataContract]
    public class RecommendationUpdateResponse : IHasResponseStatus
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