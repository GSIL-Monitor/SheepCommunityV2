using System.Runtime.Serialization;
using Sheep.ServiceModel.Books.Entities;
using Sheep.ServiceModel.Chapters.Entities;
using Sheep.ServiceModel.Users.Entities;
using Sheep.ServiceModel.Volumes.Entities;

namespace Sheep.ServiceModel.ChapterReads.Entities
{
    /// <summary>
    ///     阅读信息。
    /// </summary>
    [DataContract]
    public class ChapterReadDto
    {
        /// <summary>
        ///     书籍。
        /// </summary>
        [DataMember(Order = 1)]
        public BasicBookDto Book { get; set; }

        /// <summary>
        ///     卷。
        /// </summary>
        [DataMember(Order = 2)]
        public BasicVolumeDto Volume { get; set; }

        /// <summary>
        ///     章。
        /// </summary>
        [DataMember(Order = 3)]
        public BasicChapterDto Chapter { get; set; }

        /// <summary>
        ///     用户。
        /// </summary>
        [DataMember(Order = 4)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 5)]
        public long CreatedDate { get; set; }
    }
}