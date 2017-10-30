using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Accounts;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     更改显示名称服务接口。
    /// </summary>
    public class ChangeDisplayNameService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeDisplayNameService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置更改显示名称的校验器。
        /// </summary>
        public IValidator<AccountChangeDisplayName> AccountChangeDisplayNameValidator { get; set; }

        #endregion

        #region 更改显示名称

        /// <summary>
        ///     更改显示名称。
        /// </summary>
        public object Put(AccountChangeDisplayName request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountChangeDisplayNameValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var session = GetSession();
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var existingUserAuth = authRepo.GetUserAuth(session, null);
                if (existingUserAuth == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.UserNotFound, session.UserAuthId));
                }
                var newUserAuth = authRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
                newUserAuth.PopulateMissingExtended(existingUserAuth);
                newUserAuth.Meta = existingUserAuth.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingUserAuth.Meta);
                newUserAuth.DisplayName = request.DisplayName;
                ((IUserAuthRepository) authRepo).UpdateUserAuth(existingUserAuth, newUserAuth);
                return new AccountChangeDisplayNameResponse();
            }
        }

        #endregion
    }
}