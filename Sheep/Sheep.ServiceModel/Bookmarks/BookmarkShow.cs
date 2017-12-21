using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Bookmarks.Entities;

namespace Sheep.ServiceModel.Bookmarks
{
    /// <summary>
    ///     显示一个收藏的请求。
    /// </summary>
    [Route("/bookmarks/{ParentId}/{UserId}", HttpMethods.Get, Summary = "显示一个收藏")]
    [DataContract]
    public class BookmarkShow : IReturn<BookmarkShowResponse>
    {
        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级编号（如帖子编号）")]
        public string ParentId { get; set; }

        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "用户编号")]
        public int UserId { get; set; }
    }

    /// <summary>
    ///     显示一个收藏的响应。
    /// </summary>
    [DataContract]
    public class BookmarkShowResponse : IHasResponseStatus
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