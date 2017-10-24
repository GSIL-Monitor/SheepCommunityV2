using System.Runtime.Serialization;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     获取某用户所加入的群信息的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/joinTeams.action
    /// </remarks>
    [DataContract]
    public class TeamGetJoinedRequest
    {
        #region 属性

        /// <summary>
        ///     要查询用户的accid。
        /// </summary>
        [DataMember(Order = 1, Name = "accid")]
        public string AccountId { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("accid=");
            builder.Append(AccountId);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}