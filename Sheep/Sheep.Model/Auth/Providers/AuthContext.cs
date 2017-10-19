using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Web;

namespace Sheep.Model.Auth.Providers
{
    /// <summary>
    ///     身份验证上下文。
    /// </summary>
    public class AuthContext
    {
        /// <summary>
        ///     服务请求。
        /// </summary>
        public IRequest Request { get; set; }

        /// <summary>
        ///     身份验证服务。
        /// </summary>
        public IServiceBase Service { get; set; }

        /// <summary>
        ///     身份验证提供程序。
        /// </summary>
        public IAuthProvider AuthProvider { get; set; }

        /// <summary>
        ///     用户身份会话。
        /// </summary>
        public IAuthSession Session { get; set; }

        /// <summary>
        ///     身份验证凭据。
        /// </summary>
        public IAuthTokens AuthTokens { get; set; }

        /// <summary>
        ///     身份验证信息表。
        /// </summary>
        public Dictionary<string, string> AuthInfo { get; set; }

        /// <summary>
        ///     身份验证存储库。
        /// </summary>
        public IAuthRepository AuthRepository { get; set; }
    }
}