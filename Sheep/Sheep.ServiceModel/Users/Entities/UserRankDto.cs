using System.Runtime.Serialization;

namespace Sheep.ServiceModel.Users.Entities
{
    /// <summary>
    ///     用户排行信息。
    /// </summary>
    [DataContract]
    public class UserRankDto
    {
        /// <summary>
        ///     用户。
        /// </summary>
        [DataMember(Order = 1)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     上一次帖子查看次数。
        /// </summary>
        [DataMember(Order = 2)]
        public int LastPostViewsCount { get; set; }

        /// <summary>
        ///     上一次帖子查看次数排名。
        /// </summary>
        [DataMember(Order = 3)]
        public int LastPostViewsRank { get; set; }

        /// <summary>
        ///     帖子查看次数。
        /// </summary>
        [DataMember(Order = 4)]
        public int PostViewsCount { get; set; }

        /// <summary>
        ///     帖子查看次数排名。
        /// </summary>
        [DataMember(Order = 5)]
        public int PostViewsRank { get; set; }

        /// <summary>
        ///     上一次节查看次数。
        /// </summary>
        [DataMember(Order = 6)]
        public int LastParagraphViewsCount { get; set; }

        /// <summary>
        ///     上一次节查看次数排名。
        /// </summary>
        [DataMember(Order = 7)]
        public int LastParagraphViewsRank { get; set; }

        /// <summary>
        ///     节查看次数。
        /// </summary>
        [DataMember(Order = 8)]
        public int ParagraphViewsCount { get; set; }

        /// <summary>
        ///     节查看次数排名。
        /// </summary>
        [DataMember(Order = 9)]
        public int ParagraphViewsRank { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 10)]
        public long CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 11)]
        public long ModifiedDate { get; set; }
    }
}