﻿using System.Runtime.Serialization;
using ServiceStack.Model;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Replies.Entities
{
    /// <summary>
    ///     回复信息。
    /// </summary>
    [DataContract]
    public class ReplyDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     上级类型。（可选值：评论）
        /// </summary>
        [DataMember(Order = 2)]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如评论编号）
        /// </summary>
        [DataMember(Order = 3)]
        public string ParentId { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        [DataMember(Order = 4)]
        public string Content { get; set; }

        /// <summary>
        ///     状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
        /// </summary>
        [DataMember(Order = 5)]
        public string Status { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 6)]
        public long CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 7)]
        public long ModifiedDate { get; set; }

        /// <summary>
        ///     回复的用户。
        /// </summary>
        [DataMember(Order = 8)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     所有的投票的次数。
        /// </summary>
        [DataMember(Order = 9)]
        public int VotesCount { get; set; }

        /// <summary>
        ///     赞成的投票的次数。
        /// </summary>
        [DataMember(Order = 10)]
        public int YesVotesCount { get; set; }

        /// <summary>
        ///     反对的投票的次数。
        /// </summary>
        [DataMember(Order = 11)]
        public int NoVotesCount { get; set; }

        /// <summary>
        ///     当前用户是否已投赞成票。
        /// </summary>
        [DataMember(Order = 12)]
        public bool YesVoted { get; set; }

        /// <summary>
        ///     当前用户是否已投反对票。
        /// </summary>
        [DataMember(Order = 13)]
        public bool NoVoted { get; set; }
    }
}