using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Users.Entities
{
    /// <summary>
    ///     用户信息。
    /// </summary>
    [DataContract]
    public class UserDto : IHasIntId
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
        ///     用户名称。
        /// </summary>
        [DataMember(Order = 3)]
        public string UserName { get; set; }

        /// <summary>
        ///     电子邮件地址。
        /// </summary>
        [DataMember(Order = 4)]
        public string Email { get; set; }

        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 5)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     真实全称姓名。
        /// </summary>
        [DataMember(Order = 6)]
        public string FullName { get; set; }

        /// <summary>
        ///     真实全称姓名是否已通过认证。
        /// </summary>
        [DataMember(Order = 7)]
        public bool FullNameVerified { get; set; }

        /// <summary>
        ///     签名。
        /// </summary>
        [DataMember(Order = 8)]
        public string Signature { get; set; }

        /// <summary>
        ///     简介。
        /// </summary>
        [DataMember(Order = 9)]
        public string Description { get; set; }

        /// <summary>
        ///     头像地址。
        /// </summary>
        [DataMember(Order = 10)]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     封面图像地址。
        /// </summary>
        [DataMember(Order = 11)]
        public string CoverPhotoUrl { get; set; }

        /// <summary>
        ///     出生日期。
        /// </summary>
        [DataMember(Order = 12)]
        public string BirthDate { get; set; }

        /// <summary>
        ///     性别。
        /// </summary>
        [DataMember(Order = 13)]
        public string Gender { get; set; }

        /// <summary>
        ///     站点联系用户的电子邮件。
        /// </summary>
        [DataMember(Order = 14)]
        public string PrimaryEmail { get; set; }

        /// <summary>
        ///     手机号码。
        /// </summary>
        [DataMember(Order = 15)]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     居住国家。
        /// </summary>
        [DataMember(Order = 16)]
        public string Country { get; set; }

        /// <summary>
        ///     居住省份/州。
        /// </summary>
        [DataMember(Order = 17)]
        public string State { get; set; }

        /// <summary>
        ///     居住城市。
        /// </summary>
        [DataMember(Order = 18)]
        public string City { get; set; }

        /// <summary>
        ///     所属教会。
        /// </summary>
        [DataMember(Order = 19)]
        public string Guild { get; set; }

        /// <summary>
        ///     所在公司。
        /// </summary>
        [DataMember(Order = 20)]
        public string Company { get; set; }

        /// <summary>
        ///     居住地址。
        /// </summary>
        [DataMember(Order = 21)]
        public string Address { get; set; }

        /// <summary>
        ///     居住地址二。
        /// </summary>
        [DataMember(Order = 22)]
        public string Address2 { get; set; }

        /// <summary>
        ///     寄件地址。
        /// </summary>
        [DataMember(Order = 23)]
        public string MailAddress { get; set; }

        /// <summary>
        ///     邮政编码。
        /// </summary>
        [DataMember(Order = 24)]
        public string PostalCode { get; set; }

        /// <summary>
        ///     所在时区。
        /// </summary>
        [DataMember(Order = 25)]
        public string TimeZone { get; set; }

        /// <summary>
        ///     显示语言。
        /// </summary>
        [DataMember(Order = 26)]
        public string Language { get; set; }

        /// <summary>
        ///     私信消息的来源。（可选值：None, Friends, Everyone）
        /// </summary>
        [DataMember(Order = 27)]
        public string PrivateMessagesSource { get; set; }

        /// <summary>
        ///     是否接收电子邮件。
        /// </summary>
        [DataMember(Order = 28)]
        public bool? ReceiveEmails { get; set; }

        /// <summary>
        ///     是否接收手机短消息。
        /// </summary>
        [DataMember(Order = 29)]
        public bool? ReceiveSms { get; set; }

        /// <summary>
        ///     是否接收评论通知。
        /// </summary>
        [DataMember(Order = 30)]
        public bool? ReceiveCommentNotifications { get; set; }

        /// <summary>
        ///     是否接收对话通知。
        /// </summary>
        [DataMember(Order = 31)]
        public bool? ReceiveConversationNotifications { get; set; }

        /// <summary>
        ///     是否允许其他用户看到在线状态。
        /// </summary>
        [DataMember(Order = 32)]
        public bool? TrackPresence { get; set; }

        /// <summary>
        ///     是否共享书签。
        /// </summary>
        [DataMember(Order = 33)]
        public bool? ShareBookmarks { get; set; }

        /// <summary>
        ///     帐户状态。（可选值：Approved, Banned, Disapproved, PendingDeletion）
        /// </summary>
        [DataMember(Order = 34)]
        public string AccountStatus { get; set; }

        /// <summary>
        ///     帐户禁止的原因。（可选值：Profanity, Advertising, Spam, Aggressive, Politics, Terrorism, Abuse, Porn, Flood, Contraband,
        ///     BadUserName, BadSignature, Other）
        /// </summary>
        [DataMember(Order = 35)]
        public string BanReason { get; set; }

        /// <summary>
        ///     帐户禁止的取消日期。
        /// </summary>
        [DataMember(Order = 36)]
        public string BannedUntil { get; set; }

        /// <summary>
        ///     撰写的内容是否需要管理员审核。
        /// </summary>
        [DataMember(Order = 37)]
        public bool? RequireModeration { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 38)]
        public string CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 39)]
        public string ModifiedDate { get; set; }

        /// <summary>
        ///     锁定日期。
        /// </summary>
        [DataMember(Order = 40)]
        public string LockedDate { get; set; }

        /// <summary>
        ///     积分。
        /// </summary>
        [DataMember(Order = 41)]
        public int Points { get; set; }
    }
}