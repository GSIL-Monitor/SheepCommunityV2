using System;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Content.Entities
{
    /// <summary>
    ///     投票。
    /// </summary>
    public class Vote : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     上级类型。（可选值：评论, 回复）
        /// </summary>
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        ///     用户编号。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     赞成或反对。
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}