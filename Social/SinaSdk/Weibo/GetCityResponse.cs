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
        #region 属性

        /// <summary>
        ///     城市列表。
        /// </summary>
        [DataMember(Order = 101)]
        public Dictionary<string, string> Cities { get; set; }

        #endregion
    }
}