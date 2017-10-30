using System.Runtime.Serialization;
using ServiceStack.Text;

namespace Sina.Weibo
{
    /// <summary>
    ///     根据用户编号获取用户信息的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: GET
    ///     https://api.weibo.com/2/users/show.json
    /// </remarks>
    [DataContract]
    public class ShowUserRequest
    {
        /// <summary>
        ///     授权时生成的接口调用凭证。
        /// </summary>
        [DataMember(Order = 1, Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        ///     需要查询的用户编号。
        /// </summary>
        [DataMember(Order = 2, Name = "uid")]
        public string UserId { get; set; }

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("access_token=");
            builder.Append(AccessToken);
            builder.Append("&uid=");
            builder.Append(UserId);
            return StringBuilderCache.ReturnAndFree(builder);
        }
    }
}