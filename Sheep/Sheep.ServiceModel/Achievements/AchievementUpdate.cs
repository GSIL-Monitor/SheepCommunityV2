using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Achievements.Entities;

namespace Sheep.ServiceModel.Achievements
{
    /// <summary>
    ///     更新成就的请求。
    /// </summary>
    [Route("/achievements/{AchievementId}", HttpMethods.Put)]
    [DataContract]
    public class AchievementUpdate : IReturn<AchievementUpdateResponse>
    {
        /// <summary>
        ///     成就的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string AchievementId { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        ///     说明。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        public string Criteria { get; set; }

        /// <summary>
        ///     徽章图标的名称。
        /// </summary>
        [DataMember(Order = 4)]
        public string BadgeIconName { get; set; }

        /// <summary>
        ///     是否已开启。
        /// </summary>
        [DataMember(Order = 5)]
        public bool? IsEnabled { get; set; }
    }

    /// <summary>
    ///     更新成就的响应。
    /// </summary>
    [DataContract]
    public class AchievementUpdateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     成就信息。
        /// </summary>
        [DataMember(Order = 1)]
        public AchievementDto Achievement { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}