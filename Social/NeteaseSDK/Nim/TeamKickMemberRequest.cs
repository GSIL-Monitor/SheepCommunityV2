using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     踢人出群的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/kick.action
    /// </remarks>
    [DataContract]
    public class TeamKickMemberRequest
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
        ///     被移除人的accid，用户账号，最大长度字符。
        /// </summary>
        [DataMember(Order = 3, Name = "member")]
        public string MemberAccountId { get; set; }

        /// <summary>
        ///     自定义扩展字段，最大长度512。
        /// </summary>
        [DataMember(Order = 4, Name = "attach")]
        public string Attach { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("tid=");
            builder.Append(TeamId);
            builder.Append("&owner=");
            builder.Append(OwnerAccountId);
            builder.Append("&member=");
            builder.Append(MemberAccountId);
            if (!Attach.IsNullOrEmpty())
            {
                builder.Append("&attach=");
                builder.Append(Attach);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}