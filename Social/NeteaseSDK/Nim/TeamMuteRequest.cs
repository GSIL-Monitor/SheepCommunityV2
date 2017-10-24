using System.Runtime.Serialization;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     将群组整体禁言的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/muteTlistAll.action
    /// </remarks>
    [DataContract]
    public class TeamMuteRequest
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
        ///     true:禁言，false:解禁。
        /// </summary>
        [DataMember(Order = 3, Name = "mute")]
        public bool Mute { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("tid=");
            builder.Append(TeamId);
            builder.Append("&owner=");
            builder.Append(OwnerAccountId);
            builder.Append("&mute=");
            builder.Append(Mute);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}