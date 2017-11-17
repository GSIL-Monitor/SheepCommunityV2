using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Posts.Entities
{
    /// <summary>
    ///     帖子信息。
    /// </summary>
    [DataContract]
    public class PostDto : IHasIntId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public int Id { get; set; }

        /// <summary>
        ///     类型。
        /// </summary>
        [DataMember(Order = 2)]
        public string Type { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 3)]
        public string Title { get; set; }

        /// <summary>
        ///     概要。
        /// </summary>
        [DataMember(Order = 4)]
        public string Summary { get; set; }

        /// <summary>
        ///     图片的地址。
        /// </summary>
        [DataMember(Order = 5)]
        public string PictureUrl { get; set; }

        /// <summary>
        ///     博客编号。
        /// </summary>
        [DataMember(Order = 6)]
        public string BlogId { get; set; }

        /// <summary>
        ///     群组编号。
        /// </summary>
        [DataMember(Order = 7)]
        public string GroupId { get; set; }

        /// <summary>
        ///     内容的类型。（可选值：图文, 音频, 视频）
        /// </summary>
        [DataMember(Order = 8)]
        public string ContentType { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        [DataMember(Order = 9)]
        public string Content { get; set; }

        /// <summary>
        ///     内容的地址。（当类型为音频或视频时，填写音频或视频的地址）
        /// </summary>
        [DataMember(Order = 10)]
        public string ContentUrl { get; set; }

        /// <summary>
        ///     分类的标签列表。
        /// </summary>
        [DataMember(Order = 11)]
        public List<string> Tags { get; set; }

        /// <summary>
        ///     状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
        /// </summary>
        [DataMember(Order = 12)]
        public string Status { get; set; }

        /// <summary>
        ///     禁止的原因。（可选值：垃圾营销, 不实信息, 违法信息, 有害信息, 淫秽色情, 欺诈骗局, 冒充他人, 抄袭内容, 人身攻击, 泄露隐私）
        /// </summary>
        [DataMember(Order = 13)]
        public string BanReason { get; set; }

        /// <summary>
        ///     禁止的取消日期。
        /// </summary>
        [DataMember(Order = 14)]
        public long? BannedUntilDate { get; set; }

        /// <summary>
        ///     是否已发布。
        /// </summary>
        [DataMember(Order = 15)]
        public bool IsPublished { get; set; }

        /// <summary>
        ///     发布日期。
        /// </summary>
        [DataMember(Order = 16)]
        public long? PublishedDate { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 17)]
        public long CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 18)]
        public long ModifiedDate { get; set; }

        /// <summary>
        ///     积分。
        /// </summary>
        [DataMember(Order = 19)]
        public int Points { get; set; }
    }
}