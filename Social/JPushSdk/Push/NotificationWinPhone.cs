using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.Push
{
    /// <summary>
    ///     Windows Phone 平台上的通知。
    ///     该通知由 JPush 服务器代理向微软的 MPNs 服务器发送，并在 Windows Phone 客户端的系统通知栏上展示。
    /// </summary>
    [DataContract]
    public class NotificationWinPhone
    {
        #region 属性

        /// <summary>
        ///     通知内容。
        ///     会填充到 toast 类型 text2 字段上。这里指定了，将会覆盖上级统一指定的 alert 信息；内容为空则不展示到通知栏。
        /// </summary>
        [DataMember(Order = 1, Name = "alert", IsRequired = true)]
        public string Alert { get; set; }

        /// <summary>
        ///     通知标题。
        ///     会填充到 toast 类型 text1 字段上。
        /// </summary>
        [DataMember(Order = 2, Name = "title")]
        public string Title { get; set; }

        /// <summary>
        ///     点击打开的页面名称。
        ///     点击打开的页面。会填充到推送信息的 param 字段上，表示由哪个 App 页面打开该通知。可不填，则由默认的首页打开。
        /// </summary>
        [DataMember(Order = 3, Name = "_open_page")]
        public string OpenPage { get; set; }

        /// <summary>
        ///     扩展字段。
        ///     作为参数附加到上述打开页面的后边。
        /// </summary>
        [DataMember(Order = 4, Name = "extras")]
        public Dictionary<string, object> Extras { get; set; }

        #endregion
    }
}