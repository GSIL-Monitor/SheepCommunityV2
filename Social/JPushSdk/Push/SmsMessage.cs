using System.Runtime.Serialization;

namespace JPush.Push
{
    /// <summary>
    ///     短信补充。
    ///     使用短信业务，会产生额外的运营商费用，具体请咨询商务，联系电话：400-612-5955 商务QQ：800024881。
    /// </summary>
    [DataContract]
    public class SmsMessage
    {
        #region 属性

        /// <summary>
        ///     通知内容。
        ///     不能超过480个字符。"你好,JPush"为8个字符。70个字符记一条短信费，如果超过70个字符则按照每条67个字符拆分，逐条计费。单个汉字、标点、英文都算一个字。
        /// </summary>
        [DataMember(Order = 1, Name = "content", IsRequired = true)]
        public string Content { get; set; }

        /// <summary>
        ///     延迟时间。
        ///     单位为秒，不能超过24小时。设置为0，表示立即发送短信。该参数仅对android平台有效，iOS 和 Winphone平台则会立即发送短信。
        /// </summary>
        [DataMember(Order = 2, Name = "delay_time", IsRequired = true)]
        public int DelayTime { get; set; }

        #endregion
    }
}