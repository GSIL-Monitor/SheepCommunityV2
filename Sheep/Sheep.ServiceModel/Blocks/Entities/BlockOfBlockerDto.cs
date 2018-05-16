using System.Runtime.Serialization;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Blocks.Entities
{
    /// <summary>
    ///     屏蔽者的屏蔽信息。
    /// </summary>
    [DataContract]
    public class BlockOfBlockerDto
    {
        /// <summary>
        ///     屏蔽者。
        /// </summary>
        [DataMember(Order = 1)]
        public UserDto Blocker { get; set; }

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