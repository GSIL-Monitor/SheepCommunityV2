using System.Runtime.Serialization;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Follows.Entities
{
    /// <summary>
    ///     被关注者的关注信息。
    /// </summary>
    [DataContract]
    public class FollowOfOwnerDto
    {
        /// <summary>
        ///     被关注者。
        /// </summary>
        [DataMember(Order = 1)]
        public UserDto Owner { get; set; }

        /// <summary>
        ///     是否已双向关注。
        /// </summary>
        [DataMember(Order = 2)]
        public bool IsBidirectional { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 3)]
        public long CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 4)]
        public long ModifiedDate { get; set; }
    }
}