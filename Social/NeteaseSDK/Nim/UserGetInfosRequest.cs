using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     获取用户名片的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/user/getUinfos.action
    /// </remarks>
    [DataContract]
    public class UserGetInfosRequest
    {
        #region 属性

        /// <summary>
        ///     用户帐号（例如：JSONArray对应的accid串，如：["zhangsan"]，如果解析出错，会报414）（一次查询最多为200）。
        /// </summary>
        [DataMember(Order = 1, Name = "accids")]
        public List<string> AccountIds { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("accids=");
            builder.Append(AccountIds.ToJson());
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}