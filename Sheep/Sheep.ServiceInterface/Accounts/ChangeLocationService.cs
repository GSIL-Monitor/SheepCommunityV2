using System;
using System.Collections.Generic;
using System.Linq;
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
    ///     更改所在地服务接口。
    /// </summary>
    public class ChangeLocationService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeLocationService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置更改所在地的校验器。
        /// </summary>
        public IValidator<AccountChangeLocation> AccountChangeLocationValidator { get; set; }

        #endregion

        #region 更改所在地

        /// <summary>
        ///     更改所在地。
        /// </summary>
        public async Task<object> Put(AccountChangeLocation request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountChangeLocationValidator.ValidateAndThrow(request, ApplyTo.Put);
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
                var newUserAuth = authRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
                newUserAuth.PopulateMissingExtended(existingUserAuth);
                newUserAuth.Meta = existingUserAuth.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingUserAuth.Meta);
                newUserAuth.Country = request.Country;
                newUserAuth.State = request.State;
                newUserAuth.City = request.City;
                var user = await ((IUserAuthRepositoryExtended) authRepo).UpdateUserAuthAsync(existingUserAuth, newUserAuth);
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/{0}", user.Id)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/{0}", user.Id)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/basic/{0}", user.Id)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/basic/{0}", user.Id)).ToArray());
                if (!user.UserName.IsNullOrEmpty())
                {
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/show/{0}", user.UserName)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/show/{0}", user.UserName)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/basic/show/{0}", user.UserName)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/basic/show/{0}", user.UserName)).ToArray());
                }
                if (!user.Email.IsNullOrEmpty())
                {
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/show/{0}", user.Email)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/show/{0}", user.Email)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/basic/show/{0}", user.Email)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/basic/show/{0}", user.Email)).ToArray());
                }
                return new AccountChangeLocationResponse();
            }
        }

        #endregion
    }
}