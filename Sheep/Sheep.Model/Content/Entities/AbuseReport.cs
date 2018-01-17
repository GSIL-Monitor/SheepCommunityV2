using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Content.Entities
{
    /// <summary>
    ///     举报。
    /// </summary>
    public class AbuseReport : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     上级类型。（可选值：用户, 帖子, 评论, 回复）
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
        ///     状态。（可选值：待处理, 处理中, 已处理, 处理失败, 等待删除）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///     原因。
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        ///     扩展属性。
        /// </summary>
        public Dictionary<string, string> Meta { get; set; }
    }
}