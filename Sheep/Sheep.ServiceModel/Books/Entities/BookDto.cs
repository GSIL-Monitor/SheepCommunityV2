using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Books.Entities
{
    /// <summary>
    ///     书籍信息。
    /// </summary>
    [DataContract]
    public class BookDto : IHasStringId
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
        ///     作者。
        /// </summary>
        [DataMember(Order = 5)]
        public string Author { get; set; }

        /// <summary>
        ///     分类的标签列表。
        /// </summary>
        [DataMember(Order = 6)]
        public List<string> Tags { get; set; }

        /// <summary>
        ///     是否已发布。
        /// </summary>
        [DataMember(Order = 7)]
        public bool IsPublished { get; set; }

        /// <summary>
        ///     发布日期。
        /// </summary>
        [DataMember(Order = 8)]
        public long? PublishedDate { get; set; }

        /// <summary>
        ///     卷数。
        /// </summary>
        [DataMember(Order = 9)]
        public int VolumesCount { get; set; }

        /// <summary>
        ///     收藏的次数。
        /// </summary>
        [DataMember(Order = 10)]
        public int BookmarksCount { get; set; }

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
    }
}