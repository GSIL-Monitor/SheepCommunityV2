using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace Sheep.Model.Friendship.Entities
{
    /// <summary>
    ///     关注。
    /// </summary>
    public class Follow : IMeta
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     被关注者的用户编号。
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        ///     关注者的用户编号。
        /// </summary>
        public int FollowerId { get; set; }

        /// <summary>
        ///     是否已双向关注。
        /// </summary>
        public bool IsBidirectional { get; set; }

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