using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Users
{
    /// <summary>
    ///     显示一个用户排行的请求。
    /// </summary>
    [Route("/users/ranks/{UserId}", HttpMethods.Get, Summary = "显示一个用户排行")]
    [DataContract]
    public class UserRankShow : IReturn<UserRankShowResponse>
    {
        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "用户编号")]
        public int UserId { get; set; }
    }

    /// <summary>
    ///     显示一个用户排行的响应。
    /// </summary>
    [DataContract]
    public class UserRankShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     用户排行信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "用户排行信息")]
        public UserRankDto UserRank { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}