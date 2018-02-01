using System.Runtime.Serialization;
using ServiceStack.Model;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Feedbacks.Entities
{
    /// <summary>
    ///     反馈信息。
    /// </summary>
    [DataContract]
    public class FeedbackDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     内容。
        /// </summary>
        [DataMember(Order = 2)]
        public string Content { get; set; }

        /// <summary>
        ///     状态。（可选值：待处理, 提交技术, 提交产品, 提交运营, 等待删除）
        /// </summary>
        [DataMember(Order = 3)]
        public string Status { get; set; }

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

        /// <summary>
        ///     反馈的用户。
        /// </summary>
        [DataMember(Order = 6)]
        public BasicUserDto User { get; set; }
    }
}