using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;

namespace Sheep.Model.Auth.Providers
{
    /// <summary>
    ///     OAuth 身份验证的提供程序。
    /// </summary>
    public abstract class OAuthProvider : AuthProvider
    {
        #region 属性

        /// <summary>
        ///     消费者的关键字。
        /// </summary>
        public string ConsumerKey { get; set; }

        /// <summary>
        ///     消费者的密钥。
        /// </summary>
        public string ConsumerSecret { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="OAuthProvider" />对象。
        /// </summary>
        protected OAuthProvider()
        {
        }

        /// <summary>
        ///     初始化一个新的<see cref="OAuthProvider" />对象。
        /// </summary>
        /// <param name="appSettings">应用程序设置。</param>
        /// <param name="authRealm">身份验证域。</param>
        /// <param name="provider">提供程序名称。</param>
        protected OAuthProvider(IAppSettings appSettings, string authRealm, string provider)
            : this(appSettings, authRealm, provider, "ConsumerKey", "ConsumerSecret")
        {
        }

        /// <summary>
        ///     初始化一个新的<see cref="OAuthProvider" />对象。
        /// </summary>
        /// <param name="appSettings">应用程序设置。</param>
        /// <param name="authRealm">身份验证域。</param>
        /// <param name="provider">提供程序名称。</param>
        /// <param name="consumerKeyName">消费者的关键字的配置名称。</param>
        /// <param name="consumerSecretName">消费者的密钥的配置名称。</param>
        protected OAuthProvider(IAppSettings appSettings, string authRealm, string provider, string consumerKeyName, string consumerSecretName)
        {
            AuthRealm = appSettings != null ? appSettings.Get("OAuthRealm", authRealm) : authRealm;
            Provider = provider;
            if (appSettings != null)
            {
                ConsumerKey = appSettings.GetString($"oauth.{provider}.{consumerKeyName}");
                ConsumerSecret = appSettings.GetString($"oauth.{provider}.{consumerSecretName}");
                CallbackUrl = appSettings.GetString($"oauth.{provider}.CallbackUrl") ?? FallbackConfig(appSettings.GetString("oauth.CallbackUrl"));
                SaveExtendedUserInfo = appSettings.Get($"oauth.{provider}.SaveExtendedUserInfo", true);
            }
        }

        #endregion

        #region 重写 AuthProvider

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
            return session != null && session.IsAuthenticated && !string.IsNullOrEmpty(tokens?.AccessTokenSecret);
        }

        #endregion

        #region 初始化凭据

        /// <summary>
        ///     初始化一个OAuth的身份验证凭据。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="request">服务请求。</param>
        /// <returns></returns>
        protected IAuthTokens Init(IServiceBase authService, ref IAuthSession session, Authenticate request)
        {
            if (CallbackUrl.IsNullOrEmpty())
            {
                CallbackUrl = authService.Request.AbsoluteUri;
            }
            var tokens = session.GetAuthTokens(Provider);
            if (tokens == null)
            {
                tokens = new AuthTokens
                         {
                             Provider = Provider
                         };
                session.AddAuthToken(tokens);
            }
            return tokens;
        }

        /// <summary>
        ///     身份验证的入口点。 在AuthService中运行，因此需要将异常处理当作正常处理流程。
        /// </summary>
        /// <param name="authService">身份验证服务。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="request">身份验证请求。</param>
        /// <returns>身份验证的结果或响应。</returns>
        public abstract override object Authenticate(IServiceBase authService, IAuthSession session, Authenticate request);

        #endregion

        #region 用户身份操作

        /// <summary>
        ///     将身份凭据中的信息加载到用户身份会话中。
        /// </summary>
        /// <param name="session">用户身份会话。</param>
        /// <param name="tokens">身份验证凭据。</param>
        public virtual void LoadUserOAuthProvider(IAuthSession session, IAuthTokens tokens)
        {
        }

        #endregion
    }
}