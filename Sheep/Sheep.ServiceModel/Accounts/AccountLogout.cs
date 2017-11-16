using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     退出登录的请求。
    /// </summary>
    [Route("/account/login", HttpMethods.Delete, Summary = "退出登录")]
    [DataContract]
    public class AccountLogout : IReturn<AccountLogoutResponse>
    {
    }

    /// <summary>
    ///     退出登录的响应。
    /// </summary>
    [DataContract]
    public class AccountLogoutResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}