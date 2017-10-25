using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更换密码的请求。
    /// </summary>
    [Route("/accounts/password", HttpMethods.Put)]
    [DataContract]
    public class AccountChangePassword : IReturn<AccountChangePasswordResponse>
    {
        /// <summary>
        ///     更换的密码。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string Password { get; set; }
    }

    /// <summary>
    ///     更换密码的响应。
    /// </summary>
    [DataContract]
    public class AccountChangePasswordResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}