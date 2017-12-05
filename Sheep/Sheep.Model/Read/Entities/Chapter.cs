using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Read.Entities
{
    /// <summary>
    ///     章。
    /// </summary>
    public class Chapter : IHasStringId, IMeta
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
        ///     序号。
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     节数。
        /// </summary>
        public int ParagraphsCount { get; set; }

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