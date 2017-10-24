using System.Runtime.Serialization;

namespace Sina.Weibo
{
    /// <summary>
    ///     根据用户编号获取用户信息的响应。
    /// </summary>
    [DataContract]
    public class ShowUserResponse : ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 101, Name = "id")]
        public long Id { get; set; }

        /// <summary>
        ///     字符串型的用户编号。
        /// </summary>
        [DataMember(Order = 102, Name = "idstr")]
        public string IdString { get; set; }

        /// <summary>
        ///     用户分类。
        /// </summary>
        [DataMember(Order = 103, Name = "class")]
        public int Class { get; set; }

        /// <summary>
        ///     用户昵称。
        /// </summary>
        [DataMember(Order = 104, Name = "screen_name")]
        public string ScreenName { get; set; }

        /// <summary>
        ///     用户的友好显示名称。
        /// </summary>
        [DataMember(Order = 105, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     用户所在省级编号。
        /// </summary>
        [DataMember(Order = 106, Name = "province")]
        public string ProvinceId { get; set; }

        /// <summary>
        ///     用户所在城市编号。
        /// </summary>
        [DataMember(Order = 107, Name = "city")]
        public string CityId { get; set; }

        /// <summary>
        ///     用户所在地。
        /// </summary>
        [DataMember(Order = 108, Name = "location")]
        public string Location { get; set; }

        /// <summary>
        ///     用户个人描述。
        /// </summary>
        [DataMember(Order = 109, Name = "description")]
        public string Description { get; set; }

        /// <summary>
        ///     用户博客地址。
        /// </summary>
        [DataMember(Order = 110, Name = "url")]
        public string Url { get; set; }

        /// <summary>
        ///     用户头像地址（中图），50×50像素。
        /// </summary>
        [DataMember(Order = 111, Name = "profile_image_url")]
        public string ProfileImageUrl { get; set; }

        /// <summary>
        ///     用户封面地址。
        /// </summary>
        [DataMember(Order = 112, Name = "cover_image")]
        public string CoverImageUrl { get; set; }

        /// <summary>
        ///     用户手机封面地址。
        /// </summary>
        [DataMember(Order = 113, Name = "cover_image_phone")]
        public string CoverImagePhoneUrl { get; set; }

        /// <summary>
        ///     用户的微博统一地址。
        /// </summary>
        [DataMember(Order = 114, Name = "profile_url")]
        public string ProfileUrl { get; set; }

        /// <summary>
        ///     用户的个性化域名。
        /// </summary>
        [DataMember(Order = 115, Name = "domain")]
        public string Domain { get; set; }

        /// <summary>
        ///     用户的微号。
        /// </summary>
        [DataMember(Order = 116, Name = "weihao")]
        public string Weihao { get; set; }

        /// <summary>
        ///     性别，m：男、f：女、n：未知。
        /// </summary>
        [DataMember(Order = 117, Name = "gender")]
        public string Gender { get; set; }

        /// <summary>
        ///     粉丝数。
        /// </summary>
        [DataMember(Order = 118, Name = "followers_count")]
        public int FollowersCount { get; set; }

        /// <summary>
        ///     关注数。
        /// </summary>
        [DataMember(Order = 119, Name = "friends_count")]
        public int FriendsCount { get; set; }

        /// <summary>
        ///     微博数。
        /// </summary>
        [DataMember(Order = 120, Name = "statuses_count")]
        public int StatusesCount { get; set; }

        /// <summary>
        ///     收藏数。
        /// </summary>
        [DataMember(Order = 121, Name = "favourites_count")]
        public int FavouritesCount { get; set; }

        /// <summary>
        ///     用户创建（注册）时间。
        /// </summary>
        [DataMember(Order = 122, Name = "created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        ///     是否正在关注。（暂未支持）
        /// </summary>
        [DataMember(Order = 123, Name = "following")]
        public bool Following { get; set; }

        /// <summary>
        ///     是否允许所有人给我发私信，true：是，false：否。
        /// </summary>
        [DataMember(Order = 124, Name = "allow_all_act_msg")]
        public bool AllowAllActMessage { get; set; }

        /// <summary>
        ///     是否允许标识用户的地理位置，true：是，false：否。
        /// </summary>
        [DataMember(Order = 125, Name = "geo_enabled")]
        public bool GeoEnabled { get; set; }

        /// <summary>
        ///     是否是微博认证用户，即加V用户，true：是，false：否。
        /// </summary>
        [DataMember(Order = 126, Name = "verified")]
        public bool Verified { get; set; }

        /// <summary>
        ///     微博认证的类型。（暂未支持）
        /// </summary>
        [DataMember(Order = 127, Name = "verified_type")]
        public int VerifiedType { get; set; }

        /// <summary>
        ///     用户备注信息，只有在查询用户关系时才返回此字段。
        /// </summary>
        [DataMember(Order = 128, Name = "remark")]
        public string Remark { get; set; }

        /// <summary>
        ///     是否允许所有人对我的微博进行评论，true：是，false：否。
        /// </summary>
        [DataMember(Order = 129, Name = "allow_all_comment")]
        public bool AllowAllComment { get; set; }

        /// <summary>
        ///     用户头像地址（大图），180×180像素。
        /// </summary>
        [DataMember(Order = 130, Name = "avatar_large")]
        public string AvatarLargeUrl { get; set; }

        /// <summary>
        ///     用户头像地址（高清），高清头像原图。
        /// </summary>
        [DataMember(Order = 131, Name = "avatar_hd")]
        public string AvatarHdUrl { get; set; }

        /// <summary>
        ///     认证原因。
        /// </summary>
        [DataMember(Order = 132, Name = "verified_reason")]
        public string VerifiedReason { get; set; }

        /// <summary>
        ///     该用户是否关注当前登录用户，true：是，false：否。
        /// </summary>
        [DataMember(Order = 133, Name = "follow_me")]
        public bool FollowMe { get; set; }

        /// <summary>
        ///     用户的在线状态，0：不在线、1：在线。
        /// </summary>
        [DataMember(Order = 134, Name = "online_status")]
        public int OnlineStatus { get; set; }

        /// <summary>
        ///     用户的互粉数。
        /// </summary>
        [DataMember(Order = 135, Name = "bi_followers_count")]
        public int BiFollowersCount { get; set; }

        /// <summary>
        ///     用户当前的语言版本，zh-cn：简体中文，zh-tw：繁体中文，en：英语。
        /// </summary>
        [DataMember(Order = 136, Name = "lang")]
        public string Language { get; set; }

        #endregion
    }
}