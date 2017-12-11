using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Paragraphs.Entities;

namespace Sheep.ServiceModel.Paragraphs
{
    /// <summary>
    ///     新建一节的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}/chapters/{ChapterNumber}/paragraphs", HttpMethods.Post, Summary = "新建一节")]
    [DataContract]
    public class ParagraphCreate : IReturn<ParagraphCreateResponse>
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
        ///     主题序号。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "主题序号")]
        public int? SubjectNumber { get; set; }

        /// <summary>
        ///     节序号。
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        [ApiMember(Description = "节序号")]
        public int ParagraphNumber { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        [DataMember(Order = 6, IsRequired = true)]
        [ApiMember(Description = "正文内容")]
        public string Content { get; set; }
    }

    /// <summary>
    ///     新建一节的响应。
    /// </summary>
    [DataContract]
    public class ParagraphCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     节信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "节信息")]
        public ParagraphDto Paragraph { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}