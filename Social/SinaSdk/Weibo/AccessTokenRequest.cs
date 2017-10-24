using System.Runtime.Serialization;

namespace Sina.Weibo
{
    /// <summary>
    ///     获取接口调用凭证的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.weibo.com/oauth2/access_token
    /// </remarks>
    [DataContract]
    public class AccessTokenRequest
    {
        #region 属性

        /// <summary>
        ///     用户调用授权时获得的代码。
        /// </summary>
        [DataMember(Order = 1, Name = "code")]
        public string Code { get; set; }

        #endregion
    }
}