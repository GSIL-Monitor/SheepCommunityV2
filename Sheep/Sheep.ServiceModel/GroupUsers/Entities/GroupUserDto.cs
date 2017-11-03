using System.Runtime.Serialization;
using Sheep.ServiceModel.BasicUsers.Entities;
using Sheep.ServiceModel.Groups.Entities;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.GroupUsers.Entities
{
    /// <summary>
    ///     基本用户信息。
    /// </summary>
    [DataContract]
    public class GroupUserDto
    {
        /// <summary>
        ///     群组。
        /// </summary>
        [DataMember(Order = 1)]
        public GroupDto Group { get; set; }

        /// <summary>
        ///     用户。
        /// </summary>
        [DataMember(Order = 2)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     成员的类型。（可选值：Owner, Manager, Member, 默认为 Member）
        /// </summary>
        [DataMember(Order = 3)]
        public string MembershipType { get; set; }
    }
}