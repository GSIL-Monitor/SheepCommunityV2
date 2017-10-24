using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     创建网易云通信用户帐号的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/user/create.action
    /// </remarks>
    [DataContract]
    public class UserCreateRequest
    {
        #region 属性

        /// <summary>
        ///     用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 1, Name = "accid")]
        public string AccountId { get; set; }

        /// <summary>
        ///     用户帐号昵称，最大长度64字符，用来PUSH推送时显示的昵称。
        /// </summary>
        [DataMember(Order = 2, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     用户帐号头像URL，第三方可选填，最大长度1024。
        /// </summary>
        [DataMember(Order = 3, Name = "icon")]
        public string IconUrl { get; set; }

        /// <summary>
        ///     用户帐号可以指定登录token值，最大长度128字符，并更新，如果未指定，会自动生成token，并在创建成功后返回。
        /// </summary>
        [DataMember(Order = 4, Name = "token")]
        public string Token { get; set; }

        /// <summary>
        ///     json属性，第三方可选填，最大长度1024字符。
        /// </summary>
        [DataMember(Order = 5, Name = "props")]
        public string Properties { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("accid=");
            builder.Append(AccountId);
            if (!Name.IsNullOrEmpty())
            {
                builder.Append("&name=");
                builder.Append(Name);
            }
            if (!IconUrl.IsNullOrEmpty())
            {
                builder.Append("&icon=");
                builder.Append(IconUrl);
            }
            if (!Token.IsNullOrEmpty())
            {
                builder.Append("&token=");
                builder.Append(Token);
            }
            if (!Properties.IsNullOrEmpty())
            {
                builder.Append("&props=");
                builder.Append(Properties);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}