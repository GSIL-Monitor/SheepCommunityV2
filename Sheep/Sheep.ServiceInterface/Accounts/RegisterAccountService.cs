using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Web;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Accounts;
using CredentialsAuthProvider = Sheep.Model.Auth.Providers.CredentialsAuthProvider;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     注册服务接口。
    /// </summary>
    public class RegisterAccountService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RegisterAccountService));

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
        ///     获取及设置注册帐户的校验器。
        /// </summary>
        public IValidator<AccountRegister> AccountRegisterValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        #endregion

        #region 注册

        /// <summary>
        ///     注册。
        /// </summary>
        public object Post(AccountRegister request)
        {
            if (IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.ReRegisterNotAllowed);
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
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    AccountRegisterValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var newUserAuth = AuthRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
            newUserAuth.Meta = new Dictionary<string, string>();
            newUserAuth.UserName = request.UserName;
            newUserAuth.Email = request.Email;
            newUserAuth.PrimaryEmail = request.Email;
            var userAuth = AuthRepo.CreateUserAuth(newUserAuth, request.Password);
            AccountRegisterResponse response = null;
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
                        response = new AccountRegisterResponse
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
                   new AccountRegisterResponse
                   {
                       UserId = userAuth.Id
                   };
        }

        #endregion
    }
}