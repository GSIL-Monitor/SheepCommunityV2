using System;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using ServiceStack.Web;
using Sheep.Model.Auth.Providers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Accounts;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     绑定微博帐号服务接口。
    /// </summary>
    public class BindWeiboService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(BindWeiboService));

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
        ///     获取及设置绑定微博帐号的校验器。
        /// </summary>
        public IValidator<AccountBindWeibo> AccountBindWeiboValidator { get; set; }

        #endregion

        #region 绑定微博帐号

        /// <summary>
        ///     绑定微博帐号。
        /// </summary>
        public object Post(AccountBindWeibo request)
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
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountBindWeiboValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            using (var authService = ResolveService<AuthenticateService>())
            {
                var authResult = authService.Post(new Authenticate
                                                  {
                                                      provider = WeiboAuthProvider.Name,
                                                      UserName = request.WeiboUserId,
                                                      AccessToken = request.AccessToken,
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