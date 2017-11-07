using System.Runtime.Serialization;

namespace Tencent.Weixin
{
    /// <summary>
    ///     检验接口调用凭证是否有效的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: GET
    ///     https://api.weixin.qq.com/sns/auth?access_token=ACCESS_TOKEN&openid=OPENID
    /// </remarks>
    [DataContract]
    public class AuthenticateTokenRequest
    {
        /// <summary>
        ///     授权时生成的接口调用凭证。
        /// </summary>
        [DataMember(Order = 1, Name = "access_token", IsRequired = true)]
        public string AccessToken { get; set; }

        /// <summary>
        ///     授权用户唯一标识，对该公众帐号唯一。
        /// </summary>
        [DataMember(Order = 2, Name = "openid", IsRequired = true)]
        public string OpenId { get; set; }

        /// <summary>
        ///     转换成查询字符串格式的文本。
        /// </summary>
        /// <returns>查询字符串格式的文本。</returns>
        public string ToQueryString()
        {
            return string.Format("access_token={0}&openid={1}", AccessToken, OpenId);
        }
    }
}