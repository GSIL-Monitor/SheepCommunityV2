using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Achievements.Entities;

namespace Sheep.ServiceModel.Achievements
{
    /// <summary>
    ///     显示一个成就的请求。
    /// </summary>
    [Route("/achievements/{AchievementId}", HttpMethods.Get)]
    [DataContract]
    public class AchievementShow : IReturn<AchievementShowResponse>
    {
        /// <summary>
        ///     成就的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string AchievementId { get; set; }
    }

    /// <summary>
    ///     显示一个成就的响应。
    /// </summary>
    [DataContract]
    public class AchievementShowResponse : IHasResponseStatus
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