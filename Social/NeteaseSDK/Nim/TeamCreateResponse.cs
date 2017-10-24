using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     创建群的响应。
    /// </summary>
    [DataContract]
    public class TeamCreateResponse : ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     网易云通信服务器产生，群唯一标识，创建群时会返回，最大长度128字符。
        /// </summary>
        [DataMember(Order = 101, Name = "tid")]
        public string TeamId { get; set; }

        #endregion
    }
}