using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     批量发送点对点自定义系统通知的响应。
    /// </summary>
    [DataContract]
    public class MessageSendBatchAttachResponse : ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     未注册的用户帐号列表。
        /// </summary>
        [DataMember(Order = 101, Name = "unregister")]
        public List<string> UnregisteredAccountIds { get; set; }

        #endregion
    }
}