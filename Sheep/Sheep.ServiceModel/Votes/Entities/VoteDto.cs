using System.Runtime.Serialization;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Votes.Entities
{
    /// <summary>
    ///     投票信息。
    /// </summary>
    [DataContract]
    public class VoteDto
    {
        /// <summary>
        ///     上级类型。（可选值：帖子）
        /// </summary>
        [DataMember(Order = 1)]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 2)]
        public string ParentId { get; set; }

        /// <summary>
        ///     赞成或反对。
        /// </summary>
        [DataMember(Order = 3)]
        public bool Value { get; set; }

        /// <summary>
        ///     用户。
        /// </summary>
        [DataMember(Order = 4)]
        public BasicUserDto User { get; set; }

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