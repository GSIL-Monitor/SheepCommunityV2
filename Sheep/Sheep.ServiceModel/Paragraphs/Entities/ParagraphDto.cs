using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Paragraphs.Entities
{
    /// <summary>
    ///     节信息。
    /// </summary>
    [DataContract]
    public class ParagraphDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     卷序号。
        /// </summary>
        [DataMember(Order = 2)]
        public int VolumeNumber { get; set; }

        /// <summary>
        ///     章序号。
        /// </summary>
        [DataMember(Order = 3)]
        public int ChapterNumber { get; set; }

        /// <summary>
        ///     主题序号。
        /// </summary>
        [DataMember(Order = 4)]
        public int? SubjectNumber { get; set; }

        /// <summary>
        ///     序号。
        /// </summary>
        [DataMember(Order = 5)]
        public int Number { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        [DataMember(Order = 6)]
        public string Content { get; set; }

        /// <summary>
        ///     查看的次数。
        /// </summary>
        [DataMember(Order = 7)]
        public int ViewsCount { get; set; }

        /// <summary>
        ///     收藏的次数。
        /// </summary>
        [DataMember(Order = 8)]
        public int BookmarksCount { get; set; }

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
        ///     评分的次数。
        /// </summary>
        [DataMember(Order = 11)]
        public int RatingsCount { get; set; }

        /// <summary>
        ///     评分的平均值。
        /// </summary>
        [DataMember(Order = 12)]
        public float RatingsAverageValue { get; set; }

        /// <summary>
        ///     分享的次数。
        /// </summary>
        [DataMember(Order = 13)]
        public int SharesCount { get; set; }

        /// <summary>
        ///     注释列表。
        /// </summary>
        [DataMember(Order = 14)]
        public List<ParagraphAnnotationDto> Annotations { get; set; }
    }
}