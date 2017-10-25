using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Text;
using Sheep.Model.Properties;
using Sina.Weibo;

namespace Sheep.Model.Auth.Providers
{
    /// <summary>
    ///     使用微博帐户进行身份验证的提供程序。
    /// </summary>
    public class WeiboAuthProvider : OAuthProvider
    {
        #region 校验器类

        private class WeiboAuthValidator : AbstractValidator<Authenticate>
        {
            /// <summary>
            ///     初始化一个新的<see cref="WeiboAuthProvider.WeiboAuthValidator" />对象。
            /// </summary>
            public WeiboAuthValidator()
            {
                RuleFor(x => x.UserName).NotEmpty().WithMessage(Resources.OpenUserIdRequired);
                RuleFor(x => x.AccessToken).NotEmpty().WithMessage(Resources.AccessTokenRequired);
            }
        }

        #endregion

        #region 常量

        public const string Name = "weibo";
        public const string Realm = "https://api.weibo.com/oauth2";

        #endregion

        #region 属性

        private readonly WeiboAuthValidator _weiboAuthValidator = new WeiboAuthValidator();

        /// <summary>
        ///     新浪微博服务客户端封装库。
        /// </summary>
        public IWeiboClient WeiboClient { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="WeiboAuthProvider" />对象。
        /// </summary>
        /// <param name="appSettings">应用程序设置。</param>
        /// <param name="weiboClient">新浪微博服务客户端封装库。</param>
        public WeiboAuthProvider(IAppSettings appSettings, IWeiboClient weiboClient)
            : base(appSettings, Realm, Name, "AppKey", "AppSecret")
        {
            WeiboClient = weiboClient;
        }

        #endregion

        #region 重写 OAuthProvider

        /// <summary>
        ///     身份验证的入口点。 在AuthService中运行，因此需要将异常处理当作正常处理流程。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="request">身份验证请求。</param>
        /// <returns>身份验证的结果或响应。</returns>
        public override object Authenticate(IServiceBase authService, IAuthSession session, Authenticate request)
        {
            var tokens = Init(authService, ref session, request);
            _weiboAuthValidator.ValidateAndThrow(request);
            var openUserId = request.UserName;
            var accessToken = request.AccessToken;
            var getTokenResponse = WeiboClient.Post(new GetTokenRequest
                                                    {
                                                        AccessToken = accessToken
                                                    });
            if (getTokenResponse.ErrorCode != 0)
            {
                return HttpError.Unauthorized(getTokenResponse.ErrorDescription);
            }
            if (getTokenResponse.UserId.ToLower() != openUserId.ToLower())
            {
                return HttpError.Unauthorized(Resources.InvalidOpenUserId);
            }
            var failedResult = AuthenticateWithAccessToken(authService, session, tokens, openUserId, accessToken);
            if (failedResult != null)
            {
                return ConvertToClientError(failedResult);
            }
            return new AuthenticateResponse
                   {
                       UserId = session.UserAuthId,
                       UserName = session.UserName,
                       DisplayName = session.DisplayName,
                       SessionId = session.Id
                   };
        }

        /// <summary>
        ///     将用户身份信息表加载到用户身份会话及身份凭据中。
        /// </summary>
        /// <param name="userSession">用户身份会话。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <param name="authInfo">身份验证信息。</param>
        protected override void LoadUserAuthInfo(AuthUserSession userSession, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            try
            {
                tokens.UserId = authInfo.Get("Id");
                tokens.DisplayName = authInfo.Get("ScreenName");
                tokens.Nickname = authInfo.Get("Domain");
                tokens.Gender = MapToGender(authInfo.Get("Gender"));
                tokens.Country = MapToCountry(authInfo.Get("Country"));
                tokens.State = MapToProvince(authInfo.Get("ProvinceId"));
                tokens.City = MapToCity(authInfo.Get("CityId"));
                tokens.Language = MapToLanguage("Language");
                tokens.Items["AvatarUrl"] = authInfo.Get("AvatarHdUrl");
                tokens.Items["ProfileUrl"] = "http://weibo.com/{0}".Fmt(authInfo.Get("ProfileUrl"));
            }
            catch (Exception ex)
            {
                Log.Error($"Could not retrieve weibo user info for '{tokens.UserId}'", ex);
            }
            LoadUserOAuthProvider(userSession, tokens);
        }

        /// <summary>
        ///     将身份凭据中的信息加载到用户身份会话中。
        /// </summary>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证凭据。</param>
        public override void LoadUserOAuthProvider(IAuthSession session, IAuthTokens tokens)
        {
            if (session is AuthUserSession userSession)
            {
                userSession.DisplayName = tokens.DisplayName ?? userSession.DisplayName;
                userSession.Nickname = tokens.Nickname ?? userSession.Nickname;
                userSession.Gender = tokens.Gender ?? userSession.Gender;
                userSession.Country = tokens.Country ?? userSession.Country;
                userSession.State = tokens.State ?? userSession.State;
                userSession.City = tokens.City ?? userSession.City;
                userSession.Language = tokens.Language ?? userSession.Language;
            }
        }

        #endregion

        #region 身份验证

        /// <summary>
        ///     验证指定的微博帐户是否有效。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证第三方令牌。</param>
        /// <param name="openUserId">平台用户编号。</param>
        /// <param name="accessToken">授权码。</param>
        /// <returns>身份验证的结果或响应。</returns>
        protected virtual object AuthenticateWithAccessToken(IServiceBase authService, IAuthSession session, IAuthTokens tokens, string openUserId, string accessToken)
        {
            tokens.AccessToken = accessToken;
            var showUserResponse = WeiboClient.Get(new ShowUserRequest
                                                   {
                                                       AccessToken = accessToken,
                                                       UserId = openUserId
                                                   });
            var authInfo = new Dictionary<string, string>
                           {
                               ["Id"] = showUserResponse.Id.ToString(),
                               ["Class"] = showUserResponse.Class.ToString(),
                               ["ScreenName"] = showUserResponse.ScreenName,
                               ["Name"] = showUserResponse.Name,
                               ["ProvinceId"] = showUserResponse.ProvinceId,
                               ["CityId"] = showUserResponse.CityId,
                               ["Location"] = showUserResponse.Location,
                               ["Description"] = showUserResponse.Description,
                               ["Url"] = showUserResponse.Url,
                               ["ProfileImageUrl"] = showUserResponse.ProfileImageUrl,
                               ["CoverImageUrl"] = showUserResponse.CoverImageUrl,
                               ["CoverImagePhoneUrl"] = showUserResponse.CoverImagePhoneUrl,
                               ["ProfileUrl"] = showUserResponse.ProfileUrl,
                               ["Domain"] = showUserResponse.Domain,
                               ["Weihao"] = showUserResponse.Weihao,
                               ["Gender"] = showUserResponse.Gender,
                               ["FollowersCount"] = showUserResponse.FollowersCount.ToString(),
                               ["FriendsCount"] = showUserResponse.FriendsCount.ToString(),
                               ["StatusesCount"] = showUserResponse.StatusesCount.ToString(),
                               ["FavouritesCount"] = showUserResponse.FavouritesCount.ToString(),
                               ["CreatedAt"] = showUserResponse.CreatedAt,
                               ["Following"] = showUserResponse.Following.ToString(),
                               ["AllowAllActMessage"] = showUserResponse.AllowAllActMessage.ToString(),
                               ["GeoEnabled"] = showUserResponse.GeoEnabled.ToString(),
                               ["Verified"] = showUserResponse.Verified.ToString(),
                               ["VerifiedType"] = showUserResponse.VerifiedType.ToString(),
                               ["Remark"] = showUserResponse.Remark,
                               ["AllowAllComment"] = showUserResponse.AllowAllComment.ToString(),
                               ["AvatarLargeUrl"] = showUserResponse.AvatarLargeUrl,
                               ["AvatarHdUrl"] = showUserResponse.AvatarHdUrl,
                               ["VerifiedReason"] = showUserResponse.VerifiedReason,
                               ["FollowMe"] = showUserResponse.FollowMe.ToString(),
                               ["OnlineStatus"] = showUserResponse.OnlineStatus.ToString(),
                               ["BiFollowersCount"] = showUserResponse.BiFollowersCount.ToString(),
                               ["Language"] = showUserResponse.Language
                           };
            session.IsAuthenticated = true;
            return OnAuthenticated(authService, session, tokens, authInfo);
        }

        #endregion

        #region 映射方法

        private string MapToGender(string gender)
        {
            if (!gender.IsNullOrEmpty())
            {
                switch (gender.ToLower())
                {
                    case "m":
                        return "男";
                    case "f":
                        return "女";
                    case "n":
                        return null;
                }
            }
            return null;
        }

        private string MapToLanguage(string language)
        {
            if (!language.IsNullOrEmpty())
            {
                switch (language.ToLower())
                {
                    case "zh-cn":
                        return "简体中文";
                    case "zh-tw":
                        return "繁体中文";
                    case "en":
                        return "英文";
                }
            }
            return "简体中文";
        }

        private string MapToCountry(string country)
        {
            if (!country.IsNullOrEmpty())
            {
                switch (country.ToUpper())
                {
                    case "CN":
                        return "中国";
                    case "US":
                        return "美国";
                    case "JP":
                        return "日本";
                    case "GB":
                        return "英国";
                    case "KR":
                        return "韩国";
                    default:
                        return country;
                }
            }
            return null;
        }

        private string MapToProvince(string province)
        {
            if (!province.IsNullOrEmpty())
            {
                switch (province.ToUpper())
                {
                    default:
                        return province;
                }
            }
            return null;
        }

        private string MapToCity(string city)
        {
            if (!city.IsNullOrEmpty())
            {
                switch (city.ToUpper())
                {
                    default:
                        return city;
                }
            }
            return null;
        }

        #endregion
    }
}