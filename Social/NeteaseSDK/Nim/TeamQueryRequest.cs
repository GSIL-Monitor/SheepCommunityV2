using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     群信息与成员列表查询的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/query.action
    /// </remarks>
    [DataContract]
    public class TeamQueryRequest
    {
        #region 属性

        /// <summary>
        ///     群id列表，如["3083","3084"]。
        /// </summary>
        [DataMember(Order = 1, Name = "tids")]
        public List<string> TeamIds { get; set; }

        /// <summary>
        ///     1表示带上群成员列表，0表示不带群成员列表，只返回群信息。
        /// </summary>
        [DataMember(Order = 2, Name = "ope")]
        public int Operation { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("tids=");
            builder.Append(TeamIds.ToJson());
            builder.Append("&ope=");
            builder.Append(Operation);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}