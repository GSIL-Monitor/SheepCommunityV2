using System.Collections.Generic;
using System.Threading.Tasks;
using Netease.Nim;
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
    ///     更改显示名称服务接口。
    /// </summary>
    public class ChangeAccountDisplayNameService : ChangeAccountService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeAccountDisplayNameService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置更改显示名称的校验器。
        /// </summary>
        public IValidator<AccountChangeDisplayName> AccountChangeDisplayNameValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        #endregion

        #region 更改显示名称

        /// <summary>
        ///     更改显示名称。
        /// </summary>
        public async Task<object> Put(AccountChangeDisplayName request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountChangeDisplayNameValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var session = GetSession();
            var existingUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(session, null);
            if (existingUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, session.UserAuthId));
            }
            var newUserAuth = AuthRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
            newUserAuth.PopulateMissingExtended(existingUserAuth);
            newUserAuth.Meta = existingUserAuth.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingUserAuth.Meta);
            newUserAuth.DisplayName = request.DisplayName;
            var userAuth = await ((IUserAuthRepositoryExtended) AuthRepo).UpdateUserAuthAsync(existingUserAuth, newUserAuth);
            ResetCache(userAuth);
            await NimClient.PostAsync(new UserUpdateInfoRequest
                                      {
                                          AccountId = userAuth.Id.ToString(),
                                          Name = userAuth.DisplayName,
                                          IconUrl = userAuth.Meta.GetValueOrDefault("AvatarUrl"),
                                          Signature = userAuth.Meta.GetValueOrDefault("Signature"),
                                          Email = userAuth.Email,
                                          BirthDate = userAuth.BirthDate?.ToString("yyyy-MM-dd"),
                                          Mobile = userAuth.PhoneNumber,
                                          Gender = userAuth.Gender.IsNullOrEmpty() ? 0 : (userAuth.Gender == "男" ? 1 : 2)
                                      });
            return new AccountChangeDisplayNameResponse();
        }

        #endregion
    }
}