using System.Linq;
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
    ///     列举一组用户排行服务接口。
    /// </summary>
    [CompressResponse]
    public class ListUserRankService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListUserRankService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组用户排行的校验器。
        /// </summary>
        public IValidator<UserRankList> UserRankListValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置用户排行的存储库。
        /// </summary>
        public IUserRankRepository UserRankRepo { get; set; }

        #endregion

        #region 列举一组用户排行

        /// <summary>
        ///     列举一组用户排行。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(UserRankList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    UserRankListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingUserRanks = await UserRankRepo.FindUserRanksAsync(null, null, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingUserRanks == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserRanksNotFound));
            }
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).FindUserAuthsAsync(existingUserRanks.Select(userRank => userRank.Id.ToString()).ToList(), null, null, null, null, null, null, null, null)).ToDictionary(user => user.Id, user => user);
            var userRanksDto = existingUserRanks.Select(userRank => userRank.MapToUserRankDto(usersMap.GetValueOrDefault(userRank.Id))).ToList();
            return new UserRankListResponse
                   {
                       UserRanks = userRanksDto
                   };
        }

        #endregion
    }
}