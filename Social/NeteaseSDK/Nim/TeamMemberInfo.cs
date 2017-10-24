using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     群成员信息。
    /// </summary>
    [DataContract]
    public class TeamMemberInfo
    {
        #region 属性

        /// <summary>
        ///     网易云通信服务器产生，群唯一标识，创建群时会返回，最大长度128字符。
        /// </summary>
        [DataMember(Order = 1, Name = "tid")]
        public string TeamId { get; set; }

        /// <summary>
        ///     用户帐号，最大长度32字符。
        /// </summary>
        [DataMember(Order = 2, Name = "accid")]
        public string AccountId { get; set; }

        /// <summary>
        ///     群成员名称，最大长度32字符。
        /// </summary>
        [DataMember(Order = 3, Name = "nick")]
        public string NickName { get; set; }

        /// <summary>
        ///     0不用验证，1需要验证,2不允许任何人加入。其它返回414。
        /// </summary>
        [DataMember(Order = 4, Name = "type")]
        public int Type { get; set; }

        #endregion
    }
}