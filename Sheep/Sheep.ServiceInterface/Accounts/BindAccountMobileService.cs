using System;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Web;
using Sheep.Model.Auth.Providers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Accounts;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     绑定手机号码服务接口。
    /// </summary>
    public class BindAccountMobileService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(BindAccountMobileService));

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
        ///     获取及设置绑定手机号码的校验器。
        /// </summary>
        public IValidator<AccountBindMobile> AccountBindMobileValidator { get; set; }

        #endregion

        #region 绑定手机号码

        /// <summary>
        ///     绑定手机号码。
        /// </summary>
        public object Post(AccountBindMobile request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            var validateResponse = ValidateFn?.Invoke(this, HttpMethods.Post, request);
            if (validateResponse != null)
            {
                return validateResponse;
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    AccountBindMobileValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            using (var authService = ResolveService<AuthenticateService>())
            {
                var authResult = authService.Post(new Authenticate
                                                  {
                                                      provider = MobileAuthProvider.Name,
                                                      UserName = request.PhoneNumber,
                                                      Password = request.Token,
                                                      State = "Bind",
                                                      RememberMe = true
                                                  });
                if (authResult is IHttpError)
                {
                    throw (Exception) authResult;
                }
                if (authResult is AuthenticateResponse)
                {
                    return new AccountBindResponse();
                }
                return authResult;
            }
        }

        #endregion
    }
}