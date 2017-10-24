using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     移除管理员的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/removeManager.action
    /// </remarks>
    [DataContract]
    public class TeamRemoveManagerRequest
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
        ///     ["aaa","bbb"](JSONArray对应的accid，如果解析出错会报414)，长度最大1024字符（一次添加最多10个管理员）。
        /// </summary>
        [DataMember(Order = 3, Name = "members")]
        public List<string> MemberAccountIds { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("tid=");
            builder.Append(TeamId);
            builder.Append("&owner=");
            builder.Append(OwnerAccountId);
            builder.Append("&members=");
            builder.Append(MemberAccountIds.ToJson());
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}