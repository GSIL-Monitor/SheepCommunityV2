using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     退出登录身份的请求。
    /// </summary>
    [Route("/accounts/logout", HttpMethods.Post)]
    [DataContract]
    public class AccountLogout : IReturn<AccountLogoutResponse>
    {
    }

    /// <summary>
    ///     退出登录身份的响应。
    /// </summary>
    [DataContract]
    public class AccountLogoutResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}