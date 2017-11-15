using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Accounts;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     绑定用户名称或电子邮件地址及密码帐户服务接口。
    /// </summary>
    public class BindAccountCredentialsService : ChangeAccountService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(BindAccountCredentialsService));

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

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        #endregion

        #region 绑定用户名称或电子邮件地址及密码帐户

        /// <summary>
        ///     绑定用户名称或电子邮件地址及密码帐户。
        /// </summary>
        public async Task<object> Post(AccountBindCredentials request)
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
            var existingUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(session, null);
            if (existingUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, session.UserAuthId));
            }
            var newUserAuth = AuthRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
            newUserAuth.PopulateMissingExtended(existingUserAuth);
            newUserAuth.Meta = existingUserAuth.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingUserAuth.Meta);
            newUserAuth.UserName = request.UserName;
            newUserAuth.Email = request.Email;
            newUserAuth.PrimaryEmail = request.Email;
            var userAuth = await ((IUserAuthRepositoryExtended) AuthRepo).UpdateUserAuthAsync(existingUserAuth, newUserAuth, request.Password);
            ResetCache(userAuth);
            return new AccountBindResponse();
        }

        #endregion
    }
}