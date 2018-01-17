using System.Runtime.Serialization;
using ServiceStack.Model;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.AbuseReports.Entities
{
    /// <summary>
    ///     举报信息。
    /// </summary>
    [DataContract]
    public class AbuseReportDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     上级类型。（可选值：用户, 帖子, 评论, 回复）
        /// </summary>
        [DataMember(Order = 2)]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 3)]
        public string ParentId { get; set; }

        /// <summary>
        ///     状态。（可选值：待处理, 处理中, 已处理, 处理失败, 等待删除）
        /// </summary>
        [DataMember(Order = 4)]
        public string Status { get; set; }

        /// <summary>
        ///     原因。
        /// </summary>
        [DataMember(Order = 5)]
        public string Reason { get; set; }

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
        ///     举报的用户。
        /// </summary>
        [DataMember(Order = 8)]
        public BasicUserDto User { get; set; }
    }
}