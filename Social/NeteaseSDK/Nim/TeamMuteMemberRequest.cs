using System.Runtime.Serialization;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     禁言群成员的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/muteTlist.action
    /// </remarks>
    [DataContract]
    public class TeamMuteMemberRequest
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
        ///     要禁言群成员的群成员 accid。
        /// </summary>
        [DataMember(Order = 3, Name = "accid")]
        public string AccountId { get; set; }

        /// <summary>
        ///     1-禁言，0-解禁。
        /// </summary>
        [DataMember(Order = 4, Name = "mute")]
        public int Mute { get; set; }

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
            builder.Append("&mute=");
            builder.Append(Mute);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}