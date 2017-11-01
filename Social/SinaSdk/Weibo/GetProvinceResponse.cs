using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sina.Weibo
{
    /// <summary>
    ///     获取省份列表的响应。
    /// </summary>
    [DataContract]
    public class GetProvinceResponse : ErrorResponse
    {
        /// <summary>
        ///     省份列表。
        /// </summary>
        [DataMember(Order = 101, Name = "provinces")]
        public Dictionary<string, string> Provinces { get; set; }
    }
}