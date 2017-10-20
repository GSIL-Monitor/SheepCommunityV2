using System;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using ServiceStack.Validation;
using ServiceStack.Web;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Identities;
using CredentialsAuthProvider = Sheep.Model.Auth.Providers.CredentialsAuthProvider;

namespace Sheep.ServiceInterface.Identities
{
    /// <summary>
    ///     使用用户名称或电子邮件地址及密码登录服务接口。
    /// </summary>
    public class LoginByCredentialsService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(LoginByCredentialsService));

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
        ///     获取及设置使用用户名称或电子邮件地址及密码登录的校验器。
        /// </summary>
        public IValidator<IdentityLoginByCredentials> IdentityLoginByCredentialsValidator { get; set; }

        #endregion

        #region 使用用户名称或电子邮件地址及密码登录

        /// <summary>
        ///     使用用户名称或电子邮件地址及密码登录。
        /// </summary>
        public object Post(IdentityLoginByCredentials request)
        {
            if (IsAuthenticated)
            {
                throw HttpError.Forbidden(Resources.ReLoginNotAllowed);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                IdentityLoginByCredentialsValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var validateResponse = ValidateFn?.Invoke(this, HttpMethods.Post, request);
            if (validateResponse != null)
            {
                return validateResponse;
            }
            using (var authService = ResolveService<AuthenticateService>())
            {
                var authResult = authService.Post(new Authenticate
                                                  {
                                                      provider = CredentialsAuthProvider.Name,
                                                      UserName = request.UserNameOrEmail,
                                                      Password = request.Password,
                                                      RememberMe = true
                                                  });
                if (authResult is IHttpError)
                {
                    throw (Exception) authResult;
                }
                if (authResult is AuthenticateResponse authResponse)
                {
                    return new IdentityLoginResponse
                           {
                               SessionId = authResponse.SessionId,
                               UserId = authResponse.UserId.ToInt(),
                               NewlyCreated = authResponse.Meta.Get<bool>("NewlyCreated")
                           };
                }
                return authResult;
            }
        }

        #endregion
    }
}