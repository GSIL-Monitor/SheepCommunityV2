using System.Runtime.Serialization;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Follows.Entities
{
    /// <summary>
    ///     被关注者的基本关注信息。
    /// </summary>
    [DataContract]
    public class BasicFollowOfFollowingUserDto
    {
        /// <summary>
        ///     被关注者。
        /// </summary>
        [DataMember(Order = 1)]
        public BasicUserDto FollowingUser { get; set; }

        /// <summary>
        ///     是否已双向关注。
        /// </summary>
        [DataMember(Order = 2)]
        public bool IsBidirectional { get; set; }
    }
}