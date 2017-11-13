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
    ///     使用手机号码登录服务接口。
    /// </summary>
    public class LoginAccountByMobileService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(LoginAccountByMobileService));

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
        ///     获取及设置使用手机号码登录的校验器。
        /// </summary>
        public IValidator<AccountLoginByMobile> AccountLoginByMobileValidator { get; set; }

        #endregion

        #region 使用手机号码登录

        /// <summary>
        ///     使用手机号码登录。
        /// </summary>
        public object Post(AccountLoginByMobile request)
        {
            if (IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.ReLoginNotAllowed);
            }
            var validateResponse = ValidateFn?.Invoke(this, HttpMethods.Post, request);
            if (validateResponse != null)
            {
                return validateResponse;
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountLoginByMobileValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            using (var authService = ResolveService<AuthenticateService>())
            {
                var authResult = authService.Post(new Authenticate
                                                  {
                                                      provider = MobileAuthProvider.Name,
                                                      UserName = request.PhoneNumber,
                                                      Password = request.Token,
                                                      State = "Login",
                                                      RememberMe = true
                                                  });
                if (authResult is IHttpError)
                {
                    throw (Exception) authResult;
                }
                if (authResult is AuthenticateResponse authResponse)
                {
                    var newlyCreated = false;
                    var authRepo = HostContext.AppHost.GetAuthRepository(Request);
                    using (authRepo as IDisposable)
                    {
                        var userAuth = authRepo.GetUserAuth(authResponse.UserId);
                        if (userAuth == null)
                        {
                            throw HttpError.NotFound(string.Format(Resources.UserNotFound, authResponse.UserId));
                        }
                        if (userAuth.CreatedDate == userAuth.ModifiedDate)
                        {
                            newlyCreated = true;
                        }
                    }
                    return new AccountLoginResponse
                           {
                               SessionId = authResponse.SessionId,
                               UserId = authResponse.UserId.ToInt(),
                               NewlyCreated = newlyCreated
                           };
                }
                return authResult;
            }
        }

        #endregion
    }
}