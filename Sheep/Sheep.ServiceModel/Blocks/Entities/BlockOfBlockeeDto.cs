using System.Runtime.Serialization;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Blocks.Entities
{
    /// <summary>
    ///     被屏蔽者的屏蔽信息。
    /// </summary>
    [DataContract]
    public class BlockOfBlockeeDto
    {
        /// <summary>
        ///     被屏蔽者。
        /// </summary>
        [DataMember(Order = 1)]
        public UserDto Blockee { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 2)]
        public long CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 3)]
        public long ModifiedDate { get; set; }
    }
}