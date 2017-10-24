using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     更新好友的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/friend/update.action
    /// </remarks>
    [DataContract]
    public class FriendUpdateRequest
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

        /// <summary>
        ///     给好友增加备注名，限制长度128。
        /// </summary>
        [DataMember(Order = 3, Name = "alias")]
        public string Alias { get; set; }

        /// <summary>
        ///     修改ex字段，限制长度256。
        /// </summary>
        [DataMember(Order = 4, Name = "ex")]
        public string Extensions { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("accid=");
            builder.Append(AccountId);
            builder.Append("&faccid=");
            builder.Append(FriendAccountId);
            if (!Alias.IsNullOrEmpty())
            {
                builder.Append("&alias=");
                builder.Append(Alias);
            }
            if (!Extensions.IsNullOrEmpty())
            {
                builder.Append("&ex=");
                builder.Append(Extensions);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}