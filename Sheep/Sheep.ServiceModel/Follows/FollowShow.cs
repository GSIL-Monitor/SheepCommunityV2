using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Follows.Entities;

namespace Sheep.ServiceModel.Follows
{
    /// <summary>
    ///     显示一个关注的请求。
    /// </summary>
    [Route("/follows/{OwnerId}/{FollowerId}", HttpMethods.Get, Summary = "显示一个关注")]
    [DataContract]
    public class FollowShow : IReturn<FollowShowResponse>
    {
        /// <summary>
        ///     被关注者编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "被关注者编号")]
        public int OwnerId { get; set; }

        /// <summary>
        ///     关注者编号。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "关注者编号")]
        public int FollowerId { get; set; }
    }

    /// <summary>
    ///     显示一个关注的响应。
    /// </summary>
    [DataContract]
    public class FollowShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     关注信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "关注信息")]
        public FollowDto Follow { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}