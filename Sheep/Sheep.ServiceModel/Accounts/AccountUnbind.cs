using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     解除手机号码及验证码绑定帐户的请求。
    /// </summary>
    [Route("/accounts/binding/mobile", HttpMethods.Delete)]
    [DataContract]
    public class AccountUnbindMobile : IReturn<AccountUnbindResponse>
    {
    }

    /// <summary>
    ///     解除微博帐号绑定帐户的请求。
    /// </summary>
    [Route("/accounts/binding/weibo", HttpMethods.Delete)]
    [DataContract]
    public class AccountUnbindWeibo : IReturn<AccountUnbindResponse>
    {
    }

    /// <summary>
    ///     解除微信帐号绑定帐户的请求。
    /// </summary>
    [Route("/accounts/binding/weixin", HttpMethods.Delete)]
    [DataContract]
    public class AccountUnbindWeixin : IReturn<AccountUnbindResponse>
    {
    }

    /// <summary>
    ///     解除QQ帐号绑定帐户的请求。
    /// </summary>
    [Route("/accounts/binding/qq", HttpMethods.Delete)]
    [DataContract]
    public class AccountUnbindQQ : IReturn<AccountUnbindResponse>
    {
    }

    /// <summary>
    ///     绑定帐户的响应。
    /// </summary>
    [DataContract]
    public class AccountUnbindResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}