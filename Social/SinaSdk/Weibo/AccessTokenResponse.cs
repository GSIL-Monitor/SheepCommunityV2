using System.Runtime.Serialization;

namespace Sina.Weibo
{
    /// <summary>
    ///     获取接口调用凭证的响应。
    /// </summary>
    [DataContract]
    public class AccessTokenResponse : ErrorResponse
    {
        #region 属性

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
        ///     授权用户唯一标识。
        /// </summary>
        [DataMember(Order = 103, Name = "uid")]
        public string UserId { get; set; }

        #endregion
    }
}