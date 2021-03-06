﻿using System.Runtime.Serialization;

namespace Tencent.Weixin
{
    /// <summary>
    ///     获取接口调用凭证的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: GET
    ///     https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code
    /// </remarks>
    [DataContract]
    public class AccessTokenRequest
    {
        /// <summary>
        ///     用户调用授权时获得的代码。
        /// </summary>
        [DataMember(Order = 1, Name = "code", IsRequired = true)]
        public string Code { get; set; }

        /// <summary>
        ///     转换成查询字符串格式的文本。
        /// </summary>
        /// <returns>查询字符串格式的文本。</returns>
        public string ToQueryString()
        {
            return string.Format("code={0}", Code);
        }
    }
}