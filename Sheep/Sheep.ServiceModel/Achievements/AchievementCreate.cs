using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Achievements.Entities;

namespace Sheep.ServiceModel.Achievements
{
    /// <summary>
    ///     新建成就的请求。
    /// </summary>
    [Route("/achievements", HttpMethods.Post)]
    [DataContract]
    public class AchievementCreate : IReturn<AchievementCreateResponse>
    {
        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        ///     说明。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string Criteria { get; set; }

        /// <summary>
        ///     徽章图标的名称。
        /// </summary>
        [DataMember(Order = 3)]
        public string BadgeIconName { get; set; }
    }

    /// <summary>
    ///     新建成就的响应。
    /// </summary>
    [DataContract]
    public class AchievementCreateResponse : IHasResponseStatus
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