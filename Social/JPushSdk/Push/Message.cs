using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.Push
{
    /// <summary>
    ///     应用内消息。或者称作：自定义消息，透传消息。
    ///     此部分内容不会展示到通知栏上，JPush SDK 收到消息内容后透传给 App。需要 App 自行处理。
    ///     iOS 平台上，此部分内容在推送应用内消息通道（非APNS）获取。Windows Phone 暂时不支持应用内消息。
    /// </summary>
    [DataContract]
    public class Message
    {
        #region 属性

        /// <summary>
        ///     消息内容。
        /// </summary>
        [DataMember(Order = 1, Name = "msg_content", IsRequired = true)]
        public string Content { get; set; }

        /// <summary>
        ///     消息标题。
        /// </summary>
        [DataMember(Order = 2, Name = "title")]
        public string Title { get; set; }

        /// <summary>
        ///     消息内容类型。
        /// </summary>
        [DataMember(Order = 3, Name = "content_type")]
        public string ContentType { get; set; }

        /// <summary>
        ///     可选参数。
        /// </summary>
        [DataMember(Order = 4, Name = "extras")]
        public Dictionary<string, object> Extras { get; set; }

        #endregion
    }
}