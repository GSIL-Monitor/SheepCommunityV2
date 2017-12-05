using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Read.Entities
{
    /// <summary>
    ///     章注释。
    /// </summary>
    public class ChapterAnnotation : IHasStringId, IMeta
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
        ///     章编号。
        /// </summary>
        public string ChapterId { get; set; }

        /// <summary>
        ///     序号。
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     注释。
        /// </summary>
        public string Annotation { get; set; }

        /// <summary>
        ///     扩展属性。
        /// </summary>
        public Dictionary<string, string> Meta { get; set; }
    }
}