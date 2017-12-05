using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Read.Entities
{
    /// <summary>
    ///     主题。
    /// </summary>
    public class Subject : IHasStringId, IMeta
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
        ///     扩展属性。
        /// </summary>
        public Dictionary<string, string> Meta { get; set; }
    }
}