using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改签名的请求。
    /// </summary>
    [Route("/account/signature", HttpMethods.Put)]
    [DataContract]
    public class AccountChangeSignature : IReturn<AccountChangeSignatureResponse>
    {
        /// <summary>
        ///     更改的签名。
        /// </summary>
        [DataMember(Order = 1)]
        public string Signature { get; set; }
    }

    /// <summary>
    ///     更改签名的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeSignatureResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}