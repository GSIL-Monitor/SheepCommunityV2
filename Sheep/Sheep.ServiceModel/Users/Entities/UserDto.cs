using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Model;
using Sheep.ServiceModel.Contents.Entities;

namespace Sheep.ServiceModel.Users.Entities
{
    /// <summary>
    ///     用户信息。
    /// </summary>
    [DataContract]
    public class UserDto : IHasIntId, IMeta
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
        ///     所属的内容。
        /// </summary>
        [DataMember(Order = 3)]
        public ContentDto Content { get; set; }

        /// <summary>
        ///     用户名称。
        /// </summary>
        [DataMember(Order = 4)]
        public string UserName { get; set; }

        /// <summary>
        ///     用户的内容编号。
        /// </summary>
        [DataMember(Order = 5)]
        public string ContentId { get; set; }

        /// <summary>
        ///     用户的内容类型。
        /// </summary>
        [DataMember(Order = 6)]
        public string ContentType { get; set; }

        // -------------------- 个人 --------------------

        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 7)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     真实名称。
        /// </summary>
        [DataMember(Order = 8)]
        public string FirstName { get; set; }

        /// <summary>
        ///     真实姓氏。
        /// </summary>
        [DataMember(Order = 9)]
        public string LastName { get; set; }

        /// <summary>
        ///     真实全称姓名。
        /// </summary>
        [DataMember(Order = 10)]
        public string FullName { get; set; }

        /// <summary>
        ///     个人简介。
        /// </summary>
        [DataMember(Order = 11)]
        public string Biography { get; set; }

        /// <summary>
        ///     头像地址。
        /// </summary>
        [DataMember(Order = 12)]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     封面图像地址。
        /// </summary>
        [DataMember(Order = 13)]
        public string CoverPhotoUrl { get; set; }

        /// <summary>
        ///     出生日期。
        /// </summary>
        [DataMember(Order = 14)]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        ///     性别。
        /// </summary>
        [DataMember(Order = 15)]
        public string Gender { get; set; }

        /// <summary>
        ///     语言。
        /// </summary>
        [DataMember(Order = 16)]
        public string Language { get; set; }

        // -------------------- 联系 --------------------

        /// <summary>
        ///     电子邮件。
        /// </summary>
        [DataMember(Order = 17)]
        public string Email { get; set; }

        /// <summary>
        ///     站点联系用户的电子邮件。
        /// </summary>
        [DataMember(Order = 18)]
        public string PrimaryEmail { get; set; }

        /// <summary>
        ///     手机号码。
        /// </summary>
        [DataMember(Order = 19)]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     居住国家。
        /// </summary>
        [DataMember(Order = 20)]
        public string Country { get; set; }

        /// <summary>
        ///     居住省份/州。
        /// </summary>
        [DataMember(Order = 21)]
        public string State { get; set; }

        /// <summary>
        ///     居住城市。
        /// </summary>
        [DataMember(Order = 22)]
        public string City { get; set; }

        /// <summary>
        ///     所在公司。
        /// </summary>
        [DataMember(Order = 23)]
        public string Company { get; set; }

        /// <summary>
        ///     所属宗教信仰社团。
        /// </summary>
        [DataMember(Order = 24)]
        public string Culture { get; set; }

        /// <summary>
        ///     居住地址。
        /// </summary>
        [DataMember(Order = 25)]
        public string Address { get; set; }

        /// <summary>
        ///     居住地址二。
        /// </summary>
        [DataMember(Order = 26)]
        public string Address2 { get; set; }

        /// <summary>
        ///     寄件地址。
        /// </summary>
        [DataMember(Order = 27)]
        public string MailAddress { get; set; }

        /// <summary>
        ///     邮政编码。
        /// </summary>
        [DataMember(Order = 28)]
        public string PostalCode { get; set; }

        // -------------------- 管理 --------------------

        /// <summary>
        ///     帐户状态。（可选值：PendingApproval, Approved, Banned, Disapproved, PendingDeletion）
        /// </summary>
        [DataMember(Order = 29)]
        public string AccountStatus { get; set; }

        /// <summary>
        ///     帐户禁止的取消日期。
        /// </summary>
        [DataMember(Order = 30)]
        public DateTime? BannedUntil { get; set; }

        /// <summary>
        ///     帐户禁止的原因。（可选值：Profanity, Advertising, Spam, Aggressive, Politics, Terrorism, Abuse, Porn, Flood, Contraband,
        ///     BadUserName, BadSignature, Other）
        /// </summary>
        [DataMember(Order = 31)]
        public string BanReason { get; set; }

        /// <summary>
        ///     是否需要管理员审核。
        /// </summary>
        [DataMember(Order = 32)]
        public bool? ModerationRequired { get; set; }

        /// <summary>
        ///     是否显示在成员列表中。
        /// </summary>
        [DataMember(Order = 33)]
        public bool? DisplayInMemberList { get; set; }

        /// <summary>
        ///     下次访问是是否强制重新登录。
        /// </summary>
        [DataMember(Order = 34)]
        public bool? ForceLogin { get; set; }

        // -------------------- 日期 --------------------

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 35)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 36)]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        ///     锁定日期。
        /// </summary>
        [DataMember(Order = 37)]
        public DateTime? LockedDate { get; set; }

        /// <summary>
        ///     最后登录日期。
        /// </summary>
        [DataMember(Order = 38)]
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        ///     最后访问日期。
        /// </summary>
        [DataMember(Order = 39)]
        public DateTime? LastVisitDate { get; set; }

        // -------------------- 设置 --------------------

        /// <summary>
        ///     所在时区。
        /// </summary>
        [DataMember(Order = 40)]
        public string TimeZone { get; set; }

        /// <summary>
        ///     是否允许站点联系用户。
        /// </summary>
        [DataMember(Order = 41)]
        public bool? AllowSiteToContact { get; set; }

        /// <summary>
        ///     是否允许站点的合作伙伴联系用户。
        /// </summary>
        [DataMember(Order = 42)]
        public bool? AllowSitePartnersToContact { get; set; }

        /// <summary>
        ///     是否开启接收电子邮件。
        /// </summary>
        [DataMember(Order = 43)]
        public bool? EnableReceiveEmails { get; set; }

        /// <summary>
        ///     是否开启Html格式的电子邮件。
        /// </summary>
        [DataMember(Order = 44)]
        public bool? EnableHtmlEmail { get; set; }

        /// <summary>
        ///     当在线时是否允许其他用户可见。
        /// </summary>
        [DataMember(Order = 45)]
        public bool? EnablePresenceTracking { get; set; }

        /// <summary>
        ///     是否开启评论通知。
        /// </summary>
        [DataMember(Order = 46)]
        public bool? EnableCommentNotifications { get; set; }

        /// <summary>
        ///     是否开启对话通知。
        /// </summary>
        [DataMember(Order = 47)]
        public bool? EnableConversationNotifications { get; set; }

        /// <summary>
        ///     是否开启书签收藏共享。
        /// </summary>
        [DataMember(Order = 48)]
        public bool? EnableFavoriteSharing { get; set; }

        // -------------------- 数值 --------------------

        /// <summary>
        ///     积分。
        /// </summary>
        [DataMember(Order = 49)]
        public int? Points { get; set; }

        /// <summary>
        ///     所有发帖的数量。
        /// </summary>
        [DataMember(Order = 50)]
        public int TotalPostsCount { get; set; }

        // -------------------- 扩展 --------------------

        /// <summary>
        ///     扩展字段。
        /// </summary>
        [DataMember(Order = 51)]
        public Dictionary<string, string> Meta { get; set; }
    }
}