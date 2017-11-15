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
    ///     更改性别服务接口。
    /// </summary>
    public class ChangeAccountGenderService : ChangeAccountService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeAccountGenderService));

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

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        #endregion

        #region 更改性别

        /// <summary>
        ///     更改性别。
        /// </summary>
        public async Task<object> Put(AccountChangeGender request)
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
            var existingUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(session, null);
            if (existingUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, session.UserAuthId));
            }
            var newUserAuth = AuthRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
            newUserAuth.PopulateMissingExtended(existingUserAuth);
            newUserAuth.Meta = existingUserAuth.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingUserAuth.Meta);
            newUserAuth.Gender = request.Gender;
            var userAuth = await ((IUserAuthRepositoryExtended) AuthRepo).UpdateUserAuthAsync(existingUserAuth, newUserAuth);
            ResetCache(userAuth);
            return new AccountChangeGenderResponse();
        }

        #endregion
    }
}