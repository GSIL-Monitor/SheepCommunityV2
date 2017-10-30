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
    ///     绑定用户名称或电子邮件地址及密码帐户服务接口。
    /// </summary>
    public class BindCredentialsService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(BindCredentialsService));

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
        ///     获取及设置绑定用户名称或电子邮件地址及密码帐户的校验器。
        /// </summary>
        public IValidator<AccountBindCredentials> AccountBindCredentialsValidator { get; set; }

        #endregion

        #region 绑定用户名称或电子邮件地址及密码帐户

        /// <summary>
        ///     绑定用户名称或电子邮件地址及密码帐户。
        /// </summary>
        public object Post(AccountBindCredentials request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GetPlugin<AuthFeature>()?.SaveUserNamesInLowerCase == true)
            {
                if (request.UserName != null)
                {
                    request.UserName = request.UserName.ToLower();
                }
                if (request.Email != null)
                {
                    request.Email = request.Email.ToLower();
                }
            }
            var validateResponse = ValidateFn?.Invoke(this, HttpMethods.Post, request);
            if (validateResponse != null)
            {
                return validateResponse;
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountBindCredentialsValidator.ValidateAndThrow(request, ApplyTo.Post);
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
                var newUserAuth = MapToUserAuth(authRepo, existingUserAuth, request);
                authRepo.UpdateUserAuth(existingUserAuth, newUserAuth, request.Password);
            }
            return new AccountBindResponse();
        }

        #endregion

        #region 转换成用户身份

        /// <summary>
        ///     将绑定的请求转换成用户身份。
        /// </summary>
        public IUserAuth MapToUserAuth(IAuthRepository authRepo, IUserAuth existingUserAuth, AccountBindCredentials request)
        {
            var newUserAuth = authRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
            newUserAuth.PopulateMissingExtended(existingUserAuth);
            newUserAuth.Meta = existingUserAuth.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingUserAuth.Meta);
            newUserAuth.UserName = request.UserName;
            newUserAuth.Email = request.Email;
            newUserAuth.PrimaryEmail = request.Email;
            return newUserAuth;
        }

        #endregion
    }
}