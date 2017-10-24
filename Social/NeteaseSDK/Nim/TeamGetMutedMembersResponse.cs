using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     获取群组禁言列表的响应。
    /// </summary>
    [DataContract]
    public class TeamGetMutedMembersResponse : ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     群成员列表信息。
        /// </summary>
        [DataMember(Order = 101, Name = "mutes")]
        public List<TeamMemberInfo> TeamMemberInfos { get; set; }

        #endregion
    }
}