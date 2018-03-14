using System.Runtime.Serialization;

namespace JPush.Push
{
    /// <summary>
    ///     推送请求，以 JSON 格式表达，表示一条推送相关的所有信息。
    /// </summary>
    [DataContract]
    public class PushRequest
    {
        #region 属性

        /// <summary>
        ///     用于防止 api 调用端重试造成服务端的重复推送而定义的一个标识符。
        /// </summary>
        [DataMember(Order = 1, Name = "cid")]
        public string CId { get; set; }

        /// <summary>
        ///     推送平台设置。
        /// </summary>
        [DataMember(Order = 2, Name = "platform", IsRequired = true)]
        public object Platform { get; set; } = "all";

        /// <summary>
        ///     推送设备指定。
        /// </summary>
        [DataMember(Order = 3, Name = "audience", IsRequired = true)]
        public object Audience { get; set; } = "all";

        /// <summary>
        ///     通知内容体。是被推送到客户端的内容。与 message 一起二者必须有其一，可以二者并存。
        /// </summary>
        [DataMember(Order = 4, Name = "notification")]
        public Notification Notification { get; set; }

        /// <summary>
        ///     消息内容体。是被推送到客户端的内容。与 notification 一起二者必须有其一，可以二者并存。
        /// </summary>
        [DataMember(Order = 5, Name = "message")]
        public Message Message { get; set; }

        /// <summary>
        ///     短信渠道补充送达内容体。
        /// </summary>
        [DataMember(Order = 6, Name = "sms_message")]
        public SmsMessage SmsMessage { get; set; }

        /// <summary>
        ///     推送参数。
        /// </summary>
        [DataMember(Order = 7, Name = "options")]
        public Options Options { get; set; }

        #endregion
    }
}