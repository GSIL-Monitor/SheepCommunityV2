using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.ChapterReads.Entities;

namespace Sheep.ServiceModel.ChapterReads
{
    /// <summary>
    ///     根据章查询并列举一组阅读信息的请求。
    /// </summary>
    [Route("/chapterreads/query/bychapter", HttpMethods.Get, Summary = "根据章查询并列举一组阅读信息")]
    [DataContract]
    public class ChapterReadListByChapter : IReturn<ChapterReadListResponse>
    {
        /// <summary>
        ///     章编号。
        /// </summary>
        [DataMember(Order = 1, Name = "chapterid", IsRequired = true)]
        [ApiMember(Description = "章编号")]
        public string ChapterId { get; set; }

        /// <summary>
        ///     是否为我的。
        /// </summary>
        [DataMember(Order = 2, Name = "ismine")]
        [ApiMember(Description = "是否为我的")]
        public bool? IsMine { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：UserId, CreatedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 4, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：UserId, CreatedDate 默认为 CreatedDate）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 5, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 6, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 7, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     根据用户查询并列举一组阅读信息的请求。
    /// </summary>
    [Route("/chapterreads/query/byuser", HttpMethods.Get, Summary = "根据用户查询并列举一组阅读信息")]
    [DataContract]
    public class ChapterReadListByUser : IReturn<ChapterReadListResponse>
    {
        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 1, Name = "userid", IsRequired = true)]
        [ApiMember(Description = "用户编号")]
        public int UserId { get; set; }

        /// <summary>
        ///     书籍编号。
        /// </summary>
        [DataMember(Order = 2, Name = "bookid")]
        [ApiMember(Description = "书籍编号")]
        public string BookId { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：ChapterId, CreatedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 4, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：ChapterId, CreatedDate 默认为 CreatedDate）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 5, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 6, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 7, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     列举一组阅读信息的响应。
    /// </summary>
    [DataContract]
    public class ChapterReadListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     阅读信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "阅读信息列表")]
        public List<ChapterReadDto> ChapterReads { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}