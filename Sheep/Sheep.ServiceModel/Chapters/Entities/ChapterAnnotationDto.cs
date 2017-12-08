using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Chapters.Entities
{
    /// <summary>
    ///     章注释信息。
    /// </summary>
    [DataContract]
    public class ChapterAnnotationDto : IHasStringId
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
        ///     序号。
        /// </summary>
        [DataMember(Order = 4)]
        public int Number { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 5)]
        public string Title { get; set; }

        /// <summary>
        ///     注释。
        /// </summary>
        [DataMember(Order = 6)]
        public string Annotation { get; set; }
    }
}