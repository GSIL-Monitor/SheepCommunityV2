using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     注册帐户的请求。
    /// </summary>
    [Route("/account", HttpMethods.Post, Summary = "注册帐户")]
    [DataContract]
    public class AccountRegister : IReturn<AccountRegisterResponse>
    {
        /// <summary>
        ///     用户名称。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "用户名称")]
        public string UserName { get; set; }

        /// <summary>
        ///     电子邮件地址。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "电子邮件地址")]
        public string Email { get; set; }

        /// <summary>
        ///     登录密码。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "登录密码")]
        public string Password { get; set; }

        /// <summary>
        ///     注册后是否自动登录。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "注册后是否自动登录")]
        public bool? AutoLogin { get; set; }
    }

    /// <summary>
    ///     注册身份的响应。
    /// </summary>
    [DataContract]
    public class AccountRegisterResponse : IHasResponseStatus
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
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}