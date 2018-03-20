using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Membership;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Users;

namespace Sheep.ServiceInterface.Users
{
    /// <summary>
    ///     显示一个用户排行服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowUserRankService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowUserRankService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个用户排行的校验器。
        /// </summary>
        public IValidator<UserRankShow> UserRankShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置用户排行的存储库。
        /// </summary>
        public IUserRankRepository UserRankRepo { get; set; }

        #endregion

        #region 显示一个用户排行

        /// <summary>
        ///     显示一个用户排行。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(UserRankShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    UserRankShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingUserRank = await UserRankRepo.GetUserRankAsync(request.UserId);
            if (existingUserRank == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserRankNotFound, request.UserId));
            }
            var existingUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.UserId.ToString());
            if (existingUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserId));
            }
            var userRankDto = existingUserRank.MapToUserRankDto(existingUserAuth);
            return new UserRankShowResponse
                   {
                       UserRank = userRankDto
                   };
        }

        #endregion
    }
}