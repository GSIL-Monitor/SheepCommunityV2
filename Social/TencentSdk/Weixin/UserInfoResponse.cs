using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tencent.Weixin
{
    /// <summary>
    ///     获取用户个人信息的响应。
    /// </summary>
    [DataContract]
    public class UserInfoResponse : ErrorResponse
    {
        /// <summary>
        ///     授权用户唯一标识，对该公众帐号唯一。
        /// </summary>
        [DataMember(Order = 101, Name = "openid")]
        public string OpenId { get; set; }

        /// <summary>
        ///     普通用户昵称。
        /// </summary>
        [DataMember(Order = 102, Name = "nickname")]
        public string NickName { get; set; }

        /// <summary>
        ///     普通用户性别，1为男性，2为女性。
        /// </summary>
        [DataMember(Order = 103, Name = "sex")]
        public string Sex { get; set; }

        /// <summary>
        ///     普通用户个人资料填写的省份。
        /// </summary>
        [DataMember(Order = 104, Name = "province")]
        public string Province { get; set; }

        /// <summary>
        ///     普通用户个人资料填写的城市。
        /// </summary>
        [DataMember(Order = 105, Name = "city")]
        public string City { get; set; }

        /// <summary>
        ///     国家，如中国为CN。
        /// </summary>
        [DataMember(Order = 106, Name = "country")]
        public string Country { get; set; }

        /// <summary>
        ///     用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。
        /// </summary>
        [DataMember(Order = 107, Name = "headimgurl")]
        public string HeadImageUrl { get; set; }

        /// <summary>
        ///     用户特权信息，json数组，如微信沃卡用户为（chinaunicom）。
        /// </summary>
        [DataMember(Order = 108, Name = "privilege")]
        public List<string> Privileges { get; set; }

        /// <summary>
        ///     用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的。
        /// </summary>
        [DataMember(Order = 109, Name = "unionid")]
        public string UnionId { get; set; }
    }
}