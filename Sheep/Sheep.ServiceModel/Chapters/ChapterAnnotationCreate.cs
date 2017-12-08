using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Chapters.Entities;

namespace Sheep.ServiceModel.Chapters
{
    /// <summary>
    ///     新建一条章注释的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}/chapters/{ChapterNumber}/annotations", HttpMethods.Post, Summary = "新建一条章注释")]
    [DataContract]
    public class ChapterAnnotationCreate : IReturn<ChapterAnnotationCreateResponse>
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
        ///     注释序号。
        /// </summary>
        [DataMember(Order = 4, IsRequired = true)]
        [ApiMember(Description = "注释序号")]
        public int AnnotationNumber { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        [ApiMember(Description = "标题")]
        public string Title { get; set; }

        /// <summary>
        ///     注释。
        /// </summary>
        [DataMember(Order = 6, IsRequired = true)]
        [ApiMember(Description = "注释")]
        public string Annotation { get; set; }
    }

    /// <summary>
    ///     新建一条章注释的响应。
    /// </summary>
    [DataContract]
    public class ChapterAnnotationCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     章注释信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "章注释信息")]
        public ChapterAnnotationDto ChapterAnnotation { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}