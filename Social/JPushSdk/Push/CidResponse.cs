using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.Push
{
    /// <summary>
    ///     CID列表查询的响应。
    /// </summary>
    [DataContract]
    public class CidResponse
    {
        #region 属性

        /// <summary>
        ///     CID列表信息。
        /// </summary>
        [DataMember(Order = 1, Name = "cidlist")]
        public List<string> Cids { get; set; }

        #endregion
    }
}