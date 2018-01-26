using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.ChapterReads.Entities;

namespace Sheep.ServiceModel.ChapterReads
{
    /// <summary>
    ///     显示一个阅读的请求。
    /// </summary>
    [Route("/chapterreads/{ChapterReadId}", HttpMethods.Get, Summary = "显示一个阅读")]
    [DataContract]
    public class ChapterReadShow : IReturn<ChapterReadShowResponse>
    {
        /// <summary>
        ///     阅读的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "阅读编号")]
        public string ChapterReadId { get; set; }
    }

    /// <summary>
    ///     显示最后一个阅读的请求。
    /// </summary>
    [Route("/chapterreads/last", HttpMethods.Get, Summary = "显示最后一个阅读")]
    [DataContract]
    public class ChapterReadShowLast : IReturn<ChapterReadShowResponse>
    {
        /// <summary>
        ///     书籍编号。
        /// </summary>
        [DataMember(Order = 1, Name = "bookid")]
        [ApiMember(Description = "书籍编号")]
        public string BookId { get; set; }
    }

    /// <summary>
    ///     根据章显示最后一个阅读的请求。
    /// </summary>
    [Route("/chapterreads/last/bychapter", HttpMethods.Get, Summary = "根据章显示最后一个阅读")]
    [DataContract]
    public class ChapterReadShowLastByChapter : IReturn<ChapterReadShowResponse>
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
    }

    /// <summary>
    ///     显示一个阅读的响应。
    /// </summary>
    [DataContract]
    public class ChapterReadShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     阅读信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "阅读信息")]
        public ChapterReadDto ChapterRead { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}