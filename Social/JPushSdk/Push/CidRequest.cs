using System.Runtime.Serialization;
using ServiceStack.Text;

namespace JPush.Push
{
    /// <summary>
    ///     CID列表查询的请求。
    /// </summary>
    [DataContract]
    public class CidRequest
    {
        #region 属性

        /// <summary>
        ///     一次获取的总数。
        /// </summary>
        [DataMember(Order = 1, Name = "count")]
        public int Count { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("count=");
            builder.Append(Count);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}