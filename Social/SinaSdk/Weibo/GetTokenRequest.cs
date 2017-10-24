using System.Runtime.Serialization;

namespace Sina.Weibo
{
    /// <summary>
    ///     获取接口调用凭证的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.weibo.com/oauth2/get_token_info
    /// </remarks>
    [DataContract]
    public class GetTokenRequest
    {
        #region 属性

        /// <summary>
        ///     授权时生成的接口调用凭证。
        /// </summary>
        [DataMember(Order = 1, Name = "access_token")]
        public string AccessToken { get; set; }

        #endregion
    }
}