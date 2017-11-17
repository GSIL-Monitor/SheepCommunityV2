using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Groups.Entities
{
    /// <summary>
    ///     群组信息。
    /// </summary>
    [DataContract]
    public class GroupDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     类型。
        /// </summary>
        [DataMember(Order = 2)]
        public string Type { get; set; }

        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 3)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     真实组织全称。
        /// </summary>
        [DataMember(Order = 4)]
        public string FullName { get; set; }

        /// <summary>
        ///     真实组织全称是否已通过认证。
        /// </summary>
        [DataMember(Order = 5)]
        public bool FullNameVerified { get; set; }

        /// <summary>
        ///     简介。
        /// </summary>
        [DataMember(Order = 6)]
        public string Description { get; set; }

        /// <summary>
        ///     图像网址。
        /// </summary>
        [DataMember(Order = 7)]
        public string IconUrl { get; set; }

        /// <summary>
        ///     封面图像地址。
        /// </summary>
        [DataMember(Order = 8)]
        public string CoverPhotoUrl { get; set; }

        /// <summary>
        ///     关联的第三方编号。
        /// </summary>
        [DataMember(Order = 9)]
        public string RefId { get; set; }

        /// <summary>
        ///     所在国家。
        /// </summary>
        [DataMember(Order = 10)]
        public string Country { get; set; }

        /// <summary>
        ///     所在省份/州。
        /// </summary>
        [DataMember(Order = 11)]
        public string State { get; set; }

        /// <summary>
        ///     所在城市。
        /// </summary>
        [DataMember(Order = 12)]
        public string City { get; set; }

        /// <summary>
        ///     加入群组的方式。（可选值：Direct, RequireVerification, Joinless）
        /// </summary>
        [DataMember(Order = 13)]
        public string JoinMode { get; set; }

        /// <summary>
        ///     非群组成员是否可以访问群组内容。
        /// </summary>
        [DataMember(Order = 14)]
        public bool? IsPublic { get; set; }

        /// <summary>
        ///     是否开启群组消息。
        /// </summary>
        [DataMember(Order = 15)]
        public bool? EnableMessages { get; set; }

        /// <summary>
        ///     状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
        /// </summary>
        [DataMember(Order = 16)]
        public string Status { get; set; }

        /// <summary>
        ///     禁止的原因。（可选值：垃圾营销, 不实信息, 违法信息, 有害信息, 淫秽色情, 欺诈骗局, 冒充他人, 抄袭内容, 人身攻击, 泄露隐私）
        /// </summary>
        [DataMember(Order = 17)]
        public string BanReason { get; set; }

        /// <summary>
        ///     禁止的取消日期。
        /// </summary>
        [DataMember(Order = 18)]
        public long? BannedUntilDate { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 19)]
        public long CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 20)]
        public long ModifiedDate { get; set; }

        /// <summary>
        ///     所有的成员数。
        /// </summary>
        [DataMember(Order = 21)]
        public int TotalMembers { get; set; }
    }
}