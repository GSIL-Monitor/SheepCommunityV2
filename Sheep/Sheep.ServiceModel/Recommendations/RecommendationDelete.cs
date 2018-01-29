using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Recommendations
{
    /// <summary>
    ///     删除一个推荐的请求。
    /// </summary>
    [Route("/recommendations/{RecommendationId}", HttpMethods.Delete, Summary = "删除一个推荐")]
    [DataContract]
    public class RecommendationDelete : IReturn<RecommendationDeleteResponse>
    {
        /// <summary>
        ///     推荐编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "推荐编号")]
        public string RecommendationId { get; set; }
    }

    /// <summary>
    ///     删除一个推荐的响应。
    /// </summary>
    [DataContract]
    public class RecommendationDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}