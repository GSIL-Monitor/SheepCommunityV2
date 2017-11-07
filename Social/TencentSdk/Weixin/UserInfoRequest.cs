using System.Runtime.Serialization;

namespace Tencent.Weixin
{
    /// <summary>
    ///     获取用户个人信息的请求。
    /// </summary>
    /// <remarks>
    ///     接口说明：
    ///     此接口用于获取用户个人信息。开发者可通过OpenID来获取用户基本信息。
    ///     特别需要注意的是，如果开发者拥有多个移动应用、网站应用和公众帐号，可通过获取用户基本信息中的unionid来区分用户的唯一性，因为只要是同一个微信开放平台帐号下的移动应用、
    ///     网站应用和公众帐号， 用户的unionid是唯一的。换句话说，同一用户，对同一个微信开放平台下的不同应用，unionid是相同的。
    ///     请注意，在用户修改微信头像后，旧的微信头像URL将会失效，因此开发者应该自己在获取用户信息后，将头像图片保存下来，避免微信头像URL失效后的异常情况。
    ///     请求说明：
    ///     http请求方式: GET
    ///     https://api.weixin.qq.com/sns/userinfo?access_token=ACCESS_TOKEN&openid=OPENID
    /// </remarks>
    [DataContract]
    public class UserInfoRequest
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
        ///     国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语，默认为zh-CN。
        /// </summary>
        [DataMember(Order = 3, Name = "lang", IsRequired = true)]
        public string Language { get; set; }

        /// <summary>
        ///     转换成查询字符串格式的文本。
        /// </summary>
        /// <returns>查询字符串格式的文本。</returns>
        public string ToQueryString()
        {
            return string.Format("access_token={0}&openid={1}&lang={2}", AccessToken, OpenId, Language);
        }
    }
}