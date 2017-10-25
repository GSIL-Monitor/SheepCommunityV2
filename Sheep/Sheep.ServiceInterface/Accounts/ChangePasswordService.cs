using System;
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
    ///     更换密码服务接口。
    /// </summary>
    public class ChangePasswordService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangePasswordService));

        /// <summary>
        ///     自定义校验函数。
        /// </summary>
        public static ValidateFn ValidateFn { get; set; }

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置更换密码的校验器。
        /// </summary>
        public IValidator<AccountChangePassword> AccountResetPasswordValidator { get; set; }

        #endregion

        #region 更换密码

        /// <summary>
        ///     更换密码。
        /// </summary>
        public object Put(AccountChangePassword request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            var validateResponse = ValidateFn?.Invoke(this, HttpMethods.Put, request);
            if (validateResponse != null)
            {
                return validateResponse;
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountResetPasswordValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var existingUserAuth = authRepo.GetUserAuth(GetSession(), null);
                var newUserAuth = authRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
                newUserAuth.PopulateMissingExtended(existingUserAuth);
                authRepo.UpdateUserAuth(existingUserAuth, newUserAuth, request.Password);
            }
            return new AccountChangePasswordResponse();
        }

        #endregion
    }
}