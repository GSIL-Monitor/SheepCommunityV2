using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     使用用户名称或电子邮件地址及密码注册身份的请求。
    /// </summary>
    [Route("/account", HttpMethods.Post)]
    [DataContract]
    public class AccountRegister : IReturn<AccountRegisterResponse>
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

        /// <summary>
        ///     注册后是否自动登录。
        /// </summary>
        [DataMember(Order = 4)]
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
        public string SessionId { get; set; }

        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 2)]
        public int? UserId { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}