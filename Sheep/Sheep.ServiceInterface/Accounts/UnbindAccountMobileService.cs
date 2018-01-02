using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Auth.Providers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Accounts;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     解除绑定手机号码服务接口。
    /// </summary>
    public class UnbindAccountMobileService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UnbindAccountMobileService));

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
        ///     获取及设置解除绑定手机号码的校验器。
        /// </summary>
        public IValidator<AccountUnbindMobile> AccountUnbindMobileValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        #endregion

        #region 解除绑定手机号码

        /// <summary>
        ///     解除绑定手机号码。
        /// </summary>
        public async Task<object> Delete(AccountUnbindMobile request)
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
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    AccountUnbindMobileValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var session = GetSession();
            await ((IUserAuthRepositoryExtended) AuthRepo).DeleteUserAuthDetailsByProviderAsync(MobileAuthProvider.Name, request.PhoneNumber);
            session.ProviderOAuthAccess.RemoveAll(x => x.Provider == MobileAuthProvider.Name && x.UserId == request.PhoneNumber);
            this.SaveSession(session);
            return new AccountUnbindResponse();
        }

        #endregion
    }
}