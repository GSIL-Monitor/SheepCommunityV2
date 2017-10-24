using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     发送普通消息的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/msg/sendMsg.action
    /// </remarks>
    [DataContract]
    public class MessageSendRequest
    {
        #region 属性

        /// <summary>
        ///     发送者用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 1, Name = "from")]
        public string FromAccountId { get; set; }

        /// <summary>
        ///     0：点对点个人消息，1：群消息（高级群），其他返回414。
        /// </summary>
        [DataMember(Order = 2, Name = "ope")]
        public int Operation { get; set; }

        /// <summary>
        ///     ope==0 是表示 accid 即用户Id，ope==1 表示 tid 即群Id。
        /// </summary>
        [DataMember(Order = 3, Name = "to")]
        public string ToId { get; set; }

        /// <summary>
        ///     0 表示文本消息，1 表示图片，2 表示语音，3 表示视频，4 表示地理位置信息，6 表示文件，100 自定义消息类型。
        /// </summary>
        [DataMember(Order = 4, Name = "type")]
        public int Type { get; set; }

        /// <summary>
        ///     对应消息体字段，最大长度5000字符，为一个JSON串。
        /// </summary>
        [DataMember(Order = 5, Name = "body")]
        public MessageBody Body { get; set; }

        /// <summary>
        ///     本消息是否需要过自定义反垃圾系统，true或false, 默认false。
        /// </summary>
        [DataMember(Order = 6, Name = "antispam")]
        public bool? AntiSpam { get; set; }

        /// <summary>
        ///     自定义的反垃圾内容, JSON格式，长度限制同body字段，不能超过5000字符。
        /// </summary>
        [DataMember(Order = 7, Name = "antispamCustom")]
        public MessageAntiSpamCustom AntiSpamCustom { get; set; }

        /// <summary>
        ///     发消息时特殊指定的行为选项,JSON格式，可用于指定消息的漫游，存云端历史，发送方多端同步，推送，消息抄送等特殊行为;option中字段不填时表示默认值。
        /// </summary>
        [DataMember(Order = 8, Name = "option")]
        public MessageSendOption Option { get; set; }

        /// <summary>
        ///     ios推送内容，不超过150字符，option选项中允许推送（push=true），此字段可以指定推送内容。
        /// </summary>
        [DataMember(Order = 9, Name = "pushcontent")]
        public string PushContent { get; set; }

        /// <summary>
        ///     ios 推送对应的payload,必须是JSON,不能超过2k字符。
        /// </summary>
        [DataMember(Order = 10, Name = "payload")]
        public string PushPayload { get; set; }

        /// <summary>
        ///     开发者扩展字段，长度限制1024字符。
        /// </summary>
        [DataMember(Order = 11, Name = "ext")]
        public string Extensions { get; set; }

        /// <summary>
        ///     发送群消息时的强推（@操作）用户列表，格式为JSONArray，如["accid1","accid2"]。若forcepushall为true，则forcepushlist为除发送者外的所有有效群成员。
        /// </summary>
        [DataMember(Order = 12, Name = "forcepushlist")]
        public List<string> ForcePushAccountIds { get; set; }

        /// <summary>
        ///     发送群消息时，针对强推（@操作）列表forcepushlist中的用户，强制推送的内容。
        /// </summary>
        [DataMember(Order = 13, Name = "forcepushcontent")]
        public string ForcePushContent { get; set; }

        /// <summary>
        ///     发送群消息时，强推（@操作）列表是否为群里除发送者外的所有有效成员，true或false，默认为false。
        /// </summary>
        [DataMember(Order = 14, Name = "forcepushall")]
        public bool? ForcePushAll { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("from=");
            builder.Append(FromAccountId);
            builder.Append("&ope=");
            builder.Append(Operation);
            builder.Append("&to=");
            builder.Append(ToId);
            builder.Append("&type=");
            builder.Append(Type);
            builder.Append("&body=");
            builder.Append(Body.ToJson());
            if (AntiSpam.HasValue)
            {
                builder.Append("&antispam=");
                builder.Append(AntiSpam.Value);
            }
            if (AntiSpamCustom != null)
            {
                builder.Append("&antispamCustom=");
                builder.Append(AntiSpamCustom.ToJson());
            }
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
            if (!Extensions.IsNullOrEmpty())
            {
                builder.Append("&ext=");
                builder.Append(Extensions);
            }
            if (ForcePushAccountIds != null)
            {
                builder.Append("&forcepushlist=");
                builder.Append(ForcePushAccountIds.ToJson());
            }
            if (!ForcePushContent.IsNullOrEmpty())
            {
                builder.Append("&forcepushcontent=");
                builder.Append(ForcePushContent);
            }
            if (ForcePushAll.HasValue)
            {
                builder.Append("&forcepushall=");
                builder.Append(ForcePushAll.Value);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}