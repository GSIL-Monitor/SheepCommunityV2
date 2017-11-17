using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace Sheep.Model.Corp.Entities
{
    /// <summary>
    ///     群组。
    /// </summary>
    public class Group : IMeta
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     显示名称。
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        ///     真实组织全称。
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///     真实组织全称是否已通过认证。
        /// </summary>
        public bool FullNameVerified { get; set; }

        /// <summary>
        ///     简介。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     图标地址。
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        ///     封面图像地址。
        /// </summary>
        public string CoverPhotoUrl { get; set; }

        /// <summary>
        ///     关联的第三方编号。
        /// </summary>
        public string RefId { get; set; }

        /// <summary>
        ///     所在国家。
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        ///     所在省份/州。
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///     所在城市。
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///     所在地址。
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     所在地址二。
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        ///     寄件地址。
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        ///     邮政编码。
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        ///     加入群组的方式。（可选值：Direct, RequireVerification, Joinless）
        /// </summary>
        public string JoinMode { get; set; }

        /// <summary>
        ///     非群组成员是否可以访问群组内容。
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        ///     是否开启群组消息。
        /// </summary>
        public bool EnableMessages { get; set; }

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
        ///     扩展属性。
        /// </summary>
        public Dictionary<string, string> Meta { get; set; }
    }
}