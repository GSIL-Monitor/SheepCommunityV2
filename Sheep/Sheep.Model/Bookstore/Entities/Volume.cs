using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Bookstore.Entities
{
    /// <summary>
    ///     卷。
    /// </summary>
    public class Volume : IHasStringId, IMeta
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
        ///     序号。
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     缩写。
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        ///     章数。
        /// </summary>
        public int ChaptersCount { get; set; }

        /// <summary>
        ///     主题数。
        /// </summary>
        public int SubjectsCount { get; set; }

        /// <summary>
        ///     扩展属性。
        /// </summary>
        public Dictionary<string, string> Meta { get; set; }
    }
}