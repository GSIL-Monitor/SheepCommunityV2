using System.Runtime.Serialization;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     删除好友的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/friend/delete.action
    /// </remarks>
    [DataContract]
    public class FriendDeleteRequest
    {
        #region 属性

        /// <summary>
        ///     发起者用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 1, Name = "accid")]
        public string AccountId { get; set; }

        /// <summary>
        ///     好友用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 1, Name = "faccid")]
        public string FriendAccountId { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("accid=");
            builder.Append(AccountId);
            builder.Append("&faccid=");
            builder.Append(FriendAccountId);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}