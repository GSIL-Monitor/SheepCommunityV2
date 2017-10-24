using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     消息自定义的反垃圾内容。
    /// </summary>
    [DataContract]
    public class MessageAntiSpamCustom
    {
        #region 属性

        /// <summary>
        ///     1：文本，2：图片，3：视频。
        /// </summary>
        [DataMember(Order = 1, Name = "type")]
        public int Type { get; set; }

        /// <summary>
        ///     文本内容或图片地址或视频地址。
        /// </summary>
        [DataMember(Order = 2, Name = "data")]
        public string Data { get; set; }

        #endregion
    }
}