using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Paragraphs.Entities;

namespace Sheep.ServiceModel.Paragraphs
{
    /// <summary>
    ///     显示一条节注释的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}/chapters/{ChapterNumber}/paragraphs/{ParagraphNumber}/annotations/{AnnotationNumber}", HttpMethods.Get, Summary = "显示一条节注释")]
    [DataContract]
    public class ParagraphAnnotationShow : IReturn<ParagraphAnnotationShowResponse>
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
        ///     节序号。
        /// </summary>
        [DataMember(Order = 4, IsRequired = true)]
        [ApiMember(Description = "节序号")]
        public int ParagraphNumber { get; set; }

        /// <summary>
        ///     注释序号。
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        [ApiMember(Description = "注释序号")]
        public int AnnotationNumber { get; set; }
    }

    /// <summary>
    ///     显示一条节注释的响应。
    /// </summary>
    [DataContract]
    public class ParagraphAnnotationShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     节注释信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "节注释信息")]
        public ParagraphAnnotationDto ParagraphAnnotation { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}