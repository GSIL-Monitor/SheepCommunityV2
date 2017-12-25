using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Friendship;
using Sheep.ServiceInterface.Follows.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Follows;

namespace Sheep.ServiceInterface.Follows
{
    /// <summary>
    ///     列举一组关注者的关注信息服务接口。
    /// </summary>
    public class ListFollowOfFollowerService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListFollowOfFollowerService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组关注者的校验器。
        /// </summary>
        public IValidator<FollowListOfFollower> FollowListOfFollowerValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置关注的存储库。
        /// </summary>
        public IFollowRepository FollowRepo { get; set; }

        #endregion

        #region 列举一组关注者

        /// <summary>
        ///     列举一组关注者。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(FollowListOfFollower request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                FollowListOfFollowerValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingFollows = await FollowRepo.FindFollowsByOwnerAsync(request.OwnerId, request.CreatedSince, request.ModifiedSince, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingFollows == null)
            {
                throw HttpError.NotFound(string.Format(Resources.FollowsNotFound));
            }
            var followersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingFollows.Select(follow => follow.FollowerId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var followsDto = existingFollows.Select(follow => follow.MapToFollowOfFollowerDto(followersMap.GetValueOrDefault(follow.FollowerId))).ToList();
            return new FollowListOfFollowerResponse
                   {
                       Follows = followsDto
                   };
        }

        #endregion
    }
}