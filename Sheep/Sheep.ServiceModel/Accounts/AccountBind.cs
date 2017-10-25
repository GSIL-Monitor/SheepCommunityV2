using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     绑定用户名称或电子邮件地址及密码帐户的请求。
    /// </summary>
    [Route("/accounts/binding/credentials", HttpMethods.Post)]
    [DataContract]
    public class AccountBindCredentials : IReturn<AccountBindResponse>
    {
        /// <summary>
        ///     用户名称。
        /// </summary>
        [DataMember(Order = 1)]
        public string UserName { get; set; }

        /// <summary>
        ///     电子邮件地址。
        /// </summary>
        [DataMember(Order = 2)]
        public string Email { get; set; }

        /// <summary>
        ///     登录密码。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        public string Password { get; set; }
    }

    /// <summary>
    ///     绑定手机号码的请求。
    /// </summary>
    [Route("/accounts/binding/mobile", HttpMethods.Post)]
    [DataContract]
    public class AccountBindMobile : IReturn<AccountBindResponse>
    {
        /// <summary>
        ///     手机号码。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     验证码。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string Token { get; set; }
    }

    /// <summary>
    ///     绑定微博帐号的请求。
    /// </summary>
    [Route("/accounts/binding/weibo", HttpMethods.Post)]
    [DataContract]
    public class AccountBindWeibo : IReturn<AccountBindResponse>
    {
        /// <summary>
        ///     微博的用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string WeiboUserId { get; set; }

        /// <summary>
        ///     微博的授权码。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string AccessToken { get; set; }
    }

    /// <summary>
    ///     绑定微信帐号帐户的请求。
    /// </summary>
    [Route("/accounts/binding/weixin", HttpMethods.Post)]
    [DataContract]
    public class AccountBindWeixin : IReturn<AccountBindResponse>
    {
        /// <summary>
        ///     微信的用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string WeixinUserId { get; set; }

        /// <summary>
        ///     微信的授权码。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string AccessToken { get; set; }
    }

    /// <summary>
    ///     绑定QQ帐号的请求。
    /// </summary>
    [Route("/accounts/binding/qq", HttpMethods.Post)]
    [DataContract]
    public class AccountBindQQ : IReturn<AccountBindResponse>
    {
        /// <summary>
        ///     QQ的用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string QQUserId { get; set; }

        /// <summary>
        ///     QQ的授权码。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string AccessToken { get; set; }
    }

    /// <summary>
    ///     绑定帐户的响应。
    /// </summary>
    [DataContract]
    public class AccountBindResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}