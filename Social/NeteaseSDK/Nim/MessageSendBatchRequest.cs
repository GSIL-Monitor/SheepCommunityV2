using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     批量发送点对点普通消息的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/msg/sendBatchMsg.action
    /// </remarks>
    [DataContract]
    public class MessageSendBatchRequest
    {
        #region 属性

        /// <summary>
        ///     发送者用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 1, Name = "fromAccid")]
        public string FromAccountId { get; set; }

        /// <summary>
        ///     ["aaa","bbb"]（JSONArray对应的accid，如果解析出错，会报414错误），限500人。
        /// </summary>
        [DataMember(Order = 2, Name = "toAccids")]
        public List<string> ToAccountIds { get; set; }

        /// <summary>
        ///     0 表示文本消息，1 表示图片，2 表示语音，3 表示视频，4 表示地理位置信息，6 表示文件，100 自定义消息类型。
        /// </summary>
        [DataMember(Order = 3, Name = "type")]
        public int Type { get; set; }

        /// <summary>
        ///     对应消息体字段，最大长度5000字符，为一个JSON串。
        /// </summary>
        [DataMember(Order = 4, Name = "body")]
        public MessageBody Body { get; set; }

        /// <summary>
        ///     发消息时特殊指定的行为选项,JSON格式，可用于指定消息的漫游，存云端历史，发送方多端同步，推送，消息抄送等特殊行为;option中字段不填时表示默认值。
        /// </summary>
        [DataMember(Order = 5, Name = "option")]
        public MessageSendOption Option { get; set; }

        /// <summary>
        ///     ios推送内容，不超过150字符，option选项中允许推送（push=true），此字段可以指定推送内容。
        /// </summary>
        [DataMember(Order = 6, Name = "pushcontent")]
        public string PushContent { get; set; }

        /// <summary>
        ///     ios 推送对应的payload,必须是JSON,不能超过2k字符。
        /// </summary>
        [DataMember(Order = 7, Name = "payload")]
        public string PushPayload { get; set; }

        /// <summary>
        ///     开发者扩展字段，长度限制1024字符。
        /// </summary>
        [DataMember(Order = 8, Name = "ext")]
        public string PushExtensions { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("fromAccid=");
            builder.Append(FromAccountId);
            builder.Append("&toAccids=");
            builder.Append(ToAccountIds.ToJson());
            builder.Append("&type=");
            builder.Append(Type);
            builder.Append("&body=");
            builder.Append(Body.ToJson());
            if (Option != null)
            {
                builder.Append("&option=");
                builder.Append(Option.ToJson());
            }
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
            if (!PushExtensions.IsNullOrEmpty())
            {
                builder.Append("&ext=");
                builder.Append(PushExtensions);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}