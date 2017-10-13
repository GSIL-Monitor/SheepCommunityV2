using System;
using System.Runtime.Serialization;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Comments.Entities
{
    /// <summary>
    ///     评论投票信息。
    /// </summary>
    [DataContract]
    public class CommentVoteDto
    {
        /// <summary>
        ///     所属的评论。
        /// </summary>
        [DataMember(Order = 1)]
        public CommentDto Comment { get; set; }

        /// <summary>
        ///     赞成或反对。
        /// </summary>
        [DataMember(Order = 2)]
        public bool Value { get; set; }

        /// <summary>
        ///     评论投票的用户。
        /// </summary>
        [DataMember(Order = 3)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 4)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 5)]
        public DateTime ModifiedDate { get; set; }
    }
}