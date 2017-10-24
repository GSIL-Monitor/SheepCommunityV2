using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     群信息与成员列表查询的响应。
    /// </summary>
    [DataContract]
    public class TeamQueryResponse : ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     群列表信息。
        /// </summary>
        [DataMember(Order = 101, Name = "tinfos")]
        public List<TeamInfo> TeamInfos { get; set; }

        #endregion
    }
}