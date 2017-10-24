using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     消息撤回的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/msg/recall.action
    /// </remarks>
    [DataContract]
    public class MessageRecallRequest
    {
        #region 属性

        /// <summary>
        ///     要撤回消息的msgid。
        /// </summary>
        [DataMember(Order = 1, Name = "deleteMsgid")]
        public string DeleteMessageId { get; set; }

        /// <summary>
        ///     要撤回消息的创建时间。
        /// </summary>
        [DataMember(Order = 2, Name = "timetag")]
        public long TimeTag { get; set; }

        /// <summary>
        ///     7：表示点对点消息撤回，8：表示群消息撤回，其它为参数错误。
        /// </summary>
        [DataMember(Order = 3, Name = "type")]
        public int Type { get; set; }

        /// <summary>
        ///     发送者用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 4, Name = "from")]
        public string FromAccountId { get; set; }

        /// <summary>
        ///     如果点对点消息，为接收消息的accid,如果群消息，为对应群的tid。
        /// </summary>
        [DataMember(Order = 5, Name = "to")]
        public string ToId { get; set; }

        /// <summary>
        ///     可以带上对应的描述。
        /// </summary>
        [DataMember(Order = 6, Name = "msg")]
        public string Message { get; set; }

        /// <summary>
        ///     1表示忽略撤回时间检测，其它为非法参数，如果需要撤回时间检测，不填即可。
        /// </summary>
        [DataMember(Order = 7, Name = "ignoreTime")]
        public string IgnoreTime { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("deleteMsgid=");
            builder.Append(DeleteMessageId);
            builder.Append("&timetag=");
            builder.Append(TimeTag);
            builder.Append("&type=");
            builder.Append(Type);
            builder.Append("&from=");
            builder.Append(FromAccountId);
            builder.Append("&to=");
            builder.Append(ToId);
            if (!Message.IsNullOrEmpty())
            {
                builder.Append("&msg=");
                builder.Append(Message);
            }
            if (!IgnoreTime.IsNullOrEmpty())
            {
                builder.Append("&ignoreTime=");
                builder.Append(IgnoreTime);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}