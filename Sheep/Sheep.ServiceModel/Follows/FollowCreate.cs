using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Follows.Entities;

namespace Sheep.ServiceModel.Follows
{
    /// <summary>
    ///     新建关注的请求。
    /// </summary>
    [Route("/follows", HttpMethods.Post)]
    [DataContract]
    public class FollowCreate : IReturn<FollowCreateResponse>
    {
        /// <summary>
        ///     被关注者编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public int FollowingUserId { get; set; }
    }

    /// <summary>
    ///     新建关注的响应。
    /// </summary>
    [DataContract]
    public class FollowCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     关注信息。
        /// </summary>
        [DataMember(Order = 1)]
        public FollowDto Follow { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}