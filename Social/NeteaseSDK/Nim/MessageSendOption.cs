using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     消息发送选项。
    /// </summary>
    [DataContract]
    public class MessageSendOption
    {
        #region 属性

        /// <summary>
        ///     该消息是否需要漫游，默认true（需要app开通漫游消息功能）。
        /// </summary>
        [DataMember(Order = 1, Name = "roam")]
        public bool Roam { get; set; }

        /// <summary>
        ///     该消息是否存云端历史，默认true。
        /// </summary>
        [DataMember(Order = 2, Name = "history")]
        public bool History { get; set; }

        /// <summary>
        ///     该消息是否需要发送方多端同步，默认true。
        /// </summary>
        [DataMember(Order = 3, Name = "sendersync")]
        public bool SenderSync { get; set; }

        /// <summary>
        ///     该消息是否需要APNS推送或安卓系统通知栏推送，默认true。
        /// </summary>
        [DataMember(Order = 4, Name = "push")]
        public bool Push { get; set; }

        /// <summary>
        ///     该消息是否需要抄送第三方；默认true (需要app开通消息抄送功能)。
        /// </summary>
        [DataMember(Order = 5, Name = "route")]
        public bool Route { get; set; }

        /// <summary>
        ///     该消息是否需要计入到未读计数中，默认true。
        /// </summary>
        [DataMember(Order = 6, Name = "badge")]
        public bool Badge { get; set; }

        /// <summary>
        ///     推送文案是否需要带上昵称，不设置该参数时默认true。
        /// </summary>
        [DataMember(Order = 7, Name = "needPushNick")]
        public bool NeedPushNick { get; set; }

        /// <summary>
        ///     是否需要存离线消息，不设置该参数时默认true。
        /// </summary>
        [DataMember(Order = 8, Name = "persistent")]
        public bool Persistent { get; set; }

        #endregion

        #region 构造器

        public MessageSendOption()
        {
            Roam = true;
            History = true;
            SenderSync = true;
            Push = true;
            Route = true;
            Badge = true;
            NeedPushNick = true;
            Persistent = true;
        }

        #endregion
    }
}