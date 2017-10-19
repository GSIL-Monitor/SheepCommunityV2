using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Web;
using Sheep.Model.Properties;

namespace Sheep.Model.Auth.Providers
{
    /// <summary>
    ///     使用用户名称或电子邮件地址进行身份验证的提供程序。
    /// </summary>
    public class CredentialsAuthProvider : AuthProvider
    {
        #region 校验器类

        /// <summary>
        ///     私有身份验证请求校验器。
        /// </summary>
        private class PrivateAuthValidator : AbstractValidator<Authenticate>
        {
            /// <summary>
            ///     初始化一个新的<see cref="PrivateAuthValidator" />对象。
            /// </summary>
            public PrivateAuthValidator()
            {
                RuleFor(x => x.UserName).NotEmpty().WithMessage(Resources.UserNameOrEmailRequired);
            }
        }

        private class CredentialsAuthValidator : AbstractValidator<Authenticate>
        {
            /// <summary>
            ///     初始化一个新的<see cref="CredentialsAuthValidator" />对象。
            /// </summary>
            public CredentialsAuthValidator()
            {
                RuleFor(x => x.UserName).NotEmpty().WithMessage(Resources.UserNameOrEmailRequired);
                RuleFor(x => x.Password).NotEmpty().WithMessage(Resources.PasswordRequired);
            }
        }

        #endregion

        #region 常量

        public const string Name = "credentials";
        public const string Realm = "/auth/credentials";

        #endregion

        #region 属性

        private readonly PrivateAuthValidator _privateAuthValidator = new PrivateAuthValidator();
        private readonly CredentialsAuthValidator _credentialsAuthValidator = new CredentialsAuthValidator();

        /// <summary>
        ///     是否忽略进程内执行请求的登录密码校验。
        /// </summary>
        public bool SkipPasswordVerificationForInProcessRequests { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="CredentialsAuthProvider" />对象。
        /// </summary>
        public CredentialsAuthProvider()
        {
            Provider = Name;
            AuthRealm = Realm;
        }

        /// <summary>
        ///     初始化一个新的<see cref="CredentialsAuthProvider" />对象。
        /// </summary>
        /// <param name="appSettings">应用程序设置。</param>
        public CredentialsAuthProvider(IAppSettings appSettings)
            : base(appSettings, Realm, Name)
        {
        }

        /// <summary>
        ///     初始化一个新的<see cref="CredentialsAuthProvider" />对象。
        /// </summary>
        /// <param name="appSettings">应用程序设置。</param>
        /// <param name="authRealm">身份验证域。</param>
        /// <param name="provider">提供程序名称。</param>
        public CredentialsAuthProvider(IAppSettings appSettings, string authRealm, string provider)
            : base(appSettings, authRealm, provider)
        {
        }

        #endregion

        #region 重写 AuthProvider

        /// <summary>
        ///     指定的用户身份会话是否已通过身份验证。
        /// </summary>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证第三方令牌。</param>
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
            return session != null && session.IsAuthenticated && !session.UserAuthName.IsNullOrEmpty();
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
            if (SkipPasswordVerificationForInProcessRequests && authService.Request.IsInProcessRequest())
            {
                _privateAuthValidator.ValidateAndThrow(request);
                return AuthenticatePrivateRequest(authService, session, request.UserName, request.Password);
            }
            _credentialsAuthValidator.ValidateAndThrow(request);
            return Authenticate(authService, session, request.UserName, request.Password);
        }

        /// <summary>
        ///     身份验证成功后处理加载数据、引发事件并保存当前用户会话。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <param name="authInfo">身份验证信息。</param>
        /// <returns>身份验证后的处理结果。</returns>
        public override IHttpResult OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            session.AuthProvider = Provider;
            if (session is AuthUserSession userSession)
            {
                LoadUserAuthInfo(userSession, tokens, authInfo);
                HostContext.TryResolve<IAuthMetadataProvider>().SafeAddMetadata(tokens, authInfo);
                LoadUserAuthFilter?.Invoke(userSession, tokens, authInfo);
            }
            var authRepo = GetAuthRepository(authService.Request);
            using (authRepo as IDisposable)
            {
                if (CustomValidationFilter != null)
                {
                    var authContext = new AuthContext
                                      {
                                          Request = authService.Request,
                                          Service = authService,
                                          AuthProvider = this,
                                          Session = session,
                                          AuthTokens = tokens,
                                          AuthInfo = authInfo,
                                          AuthRepository = authRepo
                                      };
                    var response = CustomValidationFilter(authContext);
                    if (response != null)
                    {
                        authService.RemoveSession();
                        return response;
                    }
                }
                if (authRepo != null)
                {
                    if (tokens != null)
                    {
                        if (tokens.Items == null)
                        {
                            tokens.Items = new Dictionary<string, string>();
                        }
                        authInfo?.ForEach((x, y) => tokens.Items[x] = y);
                        session.UserAuthId = authRepo.CreateOrMergeAuthSession(session, tokens).UserAuthId.ToString();
                    }
                    foreach (var authTokens in session.GetAuthTokens())
                    {
                        var authProvider = AuthenticateService.GetAuthProvider(authTokens.Provider);
                        var userAuthProvider = authProvider as OAuthProvider;
                        userAuthProvider?.LoadUserOAuthProvider(session, authTokens);
                    }
                    var httpRes = authService.Request.Response as IHttpResponse;
                    httpRes?.Cookies.AddPermanentCookie(HttpHeaders.XUserAuthId, session.UserAuthId);
                    var failedResult = ValidateAccount(authService, authRepo, session, tokens);
                    if (failedResult != null)
                    {
                        return failedResult;
                    }
                }
            }
            try
            {
                session.IsAuthenticated = true;
                session.OnAuthenticated(authService, session, tokens, authInfo);
                AuthEvents.OnAuthenticated(authService.Request, session, authService, tokens, authInfo);
            }
            finally
            {
                this.SaveSession(authService, session, SessionExpiry);
                authService.Request.Items[Keywords.DidAuthenticate] = true;
            }
            return null;
        }

