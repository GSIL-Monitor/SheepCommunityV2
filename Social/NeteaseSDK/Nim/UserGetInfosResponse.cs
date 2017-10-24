using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     获取用户名片的响应。
    /// </summary>
    [DataContract]
    public class UserGetInfosResponse : ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     用户名片列表。
        /// </summary>
        [DataMember(Order = 101, Name = "uinfos")]
        public List<UserInfo> UserInfos { get; set; }

        #endregion
    }
}