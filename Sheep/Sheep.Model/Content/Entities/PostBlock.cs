using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Content.Entities
{
    /// <summary>
    ///     帖子屏蔽。
    /// </summary>
    public class PostBlock : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     帖子编号。
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        ///     用户编号。
        /// </summary>
        public int BlockerId { get; set; }

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