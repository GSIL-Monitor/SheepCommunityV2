using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Chapters.Entities;

namespace Sheep.ServiceModel.Chapters
{
    /// <summary>
    ///     显示一章的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}/chapters/{ChapterNumber}", HttpMethods.Get, Summary = "显示一章")]
    [DataContract]
    public class ChapterShow : IReturn<ChapterShowResponse>
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
    }

    /// <summary>
    ///     显示一章的响应。
    /// </summary>
    [DataContract]
    public class ChapterShowResponse : IHasResponseStatus
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