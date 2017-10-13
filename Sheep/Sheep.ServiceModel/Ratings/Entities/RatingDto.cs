using System;
using System.Runtime.Serialization;
using Sheep.ServiceModel.Contents.Entities;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Ratings.Entities
{
    /// <summary>
    ///     评分信息。
    /// </summary>
    [DataContract]
    public class RatingDto
    {
        /// <summary>
        ///     类型。
        /// </summary>
        [DataMember(Order = 1)]
        public string Type { get; set; }

        /// <summary>
        ///     评分标记的内容。
        /// </summary>
        [DataMember(Order = 2)]
        public ContentDto Content { get; set; }

        /// <summary>
        ///     评分的值。
        /// </summary>
        [DataMember(Order = 3)]
        public float? Value { get; set; }

        /// <summary>
        ///     评分的用户。
        /// </summary>
        [DataMember(Order = 4)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 5)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 6)]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        ///     用户是否可以删除。
        /// </summary>
        [DataMember(Order = 7)]
        public bool UserCanDelete { get; set; }
    }
}