using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     拉人入群的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/add.action
    /// </remarks>
    [DataContract]
    public class TeamAddMemberRequest
    {
        #region 属性

        /// <summary>
        ///     网易云通信服务器产生，群唯一标识，创建群时会返回，最大长度128字符。
        /// </summary>
        [DataMember(Order = 1, Name = "tid")]
        public string TeamId { get; set; }

        /// <summary>
        ///     群主用户帐号，最大长度32字符。
        /// </summary>
        [DataMember(Order = 2, Name = "owner")]
        public string OwnerAccountId { get; set; }

        /// <summary>
        ///     ["aaa","bbb"](JSONArray对应的accid，如果解析出错会报414)，一次最多拉200个成员。
        /// </summary>
        [DataMember(Order = 3, Name = "members")]
        public List<string> MemberAccountIds { get; set; }

        /// <summary>
        ///     管理后台建群时，0不需要被邀请人同意加入群，1需要被邀请人同意才可以加入群。其它会返回414。
        /// </summary>
        [DataMember(Order = 4, Name = "magree")]
        public int MessageAgree { get; set; }

        /// <summary>
        ///     邀请发送的文字，最大长度150字符。
        /// </summary>
        [DataMember(Order = 5, Name = "msg")]
        public string Message { get; set; }

        /// <summary>
        ///     自定义扩展字段，最大长度512。
        /// </summary>
        [DataMember(Order = 6, Name = "attach")]
        public string Attach { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("tid=");
            builder.Append(TeamId);
            builder.Append("&owner=");
            builder.Append(OwnerAccountId);
            builder.Append("&members=");
            builder.Append(MemberAccountIds.ToJson());
            builder.Append("&magree=");
            builder.Append(MessageAgree);
            builder.Append("&msg=");
            builder.Append(Message);
            if (!Attach.IsNullOrEmpty())
            {
                builder.Append("&attach=");
                builder.Append(Attach);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}