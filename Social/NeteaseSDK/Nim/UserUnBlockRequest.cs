using System.Runtime.Serialization;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     解禁网易云通信用户帐号的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/user/unblock.action
    /// </remarks>
    [DataContract]
    public class UserUnBlockRequest
    {
        #region 属性

        /// <summary>
        ///     用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 1, Name = "accid")]
        public string AccountId { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("accid=");
            builder.Append(AccountId);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}