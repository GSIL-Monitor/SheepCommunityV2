using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     解除手机号码绑定的请求。
    /// </summary>
    [Route("/account/bindings/mobile", HttpMethods.Delete, Summary = "解除手机号码绑定")]
    [DataContract]
    public class AccountUnbindMobile : IReturn<AccountUnbindResponse>
    {
        /// <summary>
        ///     手机号码。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "手机号码")]
        public string PhoneNumber { get; set; }
    }

    /// <summary>
    ///     解除微博帐号绑定的请求。
    /// </summary>
    [Route("/account/bindings/weibo", HttpMethods.Delete, Summary = "解除微博帐号绑定")]
    [DataContract]
    public class AccountUnbindWeibo : IReturn<AccountUnbindResponse>
    {
        /// <summary>
        ///     微博的用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "微博的用户编号")]
        public string WeiboUserId { get; set; }
    }

    /// <summary>
    ///     解除微信帐号绑定的请求。
    /// </summary>
    [Route("/account/bindings/weixin", HttpMethods.Delete, Summary = "解除微信帐号绑定")]
    [DataContract]
    public class AccountUnbindWeixin : IReturn<AccountUnbindResponse>
    {
        /// <summary>
        ///     微信的用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "微信的用户编号")]
        public string WeixinUserId { get; set; }
    }

    /// <summary>
    ///     解除QQ帐号绑定的请求。
    /// </summary>
    [Route("/account/bindings/qq", HttpMethods.Delete, Summary = "解除QQ帐号绑定")]
    [DataContract]
    public class AccountUnbindQQ : IReturn<AccountUnbindResponse>
    {
        /// <summary>
        ///     QQ的用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "QQ的用户编号")]
        public string QQUserId { get; set; }
    }

    /// <summary>
    ///     绑定的响应。
    /// </summary>
    [DataContract]
    public class AccountUnbindResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}