using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     修改群昵称的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/updateTeamNick.action
    /// </remarks>
    [DataContract]
    public class TeamUpdateMemberNickRequest
    {
        #region 属性

        /// <summary>
        ///     网易云通信服务器产生，群唯一标识，创建群时会返回，最大长度128字符。
        /// </summary>
        [DataMember(Order = 1, Name = "tid")]
        public string TeamId { get; set; }

        /// <summary>
        ///     群主用户帐号，最大长度32字符。
        /// </summary>
        [DataMember(Order = 2, Name = "owner")]
        public string OwnerAccountId { get; set; }

        /// <summary>
        ///     要修改群昵称的群成员 accid。
        /// </summary>
        [DataMember(Order = 3, Name = "accid")]
        public string AccountId { get; set; }

        /// <summary>
        ///     accid 对应的群昵称，最大长度32字符。
        /// </summary>
        [DataMember(Order = 4, Name = "nick")]
        public string NickName { get; set; }

        /// <summary>
        ///     自定义扩展字段，最大长度1024字节。
        /// </summary>
        [DataMember(Order = 5, Name = "custom")]
        public string Custom { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("tid=");
            builder.Append(TeamId);
            builder.Append("&owner=");
            builder.Append(OwnerAccountId);
            builder.Append("&accid=");
            builder.Append(AccountId);
            builder.Append("&nick=");
            builder.Append(NickName);
            if (!Custom.IsNullOrEmpty())
            {
                builder.Append("&custom=");
                builder.Append(Custom);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}