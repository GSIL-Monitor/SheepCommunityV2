using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.SecurityTokens
{
    /// <summary>
    ///     请求发送验证码的请求。
    /// </summary>
    [Route("/securitytokens", HttpMethods.Post, Summary = "请求发送验证码")]
    [DataContract]
    public class SecurityTokenRequest : IReturn<SecurityTokenRequestResponse>
    {
        /// <summary>
        ///     手机号码。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "手机号码")]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     验证码用途。（可选的值：Login, Register, Bind, ResetPassword）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "验证码用途（可选的值：Login, Register, Bind, ResetPassword）")]
        public string Purpose { get; set; }
    }

    /// <summary>
    ///     请求发送验证码的响应。
    /// </summary>
    [DataContract]
    public class SecurityTokenRequestResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}