using System;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Users
{
    /// <summary>
    ///     更新用户的请求。
    /// </summary>
    [Route("/users/{UserId}", HttpMethods.Put)]
    [DataContract]
    public class UserUpdate : IReturn<UserUpdateResponse>
    {
        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public int UserId { get; set; }

        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     真实姓名。
        /// </summary>
        [DataMember(Order = 3)]
        public string FullName { get; set; }

        /// <summary>
        ///     个人签名。
        /// </summary>
        [DataMember(Order = 4)]
        public string Signature { get; set; }

        /// <summary>
        ///     个人简介。
        /// </summary>
        [DataMember(Order = 5)]
        public string Description { get; set; }

        /// <summary>
        ///     出生日期。
        /// </summary>
        [DataMember(Order = 6)]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        ///     性别。
        /// </summary>
        [DataMember(Order = 7)]
        public string Gender { get; set; }

        /// <summary>
        ///     站点联系用户的电子邮件。
        /// </summary>
        [DataMember(Order = 8)]
        public string PrimaryEmail { get; set; }

        /// <summary>
        ///     手机号码。
        /// </summary>
        [DataMember(Order = 9)]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     居住国家。
        /// </summary>
        [DataMember(Order = 10)]
        public string Country { get; set; }

        /// <summary>
        ///     居住省份/州。
        /// </summary>
        [DataMember(Order = 11)]
        public string State { get; set; }

        /// <summary>
        ///     居住城市。
        /// </summary>
        [DataMember(Order = 12)]
        public string City { get; set; }

        /// <summary>
        ///     所属教会。
        /// </summary>
        [DataMember(Order = 13)]
        public string Guild { get; set; }

        /// <summary>
        ///     所在公司。
        /// </summary>
        [DataMember(Order = 14)]
        public string Company { get; set; }

        /// <summary>
        ///     居住地址。
        /// </summary>
        [DataMember(Order = 15)]
        public string Address { get; set; }

        /// <summary>
        ///     居住地址二。
        /// </summary>
        [DataMember(Order = 16)]
        public string Address2 { get; set; }

        /// <summary>
        ///     寄件地址。
        /// </summary>
        [DataMember(Order = 17)]
        public string MailAddress { get; set; }

        /// <summary>
        ///     邮政编码。
        /// </summary>
        [DataMember(Order = 18)]
        public string PostalCode { get; set; }

        /// <summary>
        ///     所在时区。
        /// </summary>
        [DataMember(Order = 19)]
        public string TimeZone { get; set; }

        /// <summary>
        ///     显示语言。
        /// </summary>
        [DataMember(Order = 20)]
        public string Culture { get; set; }

        /// <summary>
        ///     私信消息的来源。（可选值：None, Friends, Everyone）
        /// </summary>
        [DataMember(Order = 21)]
        public string PrivateMessagesSource { get; set; }

        /// <summary>
        ///     是否接收电子邮件。
        /// </summary>
        [DataMember(Order = 22)]
        public bool? ReceiveEmails { get; set; }

        /// <summary>
        ///     是否接收手机短消息。
        /// </summary>
        [DataMember(Order = 23)]
        public bool? ReceiveSms { get; set; }

        /// <summary>
        ///     是否接收评论通知。
        /// </summary>
        [DataMember(Order = 24)]
        public bool? ReceiveCommentNotifications { get; set; }

        /// <summary>
        ///     是否接收对话通知。
        /// </summary>
        [DataMember(Order = 25)]
        public bool? ReceiveConversationNotifications { get; set; }

        /// <summary>
        ///     是否允许其他用户看到在线状态。
        /// </summary>
        [DataMember(Order = 26)]
        public bool? TrackPresence { get; set; }

        /// <summary>
        ///     是否共享书签。
        /// </summary>
        [DataMember(Order = 27)]
        public bool? ShareBookmarks { get; set; }

        /// <summary>
        ///     撰写的内容是否需要管理员审核。
        /// </summary>
        [DataMember(Order = 28)]
        public bool? RequireModeration { get; set; }
    }

    /// <summary>
    ///     更新用户的响应。
    /// </summary>
    [DataContract]
    public class UserUpdateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     用户信息。
        /// </summary>
        [DataMember(Order = 1)]
        public UserDto User { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}