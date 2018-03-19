using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.Job.ServiceModel.Groups
{
    /// <summary>
    ///     查询并统计一组群组排行的请求。
    /// </summary>
    [Route("/groups/rank/count", HttpMethods.Put, Summary = "查询并统计一组群组排行信息")]
    [DataContract]
    public class GroupRankCount : IReturn<GroupRankCountResponse>
    {
    }

    /// <summary>
    ///     查询并统计一组群组排行的响应。
    /// </summary>
    [DataContract]
    public class GroupRankCountResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}