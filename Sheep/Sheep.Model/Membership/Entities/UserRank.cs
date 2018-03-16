using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Membership.Entities
{
    /// <summary>
    ///     用户排名。
    /// </summary>
    public class UserRank : IHasIntId, IMeta
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        ///     上一次帖子查看次数。
        /// </summary>
        public int LastPostViewsCount { get; set; }

        /// <summary>
        ///     上一次帖子查看次数排名。
        /// </summary>
        public int LastPostViewsRank { get; set; }

        /// <summary>
        ///     帖子查看次数。
        /// </summary>
        public int PostViewsCount { get; set; }

        /// <summary>
        ///     帖子查看次数排名。
        /// </summary>
        public int PostViewsRank { get; set; }

        /// <summary>
        ///     上一次节查看次数。
        /// </summary>
        public int LastParagraphViewsCount { get; set; }

        /// <summary>
        ///     上一次节查看次数排名。
        /// </summary>
        public int LastParagraphViewsRank { get; set; }

        /// <summary>
        ///     节查看次数。
        /// </summary>
        public int ParagraphViewsCount { get; set; }

        /// <summary>
        ///     节查看次数排名。
        /// </summary>
        public int ParagraphViewsRank { get; set; }

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