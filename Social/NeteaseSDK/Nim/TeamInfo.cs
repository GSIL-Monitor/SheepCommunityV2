using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     群信息。
    /// </summary>
    [DataContract]
    public class TeamInfo
    {
        #region 属性

        /// <summary>
        ///     网易云通信服务器产生，群唯一标识，创建群时会返回，最大长度128字符。
        /// </summary>
        [DataMember(Order = 1, Name = "tid")]
        public string TeamId { get; set; }

        /// <summary>
        ///     群名称，最大长度64字符。
        /// </summary>
        [DataMember(Order = 2, Name = "tname")]
        public string TeamName { get; set; }

        /// <summary>
        ///     群主用户帐号，最大长度32字符。
        /// </summary>
        [DataMember(Order = 3, Name = "owner")]
        public string OwnerAccountId { get; set; }

        /// <summary>
        ///     群公告，最大长度1024字符。
        /// </summary>
        [DataMember(Order = 4, Name = "announcement")]
        public string Announcement { get; set; }

        /// <summary>
        ///     群描述，最大长度512字符。
        /// </summary>
        [DataMember(Order = 5, Name = "intro")]
        public string Intro { get; set; }

        /// <summary>
        ///     群建好后，sdk操作时，0不用验证，1需要验证,2不允许任何人加入。其它返回414。
        /// </summary>
        [DataMember(Order = 6, Name = "joinmode")]
        public int JoinMode { get; set; }

        /// <summary>
        ///     群描述，最大长度512字符。
        /// </summary>
        [DataMember(Order = 7, Name = "maxusers")]
        public int MaxUsers { get; set; }

        /// <summary>
        ///     群成员人数。
        /// </summary>
        [DataMember(Order = 8, Name = "size")]
        public int Size { get; set; }

        /// <summary>
        ///     自定义高级群扩展属性，第三方可以跟据此属性自定义扩展自己的群属性。（建议为json）,最大长度1024字符。
        /// </summary>
        [DataMember(Order = 9, Name = "custom")]
        public string Custom { get; set; }

        /// <summary>
        ///     禁言开关。
        /// </summary>
        [DataMember(Order = 10, Name = "mute")]
        public bool? Mute { get; set; }

        /// <summary>
        ///     ["aaa","bbb"](JSONArray对应的accid，如果解析出错会报414)，一次最多拉200个成员。
        /// </summary>
        [DataMember(Order = 11, Name = "admins")]
        public List<string> AdminAccountIds { get; set; }

        /// <summary>
        ///     ["aaa","bbb"](JSONArray对应的accid，如果解析出错会报414)，一次最多拉200个成员。
        /// </summary>
        [DataMember(Order = 12, Name = "members")]
        public List<string> MemberAccountIds { get; set; }

        #endregion
    }
}