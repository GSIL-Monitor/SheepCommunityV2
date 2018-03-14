using System.Runtime.Serialization;

namespace JPush.Push
{
    /// <summary>
    ///     通知内容体。是被推送到客户端的内容。与 message 一起二者必须有其一，可以二者并存。
    ///     “通知”对象，是一条推送的实体内容对象之一（另一个是“消息”），是会作为“通知”推送到客户端的。
    ///     其下属属性包含 4 种，3 个平台属性，以及一个 "alert" 属性。
    ///     <see cref="http://docs.jiguang.cn/jpush/server/push/rest_api_v3_push/#notification" />
    /// </summary>
    [DataContract]
    public class Notification
    {
        #region 属性

        /// <summary>
        ///     通知的内容在各个平台上，都可能只有这一个最基本的属性 "alert"。
        ///     这个位置的 "alert" 属性（直接在 notification 对象下），是一个快捷定义，各平台的 alert 信息如果都一样，则可不定义。如果各平台有定义，则覆盖这里的定义。
        /// </summary>
        [DataMember(Order = 1, Name = "alert")]
        public string Alert { get; set; }

        /// <summary>
        ///     Android 平台上的通知。
        /// </summary>
        [DataMember(Order = 2, Name = "android")]
        public NotificationAndroid Android { get; set; }

        /// <summary>
        ///     iOS 平台上 APNs 通知结构。
        /// </summary>
        [DataMember(Order = 3, Name = "ios")]
        public NotificationIOS IOS { get; set; }

        /// <summary>
        ///     Windows Phone 平台上的通知。
        /// </summary>
        [DataMember(Order = 4, Name = "winphone")]
        public NotificationWinPhone WinPhone { get; set; }

        #endregion
    }
}