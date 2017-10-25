using System;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Logging;
using ServiceStack.Web;
using Sheep.ServiceModel.Accounts;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     退出登录服务接口。
    /// </summary>
    public class LogoutService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(LogoutService));

        #endregion

        #region 退出登录

        /// <summary>
        ///     退出登录。
        /// </summary>
        public object Post(AccountLogout request)
        {
            using (var authService = ResolveService<AuthenticateService>())
            {
                var authResult = authService.Post(new Authenticate
                                                  {
                                                      provider = AuthenticateService.LogoutAction
                                                  });
                if (authResult is IHttpError)
                {
                    throw (Exception) authResult;
                }
                if (authResult is AuthenticateResponse)
                {
                    return new AccountLogoutResponse();
                }
                return authResult;
            }
        }

        #endregion
    }
}