using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Text;
using Sheep.Model.Properties;
using Sheep.Model.Security;

namespace Sheep.Model.Auth.Providers
{
    /// <summary>
    ///     使用手机号码进行身份验证的提供程序。
    /// </summary>
    public class MobileAuthProvider : OAuthProvider
    {
        #region 校验器类

        private class MobileAuthValidator : AbstractValidator<Authenticate>
        {
            /// <summary>
            ///     初始化一个新的<see cref="MobileAuthProvider.MobileAuthValidator" />对象。
            /// </summary>
            public MobileAuthValidator()
            {
                RuleFor(x => x.UserName).NotEmpty().WithMessage(Resources.PhoneNumberRequired).Matches("^1[3|4|5|7|8][0-9]{9}$").WithMessage(Resources.PhoneNumberFormatMismatch);
                RuleFor(x => x.State).NotEmpty().WithMessage(Resources.PurposeRequired);
                RuleFor(x => x.Password).NotEmpty().WithMessage(Resources.SecurityTokenRequired).Length(6).WithMessage(Resources.SecurityTokenLengthMismatch, 6);
            }
        }

        #endregion

        #region 常量

        public const string Name = "mobile";
        public const string Realm = "/auth/mobile";

        #endregion

        #region 属性

        private readonly MobileAuthValidator _mobileAuthValidator = new MobileAuthValidator();

        /// <summary>
        ///     安全验证码提供程序。
        /// </summary>
        public ISecurityTokenProvider SecurityTokenProvider { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="MobileAuthProvider" />对象。
        /// </summary>
        /// <param name="appSettings">应用程序设置。</param>
        /// <param name="securityTokenProvider">安全验证码提供程序。</param>
        public MobileAuthProvider(IAppSettings appSettings, ISecurityTokenProvider securityTokenProvider)
            : base(appSettings, Realm, Name, "AppKey", "AppSecret")
        {
            SecurityTokenProvider = securityTokenProvider;
        }

        #endregion

        #region 重写 OAuthProvider

        /// <summary>
        ///     指定的用户身份会话是否已通过身份验证。
        /// </summary>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <param name="request">身份验证请求。</param>
        /// <returns>如果已授权，则返回 true，否则返回 false。</returns>
        public override bool IsAuthorized(IAuthSession session, IAuthTokens tokens, Authenticate request = null)
        {
            if (request != null)
            {
                if (!LoginMatchesSession(session, request.UserName))
                {
                    return false;
                }
            }
            return session != null && session.IsAuthenticated && !string.IsNullOrEmpty(tokens?.PhoneNumber);
        }

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
            _mobileAuthValidator.ValidateAndThrow(request);
            var phoneNumber = request.UserName;
            var purpose = request.State;
            var securityToken = request.Password;
            var validToken = SecurityTokenProvider.VerifyToken(phoneNumber, purpose, securityToken);
            if (!validToken)
            {
                return HttpError.Unauthorized(Resources.InvalidSecurityToken);
            }
            var failedResult = AuthenticateWithPhoneNumber(authService, session, tokens, phoneNumber, purpose, securityToken);
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
                tokens.UserId = authInfo.Get("PhoneNumber");
                tokens.UserName = authInfo.Get("UserName");
                tokens.Country = authInfo.Get("Country");
            }
            catch (Exception ex)
            {
                Log.Error($"Could not retrieve mobile user info for '{tokens.UserId}'", ex);
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
                userSession.PhoneNumber = tokens.PhoneNumber ?? userSession.PhoneNumber;
            }
        }

        #endregion

        #region 身份验证

        /// <summary>
        ///     验证指定的手机号码是否有效。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证第三方令牌。</param>
        /// <param name="phoneNumber">手机号码。</param>
        /// <param name="purpose">用途。</param>
        /// <param name="securityToken">安全验证码。</param>
        /// <returns>身份验证的结果或响应。</returns>
        protected virtual object AuthenticateWithPhoneNumber(IServiceBase authService, IAuthSession session, IAuthTokens tokens, string phoneNumber, string purpose, string securityToken)
        {
            tokens.PhoneNumber = phoneNumber;
            switch (purpose)
            {
                case "Login":
                    tokens.AccessToken = securityToken;
                    break;
                case "Bind":
                    tokens.AccessTokenSecret = securityToken;
                    break;
                case "Register":
                    tokens.RequestToken = securityToken;
                    break;
                case "ResetPassword":
                    tokens.RefreshToken = securityToken;
                    break;
            }
            var authInfo = new Dictionary<string, string>
                           {
                               ["PhoneNumber"] = phoneNumber,
                               ["UserName"] = $"Mobile{phoneNumber}",
                               ["Country"] = "中国"
                           };
            session.IsAuthenticated = true;
            return OnAuthenticated(authService, session, tokens, authInfo);
        }

        #endregion
    }
}