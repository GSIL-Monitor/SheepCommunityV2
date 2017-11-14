using System.Runtime.Serialization;
using ServiceStack.Model;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Follows.Entities
{
    /// <summary>
    ///     关注信息。
    /// </summary>
    [DataContract]
    public class FollowDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     被关注的用户。
        /// </summary>
        [DataMember(Order = 2)]
        public BasicUserDto FollowingUser { get; set; }

        /// <summary>
        ///     关注的用户。
        /// </summary>
        [DataMember(Order = 3)]
        public BasicUserDto Follower { get; set; }

        /// <summary>
        ///     是否已双向关注。
        /// </summary>
        [DataMember(Order = 4)]
        public bool IsBidirectional { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 5)]
        public long CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 6)]
        public long ModifiedDate { get; set; }
    }
}