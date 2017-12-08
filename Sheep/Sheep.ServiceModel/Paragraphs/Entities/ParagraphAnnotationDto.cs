using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Paragraphs.Entities
{
    /// <summary>
    ///     节注释信息。
    /// </summary>
    [DataContract]
    public class ParagraphAnnotationDto : IHasStringId
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
        ///     节序号。
        /// </summary>
        [DataMember(Order = 4)]
        public int ParagraphNumber { get; set; }

        /// <summary>
        ///     序号。
        /// </summary>
        [DataMember(Order = 5)]
        public int Number { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 6)]
        public string Title { get; set; }

        /// <summary>
        ///     注释。
        /// </summary>
        [DataMember(Order = 7)]
        public string Annotation { get; set; }
    }
}