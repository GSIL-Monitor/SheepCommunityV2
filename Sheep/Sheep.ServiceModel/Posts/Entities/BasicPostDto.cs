using System.Runtime.Serialization;
using ServiceStack.Model;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Posts.Entities
{
    /// <summary>
    ///     基本帖子信息。
    /// </summary>
    [DataContract]
    public class BasicPostDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 2)]
        public string Title { get; set; }

        /// <summary>
        ///     概要。
        /// </summary>
        [DataMember(Order = 3)]
        public string Summary { get; set; }

        /// <summary>
        ///     图片的地址。
        /// </summary>
        [DataMember(Order = 4)]
        public string PictureUrl { get; set; }

        /// <summary>
        ///     内容的类型。（可选值：图文, 音频, 视频）
        /// </summary>
        [DataMember(Order = 5)]
        public string ContentType { get; set; }

        /// <summary>
        ///     发布日期。
        /// </summary>
        [DataMember(Order = 6)]
        public long? PublishedDate { get; set; }

        /// <summary>
        ///     是否为精选。
        /// </summary>
        [DataMember(Order = 7)]
        public bool IsFeatured { get; set; }

        /// <summary>
        ///     作者的用户。
        /// </summary>
        [DataMember(Order = 8)]
        public BasicUserDto Author { get; set; }

        /// <summary>
        ///     评论的次数。
        /// </summary>
        [DataMember(Order = 9)]
        public int CommentsCount { get; set; }

        /// <summary>
        ///     点赞的次数。
        /// </summary>
        [DataMember(Order = 10)]
        public int LikesCount { get; set; }

        /// <summary>
        ///     分享的次数。
        /// </summary>
        [DataMember(Order = 11)]
        public int SharesCount { get; set; }
    }
}