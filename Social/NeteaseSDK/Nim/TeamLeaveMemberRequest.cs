﻿using System.Runtime.Serialization;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     主动退群的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/leave.action
    /// </remarks>
    [DataContract]
    public class TeamLeaveMemberRequest
    {
        #region 属性

        /// <summary>
        ///     网易云通信服务器产生，群唯一标识，创建群时会返回，最大长度128字符。
        /// </summary>
        [DataMember(Order = 1, Name = "tid")]
        public string TeamId { get; set; }

        /// <summary>
        ///     要主动退群的群成员 accid。
        /// </summary>
        [DataMember(Order = 2, Name = "accid")]
        public string AccountId { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("tid=");
            builder.Append(TeamId);
            builder.Append("&accid=");
            builder.Append(AccountId);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}