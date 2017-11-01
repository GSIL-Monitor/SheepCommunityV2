using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sina.Weibo
{
    /// <summary>
    ///     获取国家列表的响应。
    /// </summary>
    [DataContract]
    public class GetCountryResponse : ErrorResponse
    {
        /// <summary>
        ///     国家列表。
        /// </summary>
        [DataMember(Order = 101, Name = "countries")]
        public Dictionary<string, string> Countries { get; set; }
    }
}