using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改帐户密码的请求。
    /// </summary>
    [Route("/account/password", HttpMethods.Put, Summary = "更改帐户密码")]
    [DataContract]
    public class AccountChangePassword : IReturn<AccountChangePasswordResponse>
    {
        /// <summary>
        ///     更改的密码。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "更改的密码")]
        public string Password { get; set; }
    }

    /// <summary>
    ///     更改帐户密码的响应。
    /// </summary>
    [DataContract]
    public class AccountChangePasswordResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}