using System.Runtime.Serialization;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Follows.Entities
{
    /// <summary>
    ///     关注信息。
    /// </summary>
    [DataContract]
    public class FollowDto
    {
        /// <summary>
        ///     被关注者。
        /// </summary>
        [DataMember(Order = 1)]
        public BasicUserDto FollowingUser { get; set; }

        /// <summary>
        ///     关注者。
        /// </summary>
        [DataMember(Order = 2)]
        public BasicUserDto Follower { get; set; }

        /// <summary>
        ///     是否已双向关注。
        /// </summary>
        [DataMember(Order = 3)]
        public bool IsBidirectional { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 4)]
        public long CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 5)]
        public long ModifiedDate { get; set; }
    }
}