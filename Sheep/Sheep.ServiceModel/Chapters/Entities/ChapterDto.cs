using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Model;
using Sheep.ServiceModel.Paragraphs.Entities;

namespace Sheep.ServiceModel.Chapters.Entities
{
    /// <summary>
    ///     章信息。
    /// </summary>
    [DataContract]
    public class ChapterDto : IHasStringId
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
        ///     序号。
        /// </summary>
        [DataMember(Order = 3)]
        public int Number { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 4)]
        public string Title { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        [DataMember(Order = 5)]
        public string Content { get; set; }

        /// <summary>
        ///     节数。
        /// </summary>
        [DataMember(Order = 6)]
        public int ParagraphsCount { get; set; }

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
        public List<ChapterAnnotationDto> Annotations { get; set; }

        /// <summary>
        ///     节列表。
        /// </summary>
        [DataMember(Order = 15)]
        public List<ParagraphDto> Paragraphs { get; set; }
    }
}