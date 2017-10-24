using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     自定义系统通知发送选项。
    /// </summary>
    [DataContract]
    public class MessageSendAttachOption
    {
        #region 属性

        /// <summary>
        ///     该消息是否需要计入到未读计数中，默认true。
        /// </summary>
        [DataMember(Order = 1, Name = "badge")]
        public bool Badge { get; set; }

        /// <summary>
        ///     推送文案是否需要带上昵称，不设置该参数时默认false。
        /// </summary>
        [DataMember(Order = 2, Name = "needPushNick")]
        public bool NeedPushNick { get; set; }

        /// <summary>
        ///     该消息是否需要抄送第三方；默认true (需要app开通消息抄送功能)。
        /// </summary>
        [DataMember(Order = 3, Name = "route")]
        public bool Route { get; set; }

        #endregion

        #region 构造器

        public MessageSendAttachOption()
        {
            Badge = true;
            NeedPushNick = false;
            Route = true;
        }

        #endregion
    }
}