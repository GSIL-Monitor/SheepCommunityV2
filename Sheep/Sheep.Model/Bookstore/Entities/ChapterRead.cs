using System;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Bookstore.Entities
{
    /// <summary>
    ///     章阅读。
    /// </summary>
    public class ChapterRead : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     书籍编号。
        /// </summary>
        public string BookId { get; set; }

        /// <summary>
        ///     卷编号。
        /// </summary>
        public string VolumeId { get; set; }

        /// <summary>
        ///     章编号。
        /// </summary>
        public string ChapterId { get; set; }

        /// <summary>
        ///     用户编号。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}