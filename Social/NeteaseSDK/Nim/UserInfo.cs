using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     用户名片。
    /// </summary>
    [DataContract]
    public class UserInfo
    {
        #region 属性

        /// <summary>
        ///     用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 1, Name = "accid")]
        public string AccountId { get; set; }

        /// <summary>
        ///     用户帐号昵称，最大长度64字符，用来PUSH推送时显示的昵称。
        /// </summary>
        [DataMember(Order = 2, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     用户帐号头像URL，第三方可选填，最大长度1024。
        /// </summary>
        [DataMember(Order = 3, Name = "icon")]
        public string IconUrl { get; set; }

        /// <summary>
        ///     用户签名，最大长度256字符。
        /// </summary>
        [DataMember(Order = 4, Name = "sign")]
        public string Signature { get; set; }

        /// <summary>
        ///     用户Email，最大长度64字符。
        /// </summary>
        [DataMember(Order = 5, Name = "email")]
        public string Email { get; set; }

        /// <summary>
        ///     用户生日，最大长度16字符。
        /// </summary>
        [DataMember(Order = 6, Name = "birth")]
        public string BirthDate { get; set; }

        /// <summary>
        ///     用户Mobile，最大长度32字符，只支持国内号码。
        /// </summary>
        [DataMember(Order = 7, Name = "mobile")]
        public string Mobile { get; set; }

        /// <summary>
        ///     用户性别，0表示未知，1表示男，2女表示女，其它会报参数错误。
        /// </summary>
        [DataMember(Order = 8, Name = "gender")]
        public int Gender { get; set; }

        /// <summary>
        ///     用户名片扩展字段，最大长度1024字符，用户可自行扩展，建议封装成JSON字符串。
        /// </summary>
        [DataMember(Order = 9, Name = "ex")]
        public string Extensions { get; set; }

        #endregion
    }
}