using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     发送自定义系统通知的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/msg/sendAttachMsg.action
    /// </remarks>
    [DataContract]
    public class MessageSendAttachRequest
    {
        #region 属性

        /// <summary>
        ///     发送者用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 1, Name = "from")]
        public string FromAccountId { get; set; }

        /// <summary>
        ///     0：点对点自定义通知，1：群消息自定义通知，其他返回414
        /// </summary>
        [DataMember(Order = 2, Name = "msgtype")]
        public int MessageType { get; set; }

        /// <summary>
        ///     msgtype==0 是表示 accid 即用户Id，msgtype==1 表示 tid 即群Id。
        /// </summary>
        [DataMember(Order = 3, Name = "to")]
        public string ToId { get; set; }

        /// <summary>
        ///     自定义通知内容，第三方组装的字符串，建议是JSON串，最大长度4096字符。
        /// </summary>
        [DataMember(Order = 4, Name = "attach")]
        public string Attach { get; set; }

        /// <summary>
        ///     ios推送内容，不超过150字符，option选项中允许推送（push=true），此字段可以指定推送内容。
        /// </summary>
        [DataMember(Order = 5, Name = "pushcontent")]
        public string PushContent { get; set; }

        /// <summary>
        ///     ios 推送对应的payload,必须是JSON,不能超过2k字符。
        /// </summary>
        [DataMember(Order = 6, Name = "payload")]
        public string PushPayload { get; set; }

        /// <summary>
        ///     如果有指定推送，此属性指定为客户端本地的声音文件名，长度不要超过30个字符，如果不指定，会使用默认声音。
        /// </summary>
        [DataMember(Order = 7, Name = "sound")]
        public string PushSound { get; set; }

        /// <summary>
        ///     1表示只发在线，2表示会存离线，其他会报414错误。默认会存离线。
        /// </summary>
        [DataMember(Order = 8, Name = "save")]
        public int? Save { get; set; }

        /// <summary>
        ///     发消息时特殊指定的行为选项,JSON格式，可用于指定消息的漫游，存云端历史，发送方多端同步，推送，消息抄送等特殊行为;option中字段不填时表示默认值。
        /// </summary>
        [DataMember(Order = 9, Name = "option")]
        public MessageSendAttachOption Option { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("from=");
            builder.Append(FromAccountId);
            builder.Append("&msgtype=");
            builder.Append(MessageType);
            builder.Append("&to=");
            builder.Append(ToId);
            builder.Append("&attach=");
            builder.Append(Attach.ToJson());
            if (!PushContent.IsNullOrEmpty())
            {
                builder.Append("&pushcontent=");
                builder.Append(PushContent);
            }
            if (!PushPayload.IsNullOrEmpty())
            {
                builder.Append("&payload=");
                builder.Append(PushPayload);
            }
            if (!PushSound.IsNullOrEmpty())
            {
                builder.Append("&sound=");
                builder.Append(PushSound);
            }
            if (Save.HasValue)
            {
                builder.Append("&save=");
                builder.Append(Save.Value);
            }
            if (Option != null)
            {
                builder.Append("&option=");
                builder.Append(Option.ToJson());
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}