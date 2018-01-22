using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Books.Entities;

namespace Sheep.ServiceModel.Books
{
    /// <summary>
    ///     查询并列举一组书籍的请求。
    /// </summary>
    [Route("/books/query", HttpMethods.Get, Summary = "查询并列举一组书籍信息")]
    [DataContract]
    public class BookList : IReturn<BookListResponse>
    {
        /// <summary>
        ///     过滤标题及概要。
        /// </summary>
        [DataMember(Order = 1, Name = "titlefilter")]
        [ApiMember(Description = "过滤标题及概要")]
        public string TitleFilter { get; set; }

        /// <summary>
        ///     分类的标签。
        /// </summary>
        [DataMember(Order = 2, Name = "tag")]
        [ApiMember(Description = "分类的标签")]
        public string Tag { get; set; }

        /// <summary>
        ///     发布日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "publishedsince")]
        [ApiMember(Description = "发布日期在指定的时间之后")]
        public long? PublishedSince { get; set; }

        /// <summary>
        ///     是否已发布。
        /// </summary>
        [DataMember(Order = 4, Name = "ispublished")]
        [ApiMember(Description = "是否已发布")]
        public bool? IsPublished { get; set; }

        /// <summary>
        ///     排序的字段。（可选值： PublishedDate, VolumesCount, BookmarksCount, RatingsCount, RatingsAverageValue, SharesCount 默认为
        ///     PublishedDate）
        /// </summary>
        [DataMember(Order = 5, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值： PublishedDate, VolumesCount, BookmarksCount, RatingsCount, RatingsAverageValue, SharesCount 默认为 PublishedDate）")]
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
    ///     查询并列举一组书籍的响应。
    /// </summary>
    [DataContract]
    public class BookListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     书籍信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "书籍信息列表")]
        public List<BookDto> Books { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}