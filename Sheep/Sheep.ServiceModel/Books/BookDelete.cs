using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Books
{
    /// <summary>
    ///     删除一个书籍的请求。
    /// </summary>
    [Route("/books/{BookId}", HttpMethods.Delete, Summary = "删除一个书籍")]
    [DataContract]
    public class BookDelete : IReturn<BookDeleteResponse>
    {
        /// <summary>
        ///     书籍编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "书籍编号")]
        public string BookId { get; set; }
    }

    /// <summary>
    ///     删除一个书籍的响应。
    /// </summary>
    [DataContract]
    public class BookDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}