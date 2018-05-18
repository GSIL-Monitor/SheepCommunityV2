using System.Runtime.Serialization;
using Sheep.ServiceModel.Posts.Entities;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.PostBlocks.Entities
{
    /// <summary>
    ///     帖子屏蔽信息。
    /// </summary>
    [DataContract]
    public class PostBlockDto
    {
        /// <summary>
        ///     帖子。
        /// </summary>
        [DataMember(Order = 1)]
        public BasicPostDto Post { get; set; }

        /// <summary>
        ///     屏蔽者。
        /// </summary>
        [DataMember(Order = 2)]
        public BasicUserDto Blocker { get; set; }

        /// <summary>
        ///     原因。
        /// </summary>
        [DataMember(Order = 3)]
        public string Reason { get; set; }

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