using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.Push
{
    /// <summary>
    ///     Android 平台上的通知，JPush SDK 按照一定的通知栏样式展示。
    /// </summary>
    [DataContract]
    public class NotificationAndroid
    {
        #region 属性

        /// <summary>
        ///     通知内容。
        ///     这里指定了，则会覆盖上级统一指定的 alert 信息；内容可以为空字符串，则表示不展示到通知栏。
        /// </summary>
        [DataMember(Order = 1, Name = "alert", IsRequired = true)]
        public string Alert { get; set; }

        /// <summary>
        ///     通知标题。
        ///     如果指定了，则通知里原来展示 App名称的地方，将展示成这个字段。
        /// </summary>
        [DataMember(Order = 2, Name = "title")]
        public string Title { get; set; }

        /// <summary>
        ///     通知栏样式ID。
        ///     Android SDK 可设置通知栏样式，这里根据样式 ID 来指定该使用哪套样式。
        /// </summary>
        [DataMember(Order = 3, Name = "builder_id")]
        public int? BuilderId { get; set; }

        /// <summary>
        ///     通知栏展示优先级。
        ///     默认为0，范围为 -2～2 ，其他值将会被忽略而采用默认。
        /// </summary>
        [DataMember(Order = 4, Name = "priority")]
        public int? Priority { get; set; }

        /// <summary>
        ///     通知栏条目过滤或排序。
        ///     完全依赖 rom 厂商对 category 的处理策略。
        /// </summary>
        [DataMember(Order = 5, Name = "category")]
        public string Category { get; set; }

        /// <summary>
        ///     通知栏样式类型。
        ///     默认为0，还有1，2，3可选，用来指定选择哪种通知栏样式，其他值无效。有三种可选分别为bigText=1，Inbox=2，bigPicture=3。
        /// </summary>
        [DataMember(Order = 6, Name = "style")]
        public int? Style { get; set; }

        /// <summary>
        ///     通知提醒方式。
        ///     可选范围为 -1 ～ 7 ，对应 Notification.DEFAULT_ALL = -1 或者 Notification.DEFAULT_SOUND = 1, Notification.DEFAULT_VIBRATE = 2,
        ///     Notification.DEFAULT_LIGHTS = 4 的任意 “or” 组合。默认按照 -1 处理。
        /// </summary>
        [DataMember(Order = 7, Name = "alert_type")]
        public int? AlertType { get; set; }

        /// <summary>
        ///     大文本通知栏样式。
        ///     当 style = 1 时可用，内容会被通知栏以大文本的形式展示出来。支持 api 16以上的rom。
        /// </summary>
        [DataMember(Order = 8, Name = "big_text")]
        public string BigText { get; set; }

        /// <summary>
        ///     文本条目通知栏样式。
        ///     当 style = 2 时可用， json 的每个 key 对应的 value 会被当作文本条目逐条展示。支持 api 16以上的rom。
        /// </summary>
        [DataMember(Order = 9, Name = "inbox")]
        public Dictionary<string, object> Inbox { get; set; }

        /// <summary>
        ///     大图片通知栏样式。
        ///     当 style = 3 时可用，可以是网络图片 url，或本地图片的 path，目前支持.jpg和.png后缀的图片。图片内容会被通知栏以大图片的形式展示出来。如果是 http／https
        ///     的url，会自动下载；如果要指定开发者准备的本地图片就填sdcard 的相对路径。支持 api 16以上的rom。
        /// </summary>
        [DataMember(Order = 10, Name = "big_pic_path")]
        public string BigPicturePath { get; set; }

        /// <summary>
        ///     扩展字段。
        ///     这里自定义 JSON 格式的 Key/Value 信息，以供业务使用。
        /// </summary>
        [DataMember(Order = 11, Name = "extras")]
        public Dictionary<string, object> Extras { get; set; }

        /// <summary>
        ///     指定开发者想要打开的 Activity，值为 activity 节点的 "android:name" 属性值。
        /// </summary>
        [DataMember(Order = 12, Name = "url_activity")]
        public string URLActivity { get; set; }

        /// <summary>
        ///     指定打开 Activity 的方式，值为 Intent.java 中预定义的 "access flags" 的取值范围。
        /// </summary>
        [DataMember(Order = 13, Name = "url_flag")]
        public string URLFlag { get; set; }

        /// <summary>
        ///     指定开发者想要打开的 Activity，值为activity -> intent-filter -> action节点中的 "android:name" 属性值。
        /// </summary>
        [DataMember(Order = 14, Name = "uri_action")]
        public string URIAction { get; set; }

        #endregion
    }
}