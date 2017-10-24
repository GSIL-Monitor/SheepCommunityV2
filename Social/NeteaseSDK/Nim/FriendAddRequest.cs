using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     加好友的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/friend/add.action
    /// </remarks>
    [DataContract]
    public class FriendAddRequest
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
        ///     1直接加好友，2请求加好友，3同意加好友，4拒绝加好友。
        /// </summary>
        [DataMember(Order = 3, Name = "type")]
        public int Type { get; set; }

        /// <summary>
        ///     加好友对应的请求消息，第三方组装，最长256字符。
        /// </summary>
        [DataMember(Order = 4, Name = "msg")]
        public string Message { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("accid=");
            builder.Append(AccountId);
            builder.Append("&faccid=");
            builder.Append(FriendAccountId);
            builder.Append("&type=");
            builder.Append(Type);
            if (!Message.IsNullOrEmpty())
            {
                builder.Append("&msg=");
                builder.Append(Message);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}