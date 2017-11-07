using System.Runtime.Serialization;

namespace Tencent.Weixin
{
    /// <summary>
    ///     刷新或续期接口调用凭证使用的响应。
    /// </summary>
    [DataContract]
    public class RefreshTokenResponse : ErrorResponse
    {
        /// <summary>
        ///     授权时生成的接口调用凭证。
        /// </summary>
        [DataMember(Order = 101, Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        ///     接口调用凭证的超时时间，单位（秒）。
        /// </summary>
        [DataMember(Order = 102, Name = "expires_in")]
        public long ExpireIn { get; set; }

        /// <summary>
        ///     用户刷新接口调用凭证时生成的刷新凭证。
        /// </summary>
        [DataMember(Order = 103, Name = "refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        ///     授权用户唯一标识，对该公众帐号唯一。
        /// </summary>
        [DataMember(Order = 104, Name = "openid")]
        public string OpenId { get; set; }

        /// <summary>
        ///     用户授权的作用域，使用逗号（,）分隔。
        /// </summary>
        [DataMember(Order = 105, Name = "scope")]
        public string Scope { get; set; }
    }
}