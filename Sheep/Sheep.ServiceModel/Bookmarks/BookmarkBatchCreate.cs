using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Bookmarks
{
    /// <summary>
    ///     新建一组节收藏的请求。
    /// </summary>
    [Route("/bookmarks/paragraphs", HttpMethods.Post, Summary = "新建一组节收藏")]
    [DataContract]
    public class BookmarkBatchCreateForParagraphs : IReturn<BookmarkBatchCreateResponse>
    {
        /// <summary>
        ///     节编号列表。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "节编号列表")]
        public List<string> ParagraphIds { get; set; }
    }

    /// <summary>
    ///     新建一组收藏的响应。
    /// </summary>
    [DataContract]
    public class BookmarkBatchCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}