﻿using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Sina.Weibo
{
    /// <summary>
    ///     获取省份列表的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: GET
    ///     https://api.weibo.com/2/common/get_province.json
    /// </remarks>
    [DataContract]
    public class GetProvinceRequest
    {
        /// <summary>
        ///     授权时生成的接口调用凭证。
        /// </summary>
        [DataMember(Order = 1, Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        ///     国家的代码。
        /// </summary>
        [DataMember(Order = 2, Name = "country")]
        public string Country { get; set; }

        /// <summary>
        ///     省份的首字母，a-z，可为空代表返回全部，默认为全部。
        /// </summary>
        [DataMember(Order = 3, Name = "capital")]
        public string Capital { get; set; }

        /// <summary>
        ///     返回的语言版本，zh-cn：简体中文、zh-tw：繁体中文、english：英文，默认为zh-cn。
        /// </summary>
        [DataMember(Order = 4, Name = "language")]
        public string Language { get; set; }

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("access_token=");
            builder.Append(AccessToken);
            builder.Append("&country=");
            builder.Append(Country);
            if (!Capital.IsNullOrEmpty())
            {
                builder.Append("&capital=");
                builder.Append(Capital);
            }
            if (!Language.IsNullOrEmpty())
            {
                builder.Append("&language=");
                builder.Append(Language);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }
    }
}