using System.Runtime.Serialization;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     移交群主的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/changeOwner.action
    /// </remarks>
    [DataContract]
    public class TeamChangeOwnerRequest
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
        ///     新群主帐号，最大长度32字符。
        /// </summary>
        [DataMember(Order = 3, Name = "newowner")]
        public string NewOwnerAccountId { get; set; }

        /// <summary>
        ///     1:群主解除群主后离开群，2：群主解除群主后成为普通成员。其它414。
        /// </summary>
        [DataMember(Order = 4, Name = "leave")]
        public int Leave { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("tid=");
            builder.Append(TeamId);
            builder.Append("&owner=");
            builder.Append(OwnerAccountId);
            builder.Append("&newowner=");
            builder.Append(NewOwnerAccountId);
            builder.Append("&leave=");
            builder.Append(Leave);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}