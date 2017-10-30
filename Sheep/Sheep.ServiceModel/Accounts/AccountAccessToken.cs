using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     获取微博授权码的请求。
    /// </summary>
    [Route("/account/accesstoken/weibo", HttpMethods.Get)]
    [DataContract]
    public class AccountAccessTokenForWeibo : IReturn<AccountAccessTokenResponse>
    {
        /// <summary>
        ///     用户调用授权时获得的代码。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string Code { get; set; }
    }

    /// <summary>
    ///     获取授权码的响应。
    /// </summary>
    [DataContract]
    public class AccountAccessTokenResponse : IHasResponseStatus
    {
        /// <summary>
        ///     第三方平台的授权码。
        /// </summary>
        [DataMember(Order = 1)]
        public string AccessToken { get; set; }

        /// <summary>
        ///     第三方平台的用户编号。
        /// </summary>
        [DataMember(Order = 2)]
        public string UserId { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}