using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.ServiceInterface.Accounts.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Accounts;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     显示一个帐户服务接口。
    /// </summary>
    public class ShowAccountService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowAccountService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个帐户的校验器。
        /// </summary>
        public IValidator<AccountShow> AccountShowValidator { get; set; }

        #endregion

        #region 显示一个帐户

        /// <summary>
        ///     显示一个帐户。
        /// </summary>
        public async Task<object> Get(AccountShow request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var session = GetSession();
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var existingUserAuth = await ((IUserAuthRepositoryExtended) authRepo).GetUserAuthAsync(session, null);
                if (existingUserAuth == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.UserNotFound, session.UserAuthId));
                }
                var accountDto = existingUserAuth.MapToAccountDto();
                return new AccountShowResponse
                       {
                           Account = accountDto
                       };
            }
        }

        #endregion
    }
}