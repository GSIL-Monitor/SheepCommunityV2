using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Chapters
{
    /// <summary>
    ///     删除一条章注释的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}/chapters/{ChapterNumber}/annotations/{AnnotationNumber}", HttpMethods.Delete, Summary = "删除一条章注释")]
    [DataContract]
    public class ChapterAnnotationDelete : IReturn<ChapterAnnotationDeleteResponse>
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
    }

    /// <summary>
    ///     删除一条章注释的响应。
    /// </summary>
    [DataContract]
    public class ChapterAnnotationDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}