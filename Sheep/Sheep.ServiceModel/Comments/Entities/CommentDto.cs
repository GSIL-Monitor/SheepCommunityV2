using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Model;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Comments.Entities
{
    /// <summary>
    ///     评论信息。
    /// </summary>
    [DataContract]
    public class CommentDto : IHasStringId, IMeta
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     类型。
        /// </summary>
        [DataMember(Order = 2)]
        public string Type { get; set; }

        /// <summary>
        ///     上级编号。
        /// </summary>
        [DataMember(Order = 3)]
        public string ParentId { get; set; }

        ///// <summary>
        /////     所属的内容。
        ///// </summary>
        //[DataMember(Order = 4)]
        //public ContentDto Content { get; set; }

        /// <summary>
        ///     正文。
        /// </summary>
        [DataMember(Order = 5)]
        public string Body { get; set; }

        /// <summary>
        ///     评论的用户。
        /// </summary>
        [DataMember(Order = 6)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 7)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 8)]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        ///     排序顺序。
        /// </summary>
        [DataMember(Order = 9)]
        public int SortOrder { get; set; }

        /// <summary>
        ///     是否已通过审核。
        /// </summary>
        [DataMember(Order = 10)]
        public bool IsApproved { get; set; }

        /// <summary>
        ///     是否为特色评论。
        /// </summary>
        [DataMember(Order = 11)]
        public bool IsFeatured { get; set; }

        /// <summary>
        ///     评论的IP地址。
        /// </summary>
        [DataMember(Order = 12)]
        public string IpAddress { get; set; }

        /// <summary>
        ///     回复的数量。
        /// </summary>
        [DataMember(Order = 13)]
        public int RepliesCount { get; set; }

        /// <summary>
        ///     所有的投票数。
        /// </summary>
        [DataMember(Order = 14)]
        public int TotalVotesCount { get; set; }

        /// <summary>
        ///     赞成的投票数。
        /// </summary>
        [DataMember(Order = 15)]
        public int YesVotesCount { get; set; }

        /// <summary>
        ///     反对的投票数。
        /// </summary>
        [DataMember(Order = 16)]
        public int NoVotesCount { get; set; }

        /// <summary>
        ///     评分。
        /// </summary>
        [DataMember(Order = 17)]
        public int Score { get; set; }

        /// <summary>
        ///     用户是否可以更改。
        /// </summary>
        [DataMember(Order = 18)]
        public bool UserCanModify { get; set; }

        /// <summary>
        ///     用户是否可以删除。
        /// </summary>
        [DataMember(Order = 19)]
        public bool UserCanDelete { get; set; }

        /// <summary>
        ///     扩展字段。
        /// </summary>
        [DataMember(Order = 20)]
        public Dictionary<string, string> Meta { get; set; }
    }
}