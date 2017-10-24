using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.SecurityTokens
{
    /// <summary>
    ///     校验验证码的请求。
    /// </summary>
    [Route("/tokens/verify", HttpMethods.Post)]
    [DataContract]
    public class SecurityTokenVerify : IReturn<SecurityTokenVerifyResponse>
    {
        /// <summary>
        ///     手机号码。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     用途。（可选的值：Login, Register, Bind, ResetPassword）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string Purpose { get; set; }

        /// <summary>
        ///     验证码。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        public string Token { get; set; }
    }

    /// <summary>
    ///     校验验证码的响应。
    /// </summary>
    [DataContract]
    public class SecurityTokenVerifyResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}