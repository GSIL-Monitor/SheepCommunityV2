using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Books.Entities;

namespace Sheep.ServiceModel.Books
{
    /// <summary>
    ///     显示一本书籍的请求。
    /// </summary>
    [Route("/books/{BookId}", HttpMethods.Get, Summary = "显示一本书籍")]
    [DataContract]
    public class BookShow : IReturn<BookShowResponse>
    {
        /// <summary>
        ///     书籍的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "书籍编号")]
        public string BookId { get; set; }
    }

    /// <summary>
    ///     显示一本书籍的响应。
    /// </summary>
    [DataContract]
    public class BookShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     书籍信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "书籍信息")]
        public BookDto Book { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}