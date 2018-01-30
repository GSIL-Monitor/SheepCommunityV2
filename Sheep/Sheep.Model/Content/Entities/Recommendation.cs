using System;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Content.Entities
{
    /// <summary>
    ///     推荐。
    /// </summary>
    public class Recommendation : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     内容类型。（可选值：帖子）
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     内容编号。（如帖子编号）
        /// </summary>
        public string ContentId { get; set; }

        /// <summary>
        ///     位置。
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}