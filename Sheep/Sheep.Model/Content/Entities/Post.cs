using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Content.Entities
{
    /// <summary>
    ///     帖子。
    /// </summary>
    public class Post : IHasStringId, IMeta
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     作者编号。
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        ///     群组编号。
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     概要。
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        ///     图片的地址。
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        ///     内容的类型。（可选值：图文, 音频, 视频）
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     内容的地址。（当类型为音频或视频时，填写音频或视频的地址）
        /// </summary>
        public string ContentUrl { get; set; }

        /// <summary>
        ///     分类的标签列表。
        /// </summary>
        public List<string> Tags { get; set; }

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
        ///     是否已发布。
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        ///     发布日期。
        /// </summary>
        public DateTime? PublishedDate { get; set; }

        /// <summary>
        ///     是否为精选。
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        ///     精选开始日期。
        /// </summary>
        public DateTime? FeaturedStartDate { get; set; }

        /// <summary>
        ///     精选结束日期。
        /// </summary>
        public DateTime? FeaturedEndDate { get; set; }

        /// <summary>
        ///     查看的次数。
        /// </summary>
        public int ViewsCount { get; set; }

        /// <summary>
        ///     收藏的次数。
        /// </summary>
        public int BookmarksCount { get; set; }

        /// <summary>
        ///     评论的次数。
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        ///     点赞的次数。
        /// </summary>
        public int LikesCount { get; set; }

        /// <summary>
        ///     评分的次数。
        /// </summary>
        public int RatingsCount { get; set; }

        /// <summary>
        ///     评分的平均值。
        /// </summary>
        public float RatingsAverageValue { get; set; }

        /// <summary>
        ///     分享的次数。
        /// </summary>
        public int SharesCount { get; set; }

        /// <summary>
        ///     滥用举报的次数。
        /// </summary>
        public int AbuseReportsCount { get; set; }

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