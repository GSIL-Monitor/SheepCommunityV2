using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Auth.Providers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Accounts;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     解除绑定微博帐号服务接口。
    /// </summary>
    public class UnbindAccountWeiboService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UnbindAccountWeiboService));

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
        ///     获取及设置解除绑定微博帐号的校验器。
        /// </summary>
        public IValidator<AccountUnbindWeibo> AccountUnbindWeiboValidator { get; set; }

        #endregion

        #region 解除绑定微博帐号

        /// <summary>
        ///     解除绑定微博帐号。
        /// </summary>
        public async Task<object> Delete(AccountUnbindWeibo request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            var validateResponse = ValidateFn?.Invoke(this, HttpMethods.Delete, request);
            if (validateResponse != null)
            {
                return validateResponse;
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountUnbindWeiboValidator.ValidateAndThrow(request, ApplyTo.Delete);
            }
            var session = GetSession();
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                await ((IUserAuthRepositoryExtended) authRepo).DeleteUserAuthDetailsByProviderAsync(WeiboAuthProvider.Name, request.WeiboUserId);
            }
            session.ProviderOAuthAccess.RemoveAll(x => x.Provider == WeiboAuthProvider.Name && x.UserId == request.WeiboUserId);
            this.SaveSession(session);
            return new AccountUnbindResponse();
        }

        #endregion
    }
}