using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Read.Entities
{
    /// <summary>
    ///     节。
    /// </summary>
    public class Paragraph : IHasStringId, IMeta
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        ///     书籍编号。
        /// </summary>
        public string BookId { get; set; }

        /// <summary>
        ///     卷编号。
        /// </summary>
        public string VolumeId { get; set; }

        /// <summary>
        ///     卷序号。
        /// </summary>
        public int VolumeNumber { get; set; }

        /// <summary>
        ///     章编号。
        /// </summary>
        public string ChapterId { get; set; }

        /// <summary>
        ///     章序号。
        /// </summary>
        public int ChapterNumber { get; set; }

        /// <summary>
        ///     主题编号。
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        ///     主题序号。
        /// </summary>
        public int? SubjectNumber { get; set; }

        /// <summary>
        ///     序号。
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     查看的次数。
        /// </summary>
        public int ViewsCount { get; set; }

        /// <summary>
        ///     收藏的次数。
        /// </summary>
        public int BookmarksCount { get; set; }

        /// <summary>
        ///     评论的次数。
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        ///     点赞的次数。
        /// </summary>
        public int LikesCount { get; set; }

        /// <summary>
        ///     评分的次数。
        /// </summary>
        public int RatingsCount { get; set; }

        /// <summary>
        ///     评分的平均值。
        /// </summary>
        public float RatingsAverageValue { get; set; }

        /// <summary>
        ///     分享的次数。
        /// </summary>
        public int SharesCount { get; set; }

        /// <summary>
        ///     扩展属性。
        /// </summary>
        public Dictionary<string, string> Meta { get; set; }
    }
}