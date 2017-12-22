using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Bookstore.Entities
{
    /// <summary>
    ///     书籍。
    /// </summary>
    public class Book : IHasStringId, IMeta
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     概要。
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        ///     图片的地址。
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        ///     作者。
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///     分类的标签列表。
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        ///     是否已发布。
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        ///     发布日期。
        /// </summary>
        public DateTime? PublishedDate { get; set; }

        /// <summary>
        ///     卷数。
        /// </summary>
        public int VolumesCount { get; set; }

        /// <summary>
        ///     收藏的次数。
        /// </summary>
        public int BookmarksCount { get; set; }

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