using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sina.Weibo
{
    /// <summary>
    ///     获取城市列表的响应。
    /// </summary>
    [DataContract]
    public class GetCityResponse : ErrorResponse
    {
        /// <summary>
        ///     城市列表。
        /// </summary>
        [DataMember(Order = 101, Name = "cities")]
        public Dictionary<string, string> Cities { get; set; }
    }
}