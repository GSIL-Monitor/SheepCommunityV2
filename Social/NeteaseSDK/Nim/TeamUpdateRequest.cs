using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     编辑群资料的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/team/update.action
    /// </remarks>
    [DataContract]
    public class TeamUpdateRequest
    {
        #region 属性

        /// <summary>
        ///     网易云通信服务器产生，群唯一标识，创建群时会返回，最大长度128字符。
        /// </summary>
        [DataMember(Order = 1, Name = "tid")]
        public string TeamId { get; set; }

        /// <summary>
        ///     群名称，最大长度64字符。
        /// </summary>
        [DataMember(Order = 2, Name = "tname")]
        public string TeamName { get; set; }

        /// <summary>
        ///     群主用户帐号，最大长度32字符。
        /// </summary>
        [DataMember(Order = 3, Name = "owner")]
        public string OwnerAccountId { get; set; }

        /// <summary>
        ///     群公告，最大长度1024字符。
        /// </summary>
        [DataMember(Order = 4, Name = "announcement")]
        public string Announcement { get; set; }

        /// <summary>
        ///     群描述，最大长度512字符。
        /// </summary>
        [DataMember(Order = 5, Name = "intro")]
        public string Intro { get; set; }

        /// <summary>
        ///     群建好后，sdk操作时，0不用验证，1需要验证,2不允许任何人加入。其它返回414。
        /// </summary>
        [DataMember(Order = 6, Name = "joinmode")]
        public int? JoinMode { get; set; }

        /// <summary>
        ///     自定义高级群扩展属性，第三方可以跟据此属性自定义扩展自己的群属性。（建议为json）,最大长度1024字符。
        /// </summary>
        [DataMember(Order = 7, Name = "custom")]
        public string Custom { get; set; }

        /// <summary>
        ///     群头像，最大长度1024字符。
        /// </summary>
        [DataMember(Order = 8, Name = "icon")]
        public string IconUrl { get; set; }

        /// <summary>
        ///     被邀请人同意方式，0-需要同意(默认),1-不需要同意。其它返回414。
        /// </summary>
        [DataMember(Order = 9, Name = "beinvitemode")]
        public int? BeInviteMode { get; set; }

        /// <summary>
        ///     谁可以邀请他人入群，0-管理员(默认),1-所有人。其它返回414。
        /// </summary>
        [DataMember(Order = 10, Name = "invitemode")]
        public int? InviteMode { get; set; }

        /// <summary>
        ///     谁可以修改群资料，0-管理员(默认),1-所有人。其它返回414。
        /// </summary>
        [DataMember(Order = 11, Name = "uptinfomode")]
        public int? UpdateInfoMode { get; set; }

        /// <summary>
        ///     谁可以更新群自定义属性，0-管理员(默认),1-所有人。其它返回414。
        /// </summary>
        [DataMember(Order = 12, Name = "upcustommode")]
        public int? UpdateCustomMode { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("tid=");
            builder.Append(TeamId);
            if (!TeamName.IsNullOrEmpty())
            {
                builder.Append("&tname=");
                builder.Append(TeamName);
            }
            builder.Append("&owner=");
            builder.Append(OwnerAccountId);
            if (!Announcement.IsNullOrEmpty())
            {
                builder.Append("&announcement=");
                builder.Append(Announcement);
            }
            if (!Intro.IsNullOrEmpty())
            {
                builder.Append("&intro=");
                builder.Append(Intro);
            }
            if (JoinMode.HasValue)
            {
                builder.Append("&joinmode=");
                builder.Append(JoinMode.Value);
            }
            if (!Custom.IsNullOrEmpty())
            {
                builder.Append("&custom=");
                builder.Append(Custom);
            }
            if (!IconUrl.IsNullOrEmpty())
            {
                builder.Append("&icon=");
                builder.Append(IconUrl);
            }
            if (BeInviteMode.HasValue)
            {
                builder.Append("&beinvitemode=");
                builder.Append(BeInviteMode.Value);
            }
            if (InviteMode.HasValue)
            {
                builder.Append("&invitemode=");
                builder.Append(InviteMode.Value);
            }
            if (UpdateInfoMode.HasValue)
            {
                builder.Append("&uptinfomode=");
                builder.Append(UpdateInfoMode.Value);
            }
            if (UpdateCustomMode.HasValue)
            {
                builder.Append("&upcustommode=");
                builder.Append(UpdateCustomMode.Value);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}