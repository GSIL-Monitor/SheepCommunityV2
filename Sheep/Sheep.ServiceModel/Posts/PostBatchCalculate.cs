using System;
using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Posts
{
    /// <summary>
    ///     查询并计算一组帖子分数的请求。
    /// </summary>
    [Route("/posts/calculate", HttpMethods.Put, Summary = "查询并计算一组帖子分数信息")]
    [DataContract]
    public class PostBatchCalculate : IReturn<PostBatchCalculateResponse>
    {
        /// <summary>
        ///     过滤标题及概要。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "过滤标题及概要")]
        public string TitleFilter { get; set; }

        /// <summary>
        ///     分类的标签。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "分类的标签")]
        public string Tag { get; set; }

        /// <summary>
        ///     内容的类型。（可选值：图文, 音频, 视频）
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "内容的类型（可选值：图文, 音频, 视频）")]
        public string ContentType { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public DateTime? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 5)]
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public DateTime? ModifiedSince { get; set; }

        /// <summary>
        ///     发布日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "发布日期在指定的时间之后")]
        public DateTime? PublishedSince { get; set; }

        /// <summary>
        ///     是否已发布。
        /// </summary>
        [DataMember(Order = 7)]
        [ApiMember(Description = "是否已发布")]
        public bool? IsPublished { get; set; }

        /// <summary>
        ///     是否标记为精选。
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "是否标记为精选")]
        public bool? IsFeatured { get; set; }

        /// <summary>
        ///     排序的字段。（可选值： CreatedDate, ModifiedDate, PublishedDate, ViewsCount, BookmarksCount, CommentsCount, LikesCount,
        ///     RatingsCount, RatingsAverageValue, SharesCount, AbuseReportsCount, ContentQuality 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 9)]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate, PublishedDate, ViewsCount, BookmarksCount, CommentsCount, LikesCount, RatingsCount, RatingsAverageValue, SharesCount, AbuseReportsCount, ContentQuality 默认为 CreatedDate）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 10)]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 11)]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 12)]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     查询并计算一组帖子分数的响应。
    /// </summary>
    [DataContract]
    public class PostBatchCalculateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}