using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Follows
{
    /// <summary>
    ///     取消关注的请求。
    /// </summary>
    [Route("/follows", HttpMethods.Delete)]
    [DataContract]
    public class FollowDelete : IReturn<FollowDeleteResponse>
    {
        /// <summary>
        ///     被关注者编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public int FollowingUserId { get; set; }
    }

    /// <summary>
    ///     取消关注的响应。
    /// </summary>
    [DataContract]
    public class FollowDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}