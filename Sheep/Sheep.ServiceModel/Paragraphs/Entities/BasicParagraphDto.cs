using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Paragraphs.Entities
{
    /// <summary>
    ///     基本节信息。
    /// </summary>
    [DataContract]
    public class BasicParagraphDto : IHasStringId
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
        ///     卷标题。
        /// </summary>
        [DataMember(Order = 3)]
        public string VolumeTitle { get; set; }

        /// <summary>
        ///     章序号。
        /// </summary>
        [DataMember(Order = 4)]
        public int ChapterNumber { get; set; }

        /// <summary>
        ///     章标题。
        /// </summary>
        [DataMember(Order = 5)]
        public string ChapterTitle { get; set; }

        /// <summary>
        ///     主题序号。
        /// </summary>
        [DataMember(Order = 6)]
        public int? SubjectNumber { get; set; }

        /// <summary>
        ///     序号。
        /// </summary>
        [DataMember(Order = 7)]
        public int Number { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        [DataMember(Order = 8)]
        public string Content { get; set; }
    }
}