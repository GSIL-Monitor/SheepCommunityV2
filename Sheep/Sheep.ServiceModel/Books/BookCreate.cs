using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Books.Entities;

namespace Sheep.ServiceModel.Books
{
    /// <summary>
    ///     新建一本书籍的请求。
    /// </summary>
    [Route("/books", HttpMethods.Post, Summary = "新建一本书籍")]
    [DataContract]
    public class BookCreate : IReturn<BookCreateResponse>
    {
        /// <summary>
        ///     书籍编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "书籍编号")]
        public string BookId { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "标题")]
        public string Title { get; set; }

        /// <summary>
        ///     概要。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "概要")]
        public string Summary { get; set; }

        /// <summary>
        ///     来源图片的地址。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "来源图片的地址")]
        public string SourcePictureUrl { get; set; }

        /// <summary>
        ///     作者。
        /// </summary>
        [DataMember(Order = 5)]
        [ApiMember(Description = "作者")]
        public string Author { get; set; }

        /// <summary>
        ///     分类的标签集合。（使用英文分号分隔）
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "分类的标签集合（使用英文分号分隔）")]
        public string Tags { get; set; }

        /// <summary>
        ///     是否自动发布。（如果不发布则保存为草稿）
        /// </summary>
        [DataMember(Order = 7)]
        [ApiMember(Description = "是否自动发布（如果不发布则保存为草稿）")]
        public bool? AutoPublish { get; set; }
    }

    /// <summary>
    ///     新建一本书籍的响应。
    /// </summary>
    [DataContract]
    public class BookCreateResponse : IHasResponseStatus
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