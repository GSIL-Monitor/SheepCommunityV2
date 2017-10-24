using System.Runtime.Serialization;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     修改消息提醒开关的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/muteTeam.action
    /// </remarks>
    [DataContract]
    public class TeamUpdateMemberMuteRequest
    {
        #region 属性

        /// <summary>
        ///     网易云通信服务器产生，群唯一标识，创建群时会返回，最大长度128字符。
        /// </summary>
        [DataMember(Order = 1, Name = "tid")]
        public string TeamId { get; set; }

        /// <summary>
        ///     要修改消息提醒开关的群成员 accid。
        /// </summary>
        [DataMember(Order = 3, Name = "accid")]
        public string AccountId { get; set; }

        /// <summary>
        ///     1：关闭消息提醒，2：打开消息提醒，其他值无效。
        /// </summary>
        [DataMember(Order = 4, Name = "ope")]
        public int Operation { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("tid=");
            builder.Append(TeamId);
            builder.Append("&accid=");
            builder.Append(AccountId);
            builder.Append("&ope=");
            builder.Append(Operation);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}