        #endregion

        #region 身份验证

        /// <summary>
        ///     内部验证指定的用户名称或电子邮件地址及登录密码是否匹配。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="userNameOrEmail">用户名称或电子邮件地址。</param>
        /// <param name="password">登录密码。</param>
        /// <returns>身份验证的结果或响应。</returns>
        protected virtual object AuthenticatePrivateRequest(IServiceBase authService, IAuthSession session, string userNameOrEmail, string password)
        {
            var authRepo = (IUserAuthRepository) GetAuthRepository(authService.Request);
            using (authRepo as IDisposable)
            {
                var userAuth = authRepo.GetUserAuthByUserName(userNameOrEmail);
                if (userAuth == null)
                {
                    throw HttpError.Unauthorized(Resources.InvalidUserNameOrEmailOrPassword);
                }
                if (IsAccountLocked(authRepo, userAuth))
                {
                    throw new AuthenticationException(Resources.UserAccountLocked);
                }
                PopulateSession(authRepo, userAuth, session);
                session.IsAuthenticated = true;
                if (session.UserAuthName == null)
                {
                    session.UserAuthName = userNameOrEmail;
                }
                var response = OnAuthenticated(authService, session, null, null);
                if (response != null)
                {
                    return response;
                }
                return new AuthenticateResponse
                       {
                           UserId = session.UserAuthId,
                           UserName = userNameOrEmail,
                           SessionId = session.Id
                       };
            }
        }

        /// <summary>
        ///     验证指定的用户名称或电子邮件地址及登录密码是否匹配。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="userNameOrEmail">用户名称或电子邮件地址。</param>
        /// <param name="password">登录密码。</param>
        /// <returns>身份验证的结果或响应。</returns>
        protected object Authenticate(IServiceBase authService, IAuthSession session, string userNameOrEmail, string password)
        {
            session = ResetSessionBeforeLogin(authService, session, userNameOrEmail);
            if (TryAuthenticate(authService, userNameOrEmail, password))
            {
                session.IsAuthenticated = true;
                if (session.UserAuthName == null)
                {
                    session.UserAuthName = userNameOrEmail;
                }
                var response = OnAuthenticated(authService, session, null, null);
                if (response != null)
                {
                    return response;
                }
                return new AuthenticateResponse
                       {
                           UserId = session.UserAuthId,
                           UserName = session.UserName,
                           SessionId = session.Id,
                           DisplayName = session.DisplayName
                       };
            }
            throw HttpError.Unauthorized(Resources.InvalidUserNameOrEmailOrPassword);
        }

        /// <summary>
        ///     尝试使用用户名称或电子邮件地址及登录密码进行身份验证。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="userNameOrEmail">用户名称或电子邮件地址。</param>
        /// <param name="password">登录密码。</param>
        /// <returns>如果已通过身份验证，则返回 true，否则返回 false。</returns>
        public virtual bool TryAuthenticate(IServiceBase authService, string userNameOrEmail, string password)
        {
            var authRepo = (IUserAuthRepository) GetAuthRepository(authService.Request);
            using (authRepo as IDisposable)
            {
                var session = authService.GetSession();
                if (authRepo.TryAuthenticate(userNameOrEmail, password, out var userAuth))
                {
                    if (IsAccountLocked(authRepo, userAuth))
                    {
                        throw new AuthenticationException(Resources.UserAccountLocked);
                    }
                    PopulateSession(authRepo, userAuth, session);
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region 重置会话

        /// <summary>
        ///     登录之前重置当前用户会话。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="userNameOrEmail">用户名称或电子邮件地址。</param>
        /// <returns>重置后的会话。</returns>
        protected virtual IAuthSession ResetSessionBeforeLogin(IServiceBase authService, IAuthSession session, string userNameOrEmail)
        {
            if (!LoginMatchesSession(session, userNameOrEmail))
            {
                authService.RemoveSession();
                return authService.GetSession();
            }
            return session;
        }

        #endregion
    }
}