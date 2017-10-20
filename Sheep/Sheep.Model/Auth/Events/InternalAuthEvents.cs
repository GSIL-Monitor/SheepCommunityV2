using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Logging;
using ServiceStack.Web;

namespace Sheep.Model.Auth.Events
{
    /// <summary>
    ///     内部的注册登录注销的身份验证事件。
    /// </summary>
    public class InternalAuthEvents : AuthEvents
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(InternalAuthEvents));

        #endregion

        #region 重写 AuthEvents

        /// <summary>
        ///     注册成功后处理。
        /// </summary>
        /// <param name="httpReq">服务请求。</param>
        /// <param name="session">用户身份会话。</param>
        /// <param name="registrationService">用户注册服务。</param>
        public override void OnRegistered(IRequest httpReq, IAuthSession session, IServiceBase registrationService)
        {
        }

        public override void OnAuthenticated(IRequest httpReq, IAuthSession session, IServiceBase authService, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
        }

        #endregion
    }
}