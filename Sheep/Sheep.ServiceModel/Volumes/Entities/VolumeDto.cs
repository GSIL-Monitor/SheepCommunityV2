using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Volumes.Entities
{
    /// <summary>
    ///     卷信息。
    /// </summary>
    [DataContract]
    public class VolumeDto : IHasStringId
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

        /// <summary>
        ///     缩写。
        /// </summary>
        [DataMember(Order = 4)]
        public string Abbreviation { get; set; }

        /// <summary>
        ///     章数。
        /// </summary>
        [DataMember(Order = 5)]
        public int ChaptersCount { get; set; }

        /// <summary>
        ///     主题数。
        /// </summary>
        [DataMember(Order = 6)]
        public int SubjectsCount { get; set; }

        /// <summary>
        ///     注释列表。
        /// </summary>
        [DataMember(Order = 7)]
        public List<VolumeAnnotationDto> Annotations { get; set; }
    }
}