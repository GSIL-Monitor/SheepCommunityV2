using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.Job.ServiceModel.Users
{
    /// <summary>
    ///     查询并统计一组用户排行的请求。
    /// </summary>
    [Route("/users/rank/count", HttpMethods.Put, Summary = "查询并统计一组用户排行信息")]
    [DataContract]
    public class UserRankCount : IReturn<UserRankCountResponse>
    {
    }

    /// <summary>
    ///     查询并统计一组用户排行的响应。
    /// </summary>
    [DataContract]
    public class UserRankCountResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}