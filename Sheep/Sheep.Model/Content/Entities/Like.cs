using System;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Content.Entities
{
    /// <summary>
    ///     点赞。
    /// </summary>
    public class Like : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     内容的类型。（可选值：帖子）
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     内容编号。
        /// </summary>
        public string ContentId { get; set; }

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