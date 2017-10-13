using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Users.Entities
{
    /// <summary>
    ///     在线用户信息。
    /// </summary>
    [DataContract]
    public class OnlineUserDto : IHasIntId
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
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 3)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     头像地址。
        /// </summary>
        [DataMember(Order = 4)]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     性别。
        /// </summary>
        [DataMember(Order = 5)]
        public string Gender { get; set; }

        /// <summary>
        ///     是否在线。
        /// </summary>
        [DataMember(Order = 6)]
        public bool? IsOnline { get; set; }

        /// <summary>
        ///     所在位置。
        /// </summary>
        [DataMember(Order = 7)]
        public string Location { get; set; }
    }
}