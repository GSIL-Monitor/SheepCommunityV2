using System;
using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Achievements.Entities
{
    /// <summary>
    ///     成就信息。
    /// </summary>
    [DataContract]
    public class AchievementDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 2)]
        public string Title { get; set; }

        /// <summary>
        ///     说明。
        /// </summary>
        [DataMember(Order = 3)]
        public string Criteria { get; set; }

        /// <summary>
        ///     徽章图标网址。
        /// </summary>
        [DataMember(Order = 4)]
        public string BadgeIconUrl { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 5)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     是否已开启。
        /// </summary>
        [DataMember(Order = 6)]
        public bool IsEnabled { get; set; }
    }
}