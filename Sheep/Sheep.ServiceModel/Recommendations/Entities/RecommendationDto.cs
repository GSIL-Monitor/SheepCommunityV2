using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Recommendations.Entities
{
    /// <summary>
    ///     推荐信息。
    /// </summary>
    [DataContract]
    public class RecommendationDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     内容类型。（可选值：帖子）
        /// </summary>
        [DataMember(Order = 2)]
        public string ContentType { get; set; }

        /// <summary>
        ///     内容编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 3)]
        public string ContentId { get; set; }

        /// <summary>
        ///     位置。
        /// </summary>
        [DataMember(Order = 4)]
        public int Position { get; set; }

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