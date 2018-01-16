using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Users;

namespace Sheep.ServiceInterface.Users
{
    /// <summary>
    ///     列举一组用户服务接口。
    /// </summary>
    public class ListUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组用户的校验器。
        /// </summary>
        public IValidator<UserList> UserListValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        #endregion

        #region 列举一组用户

        /// <summary>
        ///     列举一组用户。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(UserList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    UserListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingUserAuths = await ((IUserAuthRepositoryExtended) AuthRepo).FindUserAuthsAsync(request.UserNameFilter, request.NameFilter, request.CreatedSince, request.ModifiedSince, request.LockedSince, null, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingUserAuths == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UsersNotFound));
            }
            var usersDto = existingUserAuths.Select(userAuth => userAuth.MapToUserDto()).ToList();
            return new UserListResponse
                   {
                       Users = usersDto
                   };
        }

        #endregion
    }
}