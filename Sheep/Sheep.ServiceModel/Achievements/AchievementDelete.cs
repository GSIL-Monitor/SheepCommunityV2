using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Achievements
{
    /// <summary>
    ///     删除一个成就的请求。
    /// </summary>
    [Route("/achievements/{AchievementId}", HttpMethods.Delete)]
    [DataContract]
    public class AchievementDelete : IReturn<AchievementDeleteResponse>
    {
        /// <summary>
        ///     成就的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string AchievementId { get; set; }
    }

    /// <summary>
    ///     删除一个成就的响应。
    /// </summary>
    [DataContract]
    public class AchievementDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}