using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Volumes.Entities
{
    /// <summary>
    ///     基本卷信息。
    /// </summary>
    [DataContract]
    public class BasicVolumeDto : IHasStringId
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
        ///     章数。
        /// </summary>
        [DataMember(Order = 4)]
        public int ChaptersCount { get; set; }
    }
}