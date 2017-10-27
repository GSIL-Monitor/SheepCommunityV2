using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Web;
using Sheep.Model.Properties;

namespace Sheep.Model.Auth.Providers
{
    /// <summary>
    ///     身份验证的提供程序。
    /// </summary>
    public abstract class AuthProvider : IAuthProvider
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(AuthProvider));

        #endregion

        #region 静态方法

        /// <summary>
        ///     指定的用户名称或电子邮件地址是否与用户身份会话中的相关属性匹配。
        /// </summary>
        /// <param name="session">用户身份会话。</param>
        /// <param name="userNameOrEmail">用户名称或电子邮件地址。</param>
        /// <returns>如果相等，则返回 true，否则返回 false。</returns>
        protected static bool LoginMatchesSession(IAuthSession session, string userNameOrEmail)
        {
            if (session == null || userNameOrEmail == null)
            {
                return false;
            }
            var isEmail = userNameOrEmail.Contains("@");
            if (isEmail)
            {
                if (!userNameOrEmail.EqualsIgnoreCase(session.Email))
                {
                    return false;
                }
            }
            else
            {
                if (!userNameOrEmail.EqualsIgnoreCase(session.UserAuthName))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 属性

        /// <summary>
        ///     会话过期时间。
        /// </summary>
        public TimeSpan? SessionExpiry { get; set; }

        /// <summary>
        ///     是否持久化会话。
        /// </summary>
        public bool PersistSession { get; set; }

        /// <summary>
        ///     是否保存扩展的用户信息。
        /// </summary>
        public bool SaveExtendedUserInfo { get; set; }

        /// <summary>
        ///     加载用户身份信息的过滤器。
        /// </summary>
        public Action<AuthUserSession, IAuthTokens, Dictionary<string, string>> LoadUserAuthFilter { get; set; }

        /// <summary>
        ///     自定义身份验证校验的过滤器。
        /// </summary>
        public Func<AuthContext, IHttpResult> CustomValidationFilter { get; set; }

        /// <summary>
        ///     获取身份验证事件。
        /// </summary>
        public IAuthEvents AuthEvents
        {
            get
            {
                return HostContext.TryResolve<IAuthEvents>() ?? new AuthEvents();
            }
        }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="AuthProvider" />对象。
        /// </summary>
        protected AuthProvider()
        {
            PersistSession = !GetType().HasInterface(typeof(IAuthWithRequest));
        }

        /// <summary>
        ///     初始化一个新的<see cref="AuthProvider" />对象。
        /// </summary>
        /// <param name="appSettings">应用程序设置。</param>
        /// <param name="authRealm">身份验证域。</param>
        /// <param name="provider">提供程序名称。</param>
        protected AuthProvider(IAppSettings appSettings, string authRealm, string provider)
            : this()
        {
            AuthRealm = appSettings != null ? appSettings.Get("OAuthRealm", authRealm) : authRealm;
            Provider = provider;
            if (appSettings != null)
            {
                CallbackUrl = appSettings.GetString($"oauth.{provider}.CallbackUrl") ?? FallbackConfig(appSettings.GetString("oauth.CallbackUrl"));
            }
        }

        #endregion

        #region IAuthProvider 接口实现

        /// <summary>
        ///     身份验证域。
        /// </summary>
        public string AuthRealm { get; set; }

        /// <summary>
        ///     提供程序名称。
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        ///     回调地址。
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        ///     指定的用户身份会话是否已通过身份验证。
        /// </summary>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <param name="request">身份验证请求。</param>
        /// <returns>如果已授权，则返回 true，否则返回 false。</returns>
        public abstract bool IsAuthorized(IAuthSession session, IAuthTokens tokens, Authenticate request = null);

        /// <summary>
        ///     身份验证的入口点。 在AuthService中运行，因此需要将异常处理当作正常处理流程。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="request">身份验证请求。</param>
        /// <returns>身份验证的结果或响应。</returns>
        public abstract object Authenticate(IServiceBase authService, IAuthSession session, Authenticate request);

        /// <summary>
        ///     移除用户会话。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="request">身份验证请求。</param>
        /// <returns>默认的身份验证响应。</returns>
        public virtual object Logout(IServiceBase authService, Authenticate request)
        {
            var feature = HostContext.GetPlugin<AuthFeature>();
            var session = authService.GetSession();
            session.OnLogout(authService);
            AuthEvents.OnLogout(authService.Request, session, authService);
            authService.RemoveSession();
            if (feature != null && feature.DeleteSessionCookiesOnLogout)
            {
                authService.Request.Response.DeleteSessionCookies();
                authService.Request.Response.DeleteJwtCookie();
            }
            return new AuthenticateResponse();
        }

        #endregion

        #region 后备配置

        /// <summary>
        ///     允许指定一个全局后备配置 (如果存在) 以提供程序作为第一个参数进行格式化。
        ///     示例：在 AppSetting 中设置 TwitterAuthProvider：oauth.CallbackUrl="http://localhost:11001/auth/{0}"
        ///     结果应是：oauth.CallbackUrl="http://localhost:11001/auth/twitter"
        /// </summary>
        protected string FallbackConfig(string fallback)
        {
            return fallback?.Fmt(Provider);
        }

        #endregion

        #region 检测帐户操作

        /// <summary>
        ///     校验帐户是否唯一及帐户是否锁定。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="authRepo">身份验证存储库。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <returns>校验结果。</returns>
        protected virtual IHttpResult ValidateAccount(IServiceBase authService, IAuthRepository authRepo, IAuthSession session, IAuthTokens tokens)
        {
            var userAuth = authRepo.GetUserAuth(session, tokens);
            var authFeature = HostContext.GetPlugin<AuthFeature>();
            if (authFeature != null && authFeature.ValidateUniqueUserNames && UserNameAlreadyExists(authRepo, userAuth, tokens))
            {
                return new HttpResult(HttpStatusCode.BadRequest, Resources.UserNameAlreadyExists);
            }
            if (authFeature != null && authFeature.ValidateUniqueEmails && EmailAlreadyExists(authRepo, userAuth, tokens))
            {
                return new HttpResult(HttpStatusCode.BadRequest, Resources.EmailAlreadyExists);
            }
            if (IsAccountLocked(authRepo, userAuth, tokens))
            {
                return new HttpResult(HttpStatusCode.Unauthorized, Resources.UserAccountLocked);
            }
            return null;
        }

        /// <summary>
        ///     检测用户名称是否存在。
        /// </summary>
        /// <param name="authRepo">身份验证存储库。</param>
        /// <param name="userAuth">用户身份。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <returns>如果用户名称存在，则返回 true，否则返回 false。</returns>
        protected virtual bool UserNameAlreadyExists(IAuthRepository authRepo, IUserAuth userAuth, IAuthTokens tokens = null)
        {
            if (tokens?.UserName != null)
            {
                var userWithUserName = authRepo.GetUserAuthByUserName(tokens.UserName);
                if (userWithUserName == null)
                {
                    return false;
                }
                var isAnotherUser = userAuth == null || userAuth.Id != userWithUserName.Id;
                return isAnotherUser;
            }
            return false;
        }

        /// <summary>
        ///     检测电子邮件地址是否存在。
        /// </summary>
        /// <param name="authRepo">身份验证存储库。</param>
        /// <param name="userAuth">用户身份。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <returns>如果电子邮件地址存在，则返回 true，否则返回 false。</returns>
        protected virtual bool EmailAlreadyExists(IAuthRepository authRepo, IUserAuth userAuth, IAuthTokens tokens = null)
        {
            if (tokens?.Email != null)
            {
                var userWithEmail = authRepo.GetUserAuthByUserName(tokens.Email);
                if (userWithEmail == null)
                {
                    return false;
                }
                var isAnotherUser = userAuth == null || userAuth.Id != userWithEmail.Id;
                return isAnotherUser;
            }
            return false;
        }

        /// <summary>
        ///     检测帐户是否被锁定。
        /// </summary>
        /// <param name="authRepo">身份验证存储库。</param>
        /// <param name="userAuth">用户身份。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <returns>true 表示已通过锁定，否则为 false。</returns>
        protected virtual bool IsAccountLocked(IAuthRepository authRepo, IUserAuth userAuth, IAuthTokens tokens = null)
        {
            if (userAuth != null)
            {
                if (userAuth.LockedDate != null)
                {
                    return true;
                }
                if (userAuth.Meta != null && userAuth.Meta.TryGetValue("AccountStatus", out var accountStatus) && accountStatus != null && accountStatus != "Approved")
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 用户身份操作

        /// <summary>
        ///     获取身份验证存储库。
        /// </summary>
        /// <param name="req">服务请求。</param>
        /// <returns>身份验证存储库。</returns>
        protected virtual IAuthRepository GetAuthRepository(IRequest req)
        {
            return HostContext.AppHost.GetAuthRepository(req);
        }

        /// <summary>
        ///     将用户身份信息表加载到用户身份会话及身份凭据中。
        /// </summary>
        /// <param name="userSession">用户身份会话。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <param name="authInfo">身份验证信息。</param>
        protected virtual void LoadUserAuthInfo(AuthUserSession userSession, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
        }

        /// <summary>
        ///     保存用户身份到存储库中。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="authRepo">身份验证存储库。</param>
        /// <param name="tokens">身份验证凭据。</param>
        protected virtual void SaveUserAuth(IServiceBase authService, IAuthSession session, IAuthRepository authRepo, IAuthTokens tokens)
        {
            if (authRepo == null)
            {
                return;
            }
            if (tokens != null)
            {
                session.UserAuthId = authRepo.CreateOrMergeAuthSession(session, tokens).UserAuthId.ToString();
            }
            authRepo.LoadUserAuth(session, tokens);
            foreach (var authTokens in session.GetAuthTokens())
            {
                var authProvider = AuthenticateService.GetAuthProvider(authTokens.Provider);
                var userAuthProvider = authProvider as OAuthProvider;
                userAuthProvider?.LoadUserOAuthProvider(session, authTokens);
            }
            authRepo.SaveUserAuth(session);
            var httpRes = authService.Request.Response as IHttpResponse;
            httpRes?.Cookies.AddPermanentCookie(HttpHeaders.XUserAuthId, session.UserAuthId);
            OnSaveUserAuth(authService, session);
        }

        /// <summary>
        ///     保存用户身份到存储库中后处理数据。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        public virtual void OnSaveUserAuth(IServiceBase authService, IAuthSession session)
        {
        }

        #endregion

        #region 身份会话操作

        /// <summary>
        ///     将用户身份信息填充到用户会话中。
        /// </summary>
        /// <param name="authRepo">身份验证存储库。</param>
        /// <param name="userAuth">用户身份。</param>
        /// <param name="session">用户身份会话。</param>
        internal void PopulateSession(IUserAuthRepository authRepo, IUserAuth userAuth, IAuthSession session)
        {
            if (authRepo == null)
            {
                return;
            }
            var holdSessionId = session.Id;
            session.PopulateWith(userAuth);
            session.Id = holdSessionId;
            session.UserAuthId = userAuth.Id.ToString(CultureInfo.InvariantCulture);
            session.ProviderOAuthAccess = authRepo.GetUserAuthDetails(session.UserAuthId).ConvertAll(x => (IAuthTokens) x);
        }

        private static long s_TransientUserAuthId;
        private static readonly ConcurrentDictionary<string, long> s_TransientUserIdsMap = new ConcurrentDictionary<string, long>();

        /// <summary>
        ///     当身份验证存储库不存在的时候，将身份凭据合并到用户身份会话中。
        /// </summary>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <returns>用户身份编号。</returns>
        public virtual string CreateOrMergeAuthSession(IAuthSession session, IAuthTokens tokens)
        {
            if (session.UserName.IsNullOrEmpty())
            {
                session.UserName = tokens.UserName;
            }
            if (session.DisplayName.IsNullOrEmpty())
            {
                session.DisplayName = tokens.DisplayName;
            }
            if (session.Email.IsNullOrEmpty())
            {
                session.Email = tokens.Email;
            }
            var authTokens = session.GetAuthTokens(tokens.Provider);
            if (authTokens != null && authTokens.UserId == tokens.UserId)
            {
                if (!authTokens.UserName.IsNullOrEmpty())
                {
                    session.UserName = authTokens.UserName;
                }
                if (!authTokens.DisplayName.IsNullOrEmpty())
                {
                    session.DisplayName = authTokens.DisplayName;
                }
                if (!authTokens.Email.IsNullOrEmpty())
                {
                    session.Email = authTokens.Email;
                }
                if (!authTokens.FirstName.IsNullOrEmpty())
                {
                    session.FirstName = authTokens.FirstName;
                }
                if (!authTokens.LastName.IsNullOrEmpty())
                {
                    session.LastName = authTokens.LastName;
                }
            }
            var key = $"{tokens.Provider}:{tokens.UserId ?? tokens.UserName}";
            return s_TransientUserIdsMap.GetOrAdd(key, k => Interlocked.Increment(ref s_TransientUserAuthId)).ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region 身份验证后处理

        /// <summary>
        ///     身份验证成功后处理加载数据、引发事件并保存当前用户会话。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证凭据。</param>
        /// <param name="authInfo">身份验证信息。</param>
        /// <returns>身份验证后的处理结果。</returns>
        public virtual IHttpResult OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            session.AuthProvider = Provider;
            if (session is AuthUserSession userSession)
            {
                LoadUserAuthInfo(userSession, tokens, authInfo);
                HostContext.TryResolve<IAuthMetadataProvider>().SafeAddMetadata(tokens, authInfo);
                LoadUserAuthFilter?.Invoke(userSession, tokens, authInfo);
            }
            var hasTokens = tokens != null && authInfo != null;
            if (hasTokens && SaveExtendedUserInfo)
            {
                if (tokens.Items == null)
                {
                    tokens.Items = new Dictionary<string, string>();
                }
                authInfo.ForEach((x, y) => tokens.Items[x] = y);
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
                    var failedResult = ValidateAccount(authService, authRepo, session, tokens);
                    if (failedResult != null)
                    {
                        authService.RemoveSession();
                        return failedResult;
                    }
                    if (hasTokens)
                    {
                        var authDetails = authRepo.CreateOrMergeAuthSession(session, tokens);
                        session.UserAuthId = authDetails.UserAuthId.ToString();
                        var firstTimeAuthenticated = authDetails.CreatedDate == authDetails.ModifiedDate;
                        if (firstTimeAuthenticated)
                        {
                            session.OnRegistered(authService.Request, session, authService);
                            AuthEvents.OnRegistered(authService.Request, session, authService);
                        }
                    }
                    authRepo.LoadUserAuth(session, tokens);
                    foreach (var authTokens in session.GetAuthTokens())
                    {
                        var authProvider = AuthenticateService.GetAuthProvider(authTokens.Provider);
                        var userAuthProvider = authProvider as OAuthProvider;
                        userAuthProvider?.LoadUserOAuthProvider(session, authTokens);
                    }
                    var httpRes = authService.Request.Response as IHttpResponse;
                    if (session.UserAuthId != null)
                    {
                        httpRes?.Cookies.AddPermanentCookie(HttpHeaders.XUserAuthId, session.UserAuthId);
                    }
                }
                else
                {
                    if (hasTokens)
                    {
                        session.UserAuthId = CreateOrMergeAuthSession(session, tokens);
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

        #region 错误结果处理

        /// <summary>
        ///     将指定的服务器错误结果转换为客户端错误结果。
        /// </summary>
        /// <param name="failedResult">服务器错误结果。</param>
        /// <returns>转换后的客户端错误结果。</returns>
        protected virtual object ConvertToClientError(object failedResult)
        {
            if (failedResult is IHttpResult httpRes)
            {
                if (httpRes.Headers.TryGetValue(HttpHeaders.Location, out var location))
                {
                    var parts = location.SplitOnLast("f=");
                    if (parts.Length == 2)
                    {
                        return new HttpError(HttpStatusCode.BadRequest, parts[1], parts[1].SplitCamelCase());
                    }
                }
            }
            return failedResult;
        }

        #endregion
    }
}