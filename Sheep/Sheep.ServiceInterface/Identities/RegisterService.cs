using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using ServiceStack.Web;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Identities;
using CredentialsAuthProvider = Sheep.Model.Auth.Providers.CredentialsAuthProvider;

namespace Sheep.ServiceInterface.Identities
{
    /// <summary>
    ///     使用用户名称或电子邮件地址及密码注册服务接口。
    /// </summary>
    public class RegisterService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RegisterService));

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
        ///     获取及设置身份验证事件。
        /// </summary>
        public IAuthEvents AuthEvents { get; set; }

        /// <summary>
        ///     获取及设置使用用户名称或电子邮件地址及密码注册的校验器。
        /// </summary>
        public IValidator<IdentityRegister> IdentityRegisterValidator { get; set; }

        #endregion

        #region 使用用户名称或电子邮件地址及密码注册

        /// <summary>
        ///     使用用户名称或电子邮件地址及密码注册。
        /// </summary>
        public object Post(IdentityRegister request)
        {
            if (IsAuthenticated)
            {
                throw HttpError.Forbidden(Resources.ReRegisterNotAllowed);
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
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                IdentityRegisterValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var validateResponse = ValidateFn?.Invoke(this, HttpMethods.Post, request);
            if (validateResponse != null)
            {
                return validateResponse;
            }
            IUserAuth userAuth;
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                userAuth = authRepo.CreateUserAuth(MapToUserAuth(authRepo, request), request.Password);
            }
            IdentityRegisterResponse response = null;
            if (request.AutoLogin.GetValueOrDefault())
            {
                using (var authService = ResolveService<AuthenticateService>())
                {
                    var authResult = authService.Post(new Authenticate
                                                      {
                                                          provider = CredentialsAuthProvider.Name,
                                                          UserName = userAuth.UserName ?? userAuth.Email,
                                                          Password = request.Password,
                                                          RememberMe = true
                                                      });
                    if (authResult is IHttpError)
                    {
                        throw (Exception) authResult;
                    }
                    if (authResult is AuthenticateResponse authResponse)
                    {
                        response = new IdentityRegisterResponse
                                   {
                                       SessionId = authResponse.SessionId,
                                       UserId = authResponse.UserId.ToInt()
                                   };
                    }
                }
            }
            var session = GetSession();
            session.PopulateSession(userAuth, new List<IAuthTokens>());
            session.OnRegistered(Request, session, this);
            AuthEvents?.OnRegistered(Request, session, this);
            return response ??
                   new IdentityRegisterResponse
                   {
                       UserId = userAuth.Id
                   };
        }

        #endregion

        #region 转换成用户身份

        /// <summary>
        ///     将注册身份的请求转换成用户身份。
        /// </summary>
        public IUserAuth MapToUserAuth(IAuthRepository authRepo, IdentityRegister request)
        {
            var newUserAuth = authRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
            newUserAuth.UserName = request.UserName;
            newUserAuth.Email = request.Email;
            newUserAuth.PrimaryEmail = request.Email;
            return newUserAuth;
        }

        #endregion
    }
}