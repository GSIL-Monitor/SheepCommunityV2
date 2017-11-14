using System;
using System.Threading.Tasks;
using ServiceStack;
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
    ///     显示一个用户基本信息服务接口。
    /// </summary>
    public class ShowBasicUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowBasicUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个用户基本信息的校验器。
        /// </summary>
        public IValidator<BasicUserShow> BasicUserShowValidator { get; set; }

        #endregion

        #region 显示一个用户基本信息

        /// <summary>
        ///     显示一个用户基本信息。
        /// </summary>
        [CacheResponse(Duration = 600, MaxAge = 300)]
        public async Task<object> Get(BasicUserShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BasicUserShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var existingUserAuth = await ((IUserAuthRepositoryExtended) authRepo).GetUserAuthAsync(request.UserId.ToString());
                if (existingUserAuth == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserId));
                }
                var userDto = existingUserAuth.MapToBasicUserDto();
                return new BasicUserShowResponse
                       {
                           User = userDto
                       };
            }
        }

        #endregion
    }
}