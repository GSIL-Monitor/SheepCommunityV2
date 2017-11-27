using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Content.Entities
{
    /// <summary>
    ///     评论。
    /// </summary>
    public class Comment : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     上级类型。（可选值：帖子）
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
        ///     正文内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///     禁止的原因。（可选值：垃圾营销, 不实信息, 违法信息, 有害信息, 淫秽色情, 欺诈骗局, 冒充他人, 抄袭内容, 人身攻击, 泄露隐私）
        /// </summary>
        public string BanReason { get; set; }

        /// <summary>
        ///     禁止的取消日期。
        /// </summary>
        public DateTime? BannedUntilDate { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        ///     是否为精选。
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        ///     回复的次数。
        /// </summary>
        public int RepliesCount { get; set; }

        /// <summary>
        ///     投票的次数。
        /// </summary>
        public int VotesCount { get; set; }

        /// <summary>
        ///     赞成投票的次数。
        /// </summary>
        public int YesVotesCount { get; set; }

        /// <summary>
        ///     反对投票的次数。
        /// </summary>
        public int NoVotesCount { get; set; }

        /// <summary>
        ///     内容质量的评分。
        /// </summary>
        public float ContentQuality { get; set; }

        /// <summary>
        ///     扩展属性。
        /// </summary>
        public Dictionary<string, string> Meta { get; set; }
    }
}