using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Bookmarks.Entities;

namespace Sheep.ServiceModel.Bookmarks
{
    /// <summary>
    ///     新建一个收藏的请求。
    /// </summary>
    [Route("/bookmarks", HttpMethods.Post, Summary = "新建一个收藏")]
    [DataContract]
    public class BookmarkCreate : IReturn<BookmarkCreateResponse>
    {
        /// <summary>
        ///     上级类型。（可选值：帖子, 章, 节）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级类型（可选值：帖子, 章, 节）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "上级编号（如帖子编号）")]
        public string ParentId { get; set; }
    }

    /// <summary>
    ///     新建一个收藏的响应。
    /// </summary>
    [DataContract]
    public class BookmarkCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     收藏信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "收藏信息")]
        public BookmarkDto Bookmark { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}