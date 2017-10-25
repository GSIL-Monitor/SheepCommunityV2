﻿using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     使用用户名称或电子邮件地址及密码登录的请求。
    /// </summary>
    [Route("/accounts/login/credentials", HttpMethods.Post)]
    [DataContract]
    public class AccountLoginByCredentials : IReturn<AccountLoginResponse>
    {
        /// <summary>
        ///     用户名称或电子邮件地址。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string UserNameOrEmail { get; set; }

        /// <summary>
        ///     登录密码。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string Password { get; set; }
    }

    /// <summary>
    ///     使用手机号码及验证码登录的请求。
    /// </summary>
    [Route("/accounts/login/mobile", HttpMethods.Post)]
    [DataContract]
    public class AccountLoginByMobile : IReturn<AccountLoginResponse>
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
    ///     使用微博帐号登录的请求。
    /// </summary>
    [Route("/accounts/login/weibo", HttpMethods.Post)]
    [DataContract]
    public class AccountLoginByWeibo : IReturn<AccountLoginResponse>
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
    ///     使用微信帐号登录的请求。
    /// </summary>
    [Route("/accounts/login/weixin", HttpMethods.Post)]
    [DataContract]
    public class AccountLoginByWeixin : IReturn<AccountLoginResponse>
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
    ///     使用QQ帐号登录的请求。
    /// </summary>
    [Route("/accounts/login/qq", HttpMethods.Post)]
    [DataContract]
    public class AccountLoginByQQ : IReturn<AccountLoginResponse>
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
    ///     登录的响应。
    /// </summary>
    [DataContract]
    public class AccountLoginResponse : IHasResponseStatus
    {
        /// <summary>
        ///     登录会话编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string SessionId { get; set; }

        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 2)]
        public int? UserId { get; set; }

        /// <summary>
        ///     是否通过此登录创建了一个新帐户。
        /// </summary>
        [DataMember(Order = 3)]
        public bool? NewlyCreated { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}