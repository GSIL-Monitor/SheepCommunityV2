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
    ///     显示一个关注服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowFollowService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowFollowService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个关注的校验器。
        /// </summary>
        public IValidator<FollowShow> FollowShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置关注的存储库。
        /// </summary>
        public IFollowRepository FollowRepo { get; set; }

        #endregion

        #region 显示一个关注

        /// <summary>
        ///     显示一个关注。
        /// </summary>
        [CacheResponse(Duration = 3600)]
        public async Task<object> Get(FollowShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    FollowShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var owner = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.OwnerId.ToString());
            if (owner == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.OwnerId));
            }
            var follower = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.FollowerId.ToString());
            if (follower == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.FollowerId));
            }
            var existingFollow = await FollowRepo.GetFollowAsync(request.OwnerId, request.FollowerId);
            if (existingFollow == null)
            {
                throw HttpError.NotFound(string.Format(Resources.FollowNotFound, request.OwnerId));
            }
            var followDto = existingFollow.MapToFollowDto(owner, follower);
            return new FollowShowResponse
                   {
                       Follow = followDto
                   };
        }

        #endregion
    }
}