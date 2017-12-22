using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Views
{
    /// <summary>
    ///     新建一组节阅读的请求。
    /// </summary>
    [Route("/views/paragraphs", HttpMethods.Post, Summary = "新建一组节阅读")]
    [DataContract]
    public class ViewBatchCreateForParagraphs : IReturn<ViewBatchCreateResponse>
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
        ///     起始章序号。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "起始章序号")]
        public int BeginChapterNumber { get; set; }

        /// <summary>
        ///     起始节序号。
        /// </summary>
        [DataMember(Order = 4, IsRequired = true)]
        [ApiMember(Description = "起始节序号")]
        public int BeginParagraphNumber { get; set; }

        /// <summary>
        ///     结束章序号。
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        [ApiMember(Description = "结束章序号")]
        public int EndChapterNumber { get; set; }

        /// <summary>
        ///     结束节序号。
        /// </summary>
        [DataMember(Order = 6, IsRequired = true)]
        [ApiMember(Description = "结束节序号")]
        public int EndParagraphNumber { get; set; }
    }

    /// <summary>
    ///     新建一组阅读的响应。
    /// </summary>
    [DataContract]
    public class ViewBatchCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}