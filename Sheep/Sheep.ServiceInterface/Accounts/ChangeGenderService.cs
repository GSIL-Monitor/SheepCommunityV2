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
    ///     更改性别服务接口。
    /// </summary>
    public class ChangeGenderService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeGenderService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置更改性别的校验器。
        /// </summary>
        public IValidator<AccountChangeGender> AccountChangeGenderValidator { get; set; }

        #endregion

        #region 更改性别

        /// <summary>
        ///     更改性别。
        /// </summary>
        public object Put(AccountChangeGender request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountChangeGenderValidator.ValidateAndThrow(request, ApplyTo.Put);
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
                newUserAuth.Gender = request.Gender.HasValue ? (request.Gender.Value == 0 ? "女" : "男") : null;
                ((IUserAuthRepository) authRepo).UpdateUserAuth(existingUserAuth, newUserAuth);
                return new AccountChangeGenderResponse();
            }
        }

        #endregion
    }
}