using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Chapters.Entities;

namespace Sheep.ServiceModel.Chapters
{
    /// <summary>
    ///     新建一章的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}/chapters", HttpMethods.Post, Summary = "新建一章")]
    [DataContract]
    public class ChapterCreate : IReturn<ChapterCreateResponse>
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
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "卷序号")]
        public int VolumeNumber { get; set; }

        /// <summary>
        ///     章序号。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "章序号")]
        public int ChapterNumber { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 4, IsRequired = true)]
        [ApiMember(Description = "标题")]
        public string Title { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        [DataMember(Order = 5)]
        [ApiMember(Description = "正文内容")]
        public string Content { get; set; }
    }

    /// <summary>
    ///     新建一章的响应。
    /// </summary>
    [DataContract]
    public class ChapterCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     章信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "章信息")]
        public ChapterDto Chapter { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}