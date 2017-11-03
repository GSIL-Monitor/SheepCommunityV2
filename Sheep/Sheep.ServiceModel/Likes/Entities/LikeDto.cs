using System;
using System.Runtime.Serialization;
using Sheep.ServiceModel.BasicUsers.Entities;
using Sheep.ServiceModel.Contents.Entities;
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
        ///     类型。
        /// </summary>
        [DataMember(Order = 1)]
        public string Type { get; set; }

        /// <summary>
        ///     点赞标记的内容。
        /// </summary>
        [DataMember(Order = 2)]
        public ContentDto Content { get; set; }

        /// <summary>
        ///     点赞的用户。
        /// </summary>
        [DataMember(Order = 3)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 4)]
        public DateTime CreatedDate { get; set; }
    }
}