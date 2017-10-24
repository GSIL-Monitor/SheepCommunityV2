using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     获取某用户所加入的群信息的响应。
    /// </summary>
    [DataContract]
    public class TeamGetJoinedResponse : ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     加入的群总数。
        /// </summary>
        [DataMember(Order = 101, Name = "count")]
        public int Count { get; set; }

        /// <summary>
        ///     群列表信息。
        /// </summary>
        [DataMember(Order = 102, Name = "infos")]
        public List<TeamInfo> TeamInfos { get; set; }

        #endregion
    }
}