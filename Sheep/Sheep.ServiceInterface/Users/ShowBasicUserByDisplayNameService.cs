using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Users;

namespace Sheep.ServiceInterface.Users
{
    /// <summary>
    ///     根据显示名称显示一个用户基本信息服务接口。
    /// </summary>
    public class ShowBasicUserByDisplayNameService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowBasicUserByDisplayNameService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据显示名称显示一个用户基本信息的校验器。
        /// </summary>
        public IValidator<BasicUserShowByDisplayName> BasicUserShowByDisplayNameValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        #endregion

        #region 根据显示名称显示一个用户基本信息

        /// <summary>
        ///     根据显示名称显示一个用户基本信息。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(BasicUserShowByDisplayName request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BasicUserShowByDisplayNameValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthByDisplayNameAsync(request.DisplayName);
            if (existingUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.DisplayName));
            }
            var userDto = existingUserAuth.MapToBasicUserDto();
            return new BasicUserShowResponse
                   {
                       User = userDto
                   };
        }

        #endregion
    }
}