using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     使用用户名称或电子邮件地址及密码登录的请求。
    /// </summary>
    [Route("/account/login/credentials", HttpMethods.Post, Summary = "使用用户名称或电子邮件地址及密码登录")]
    [DataContract]
    public class AccountLoginByCredentials : IReturn<AccountLoginResponse>
    {
        /// <summary>
        ///     用户名称或电子邮件地址。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "用户名称或电子邮件地址")]
        public string UserNameOrEmail { get; set; }

        /// <summary>
        ///     登录密码。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "登录密码")]
        public string Password { get; set; }
    }

    /// <summary>
    ///     使用手机号码及验证码登录的请求。
    /// </summary>
    [Route("/account/login/mobile", HttpMethods.Post, Summary = "使用手机号码及验证码登录")]
    [DataContract]
    public class AccountLoginByMobile : IReturn<AccountLoginResponse>
    {
        /// <summary>
        ///     手机号码。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "手机号码")]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     验证码。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "验证码")]
        public string Token { get; set; }
    }

    /// <summary>
    ///     使用微博帐号登录的请求。
    /// </summary>
    [Route("/account/login/weibo", HttpMethods.Post, Summary = "使用微博帐号登录")]
    [DataContract]
    public class AccountLoginByWeibo : IReturn<AccountLoginResponse>
    {
        /// <summary>
        ///     微博的用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "微博的用户编号")]
        public string WeiboUserId { get; set; }

        /// <summary>
        ///     微博的授权码。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "微博的授权码")]
        public string AccessToken { get; set; }
    }

    /// <summary>
    ///     使用微信帐号登录的请求。
    /// </summary>
    [Route("/account/login/weixin", HttpMethods.Post, Summary = "使用微信帐号登录")]
    [DataContract]
    public class AccountLoginByWeixin : IReturn<AccountLoginResponse>
    {
        /// <summary>
        ///     微信的用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "微信的用户编号")]
        public string WeixinUserId { get; set; }

        /// <summary>
        ///     微信的授权码。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "微信的授权码")]
        public string AccessToken { get; set; }
    }

    /// <summary>
    ///     使用QQ帐号登录的请求。
    /// </summary>
    [Route("/account/login/qq", HttpMethods.Post, Summary = "使用QQ帐号登录")]
    [DataContract]
    public class AccountLoginByQQ : IReturn<AccountLoginResponse>
    {
        /// <summary>
        ///     QQ的用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "QQ的用户编号")]
        public string QQUserId { get; set; }

        /// <summary>
        ///     QQ的授权码。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "QQ的授权码")]
        public string AccessToken { get; set; }
    }

    /// <summary>
    ///     登录的响应。
    /// </summary>
    [DataContract]
    public class AccountLoginResponse : IHasResponseStatus
    {
        /// <summary>
        ///     登录会话编号。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "登录会话编号")]
        public string SessionId { get; set; }

        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "用户编号")]
        public int? UserId { get; set; }

        /// <summary>
        ///     是否通过此登录创建了一个新帐户。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "是否通过此登录创建了一个新帐户")]
        public bool? NewlyCreated { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}