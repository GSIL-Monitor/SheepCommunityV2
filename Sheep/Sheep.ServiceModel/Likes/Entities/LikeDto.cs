using System.Runtime.Serialization;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Likes.Entities
{
    /// <summary>
    ///     点赞信息。
    /// </summary>
    [DataContract]
    public class LikeDto
    {
        /// <summary>
        ///     内容的类型。（可选值：帖子）
        /// </summary>
        [DataMember(Order = 1)]
        public string ContentType { get; set; }

        /// <summary>
        ///     内容编号。
        /// </summary>
        [DataMember(Order = 2)]
        public string ContentId { get; set; }

        /// <summary>
        ///     用户。
        /// </summary>
        [DataMember(Order = 3)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 4)]
        public long CreatedDate { get; set; }
    }
}