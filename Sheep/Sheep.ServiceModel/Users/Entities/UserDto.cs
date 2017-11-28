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
        ///     用户名称。
        /// </summary>
        [DataMember(Order = 2)]
        public string UserName { get; set; }

        /// <summary>
        ///     电子邮件地址。
        /// </summary>
        [DataMember(Order = 3)]
        public string Email { get; set; }

        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 4)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     真实全称姓名。
        /// </summary>
        [DataMember(Order = 5)]
        public string FullName { get; set; }

        /// <summary>
        ///     真实全称姓名是否已通过认证。
        /// </summary>
        [DataMember(Order = 6)]
        public bool FullNameVerified { get; set; }

        /// <summary>
        ///     签名。
        /// </summary>
        [DataMember(Order = 7)]
        public string Signature { get; set; }

        /// <summary>
        ///     简介。
        /// </summary>
        [DataMember(Order = 8)]
        public string Description { get; set; }

        /// <summary>
        ///     头像地址。
        /// </summary>
        [DataMember(Order = 9)]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     封面图像地址。
        /// </summary>
        [DataMember(Order = 10)]
        public string CoverPhotoUrl { get; set; }

        /// <summary>
        ///     出生日期。
        /// </summary>
        [DataMember(Order = 11)]
        public string BirthDate { get; set; }

        /// <summary>
        ///     性别。
        /// </summary>
        [DataMember(Order = 12)]
        public string Gender { get; set; }

        /// <summary>
        ///     居住国家。
        /// </summary>
        [DataMember(Order = 13)]
        public string Country { get; set; }

        /// <summary>
        ///     居住省份/州。
        /// </summary>
        [DataMember(Order = 14)]
        public string State { get; set; }

        /// <summary>
        ///     居住城市。
        /// </summary>
        [DataMember(Order = 15)]
        public string City { get; set; }

        /// <summary>
        ///     所属教会。
        /// </summary>
        [DataMember(Order = 16)]
        public string Guild { get; set; }

        /// <summary>
        ///     状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
        /// </summary>
        [DataMember(Order = 17)]
        public string Status { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 18)]
        public long CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 19)]
        public long ModifiedDate { get; set; }

        /// <summary>
        ///     锁定日期。
        /// </summary>
        [DataMember(Order = 20)]
        public long? LockedDate { get; set; }

        /// <summary>
        ///     积分。
        /// </summary>
        [DataMember(Order = 21)]
        public int Points { get; set; }
    }
}