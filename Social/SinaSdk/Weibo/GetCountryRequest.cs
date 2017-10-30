using System.Runtime.Serialization;

namespace Sina.Weibo
{
    /// <summary>
    ///     获取国家列表的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: GET
    ///     https://api.weibo.com/2/common/get_country.json
    /// </remarks>
    [DataContract]
    public class GetCountryRequest
    {
        #region 属性

        /// <summary>
        ///     授权时生成的接口调用凭证。
        /// </summary>
        [DataMember(Order = 1, Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        ///     国家的首字母，a-z，可为空代表返回全部，默认为全部。
        /// </summary>
        [DataMember(Order = 2, Name = "capital")]
        public string Capital { get; set; }

        /// <summary>
        ///     返回的语言版本，zh-cn：简体中文、zh-tw：繁体中文、english：英文，默认为zh-cn。
        /// </summary>
        [DataMember(Order = 3, Name = "language")]
        public string Language { get; set; }

        #endregion

        #region 转换

        /// <summary>
        ///     转换成查询字符串格式的文本。
        /// </summary>
        /// <returns>查询字符串格式的文本。</returns>
        public string ToQueryString()
        {
            return string.Format("access_token={0}&capital={1}&language={2}", AccessToken, Capital, Language);
        }

        #endregion
    }
}