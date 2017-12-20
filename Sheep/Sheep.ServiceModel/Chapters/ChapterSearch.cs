using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Chapters.Entities;

namespace Sheep.ServiceModel.Chapters
{
    /// <summary>
    ///     搜索一组章的请求。
    /// </summary>
    [Route("/books/{BookId}/chapters/query", HttpMethods.Get, Summary = "搜索一组章信息")]
    [DataContract]
    public class ChapterSearch : IReturn<ChapterSearchResponse>
    {
        /// <summary>
        ///     书籍编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "书籍编号")]
        public string BookId { get; set; }

        /// <summary>
        ///     卷序号。
        /// </summary>
        [DataMember(Order = 2, Name = "volumenumber")]
        [ApiMember(Description = "卷序号")]
        public int? VolumeNumber { get; set; }

        /// <summary>
        ///     过滤内容。
        /// </summary>
        [DataMember(Order = 3, Name = "contentfilter")]
        [ApiMember(Description = "过滤内容")]
        public string ContentFilter { get; set; }

        /// <summary>
        ///     是否加载注释。
        /// </summary>
        [DataMember(Order = 4, Name = "loadannotations")]
        [ApiMember(Description = "是否加载注释")]
        public bool? LoadAnnotations { get; set; }

        /// <summary>
        ///     是否加载节。
        /// </summary>
        [DataMember(Order = 5, Name = "loadparagraphs")]
        [ApiMember(Description = "是否加载节")]
        public bool? LoadParagraphs { get; set; }

        /// <summary>
        ///     排序的字段。（可选值： Number, ParagraphsCount, ViewsCount, BookmarksCount, CommentsCount, LikesCount, RatingsCount,
        ///     RatingsAverageValue, SharesCount 默认为 Number）
        /// </summary>
        [DataMember(Order = 6, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值： Number, ParagraphsCount, ViewsCount, BookmarksCount, CommentsCount, LikesCount, RatingsCount, RatingsAverageValue, SharesCount 默认为 Number）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 7, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 8, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 9, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     搜索一组章的响应。
    /// </summary>
    [DataContract]
    public class ChapterSearchResponse : IHasResponseStatus
    {
        /// <summary>
        ///     章信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "章信息列表")]
        public List<ChapterDto> Chapters { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}