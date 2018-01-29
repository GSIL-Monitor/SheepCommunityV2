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

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "modifiedsince")]
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public long? ModifiedSince { get; set; }

        /// <summary>
        ///     位置。
        /// </summary>
        [DataMember(Order = 4, Name = "position")]
        [ApiMember(Description = "位置")]
        public int? Position { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：Position, CreatedDate, ModifiedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 5, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：Position, CreatedDate, ModifiedDate 默认为 CreatedDate）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 6, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 7, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 8, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
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