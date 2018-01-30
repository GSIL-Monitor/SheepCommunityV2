using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Recommendations.Entities;

namespace Sheep.ServiceModel.Recommendations
{
    /// <summary>
    ///     查询并列举一组推荐的请求。
    /// </summary>
    [Route("/recommendations/query", HttpMethods.Get, Summary = "查询并列举一组推荐信息")]
    [DataContract]
    public class RecommendationList : IReturn<RecommendationListResponse>
    {
        /// <summary>
        ///     内容类型。（可选值：帖子）
        /// </summary>
        [DataMember(Order = 1, Name = "contenttype")]
        [ApiMember(Description = "内容类型（可选值：帖子）")]
        public string ContentType { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }
    }

    /// <summary>
    ///     查询并列举一组推荐的响应。
    /// </summary>
    [DataContract]
    public class RecommendationListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     推荐信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "推荐信息列表")]
        public List<RecommendationDto> Recommendations { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}