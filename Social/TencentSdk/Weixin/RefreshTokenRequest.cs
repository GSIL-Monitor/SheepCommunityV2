using System.Runtime.Serialization;

namespace Tencent.Weixin
{
    /// <summary>
    ///     刷新或续期接口调用凭证使用的请求。
    /// </summary>
    /// <remarks>
    ///     接口说明：
    ///     access_token是调用授权关系接口的调用凭证，由于access_token有效期（目前为2个小时）较短，当access_token超时后，可以使用refresh_token进行刷新，access_token刷新结果有两种：
    ///     1. 若access_token已超时，那么进行refresh_token会获取一个新的access_token，新的超时时间；
    ///     2. 若access_token未超时，那么进行refresh_token不会改变access_token，但超时时间会刷新，相当于续期access_token。
    ///     （refresh_token拥有较长的有效期（30天），当refresh_token失效的后，需要用户重新授权，所以，请开发者在refresh_token即将过期时（如第29天时），进行定时的自动刷新并保存好它。）
    ///     请求方法：
    ///     使用/sns/oauth2/access_token接口获取到的refresh_token进行以下接口调用：
    ///     http请求方式: GET
    ///     https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=APPID&grant_type=refresh_token&refresh_token=REFRESH_TOKEN
    /// </remarks>
    [DataContract]
    public class RefreshTokenRequest
    {
        /// <summary>
        ///     用户刷新接口调用凭证时生成的刷新凭证。
        /// </summary>
        [DataMember(Order = 1, Name = "refresh_token", IsRequired = true)]
        public string RefreshToken { get; set; }

        /// <summary>
        ///     转换成查询字符串格式的文本。
        /// </summary>
        /// <returns>查询字符串格式的文本。</returns>
        public string ToQueryString()
        {
            return string.Format("refresh_token={0}", RefreshToken);
        }
    }
}