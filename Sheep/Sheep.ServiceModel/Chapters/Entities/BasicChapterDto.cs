using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Chapters.Entities
{
    /// <summary>
    ///     基本章信息。
    /// </summary>
    [DataContract]
    public class BasicChapterDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     序号。
        /// </summary>
        [DataMember(Order = 2)]
        public int Number { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 3)]
        public string Title { get; set; }
    }
}