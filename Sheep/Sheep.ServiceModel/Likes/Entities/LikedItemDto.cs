using System.Runtime.Serialization;

namespace Sheep.ServiceModel.Likes.Entities
{
    /// <summary>
    ///     被点赞对象信息。
    /// </summary>
    [DataContract]
    public class LikedItemDto
    {
        /// <summary>
        ///     类型。
        /// </summary>
        [DataMember(Order = 1)]
        public string Type { get; set; }

        ///// <summary>
        /////     被点赞对象相关的内容。
        ///// </summary>
        //[DataMember(Order = 2)]
        //public ContentDto Content { get; set; }

        /// <summary>
        ///     点赞的数量。
        /// </summary>
        [DataMember(Order = 3)]
        public int LikesCount { get; set; }
    }
}