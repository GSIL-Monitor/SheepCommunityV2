using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Friendship;
using Sheep.ServiceInterface.Follows.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Follows;

namespace Sheep.ServiceInterface.Follows
{
    /// <summary>
    ///     列举一组被关注者的关注信息服务接口。
    /// </summary>
    public class ListFollowOfOwnerService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListFollowOfOwnerService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组被关注者的校验器。
        /// </summary>
        public IValidator<FollowListOfOwner> FollowListOfOwnerValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置关注的存储库。
        /// </summary>
        public IFollowRepository FollowRepo { get; set; }

        #endregion

        #region 列举一组被关注者

        /// <summary>
        ///     列举一组被关注者。
        /// </summary>
        [CacheResponse(Duration = 3600)]
        public async Task<object> Get(FollowListOfOwner request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    FollowListOfOwnerValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingFollows = await FollowRepo.FindFollowsByFollowerAsync(request.FollowerId, request.CreatedSince, request.ModifiedSince, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingFollows == null)
            {
                throw HttpError.NotFound(string.Format(Resources.FollowsNotFound));
            }
            var ownersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingFollows.Select(follow => follow.OwnerId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var followsDto = existingFollows.Select(follow => follow.MapToFollowOfOwnerDto(ownersMap.GetValueOrDefault(follow.OwnerId))).ToList();
            return new FollowListOfOwnerResponse
                   {
                       Follows = followsDto
                   };
        }

        #endregion
    }
}