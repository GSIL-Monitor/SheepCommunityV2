using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     创建网易云通信用户帐号的响应。
    /// </summary>
    [DataContract]
    public class UserCreateResponse : ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 101, Name = "accid")]
        public string AccountId { get; set; }

        /// <summary>
        ///     用户帐号昵称，最大长度64字符，用来PUSH推送时显示的昵称。
        /// </summary>
        [DataMember(Order = 102, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     用户帐号可以指定登录token值，最大长度128字符，并更新，如果未指定，会自动生成token，并在创建成功后返回。
        /// </summary>
        [DataMember(Order = 103, Name = "token")]
        public string Token { get; set; }

        #endregion
    }
}