using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Identities
{
    /// <summary>
    ///     退出登录身份系统的请求。
    /// </summary>
    [Route("/identities/logout", HttpMethods.Post)]
    [DataContract]
    public class IdentityLogout : IReturn<IdentityLogoutResponse>
    {
    }

    /// <summary>
    ///     退出登录身份系统的响应。
    /// </summary>
    [DataContract]
    public class IdentityLogoutResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}