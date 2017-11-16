using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改帐户签名的请求。
    /// </summary>
    [Route("/account/signature", HttpMethods.Put, Summary = "更改帐户签名")]
    [DataContract]
    public class AccountChangeSignature : IReturn<AccountChangeSignatureResponse>
    {
        /// <summary>
        ///     更改的签名。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "更改的签名")]
        public string Signature { get; set; }
    }

    /// <summary>
    ///     更改帐户签名的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeSignatureResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}