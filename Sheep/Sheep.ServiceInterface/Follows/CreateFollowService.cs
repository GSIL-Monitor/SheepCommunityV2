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
using Sheep.Model.Friendship;
using Sheep.Model.Friendship.Entities;
using Sheep.ServiceInterface.Follows.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Follows;

namespace Sheep.ServiceInterface.Follows
{
    /// <summary>
    ///     新建一个关注服务接口。
    /// </summary>
    public class CreateFollowService : ChangeFollowService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateFollowService));

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
        ///     获取及设置新建一个关注的校验器。
        /// </summary>
        public IValidator<FollowCreate> FollowCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置关注的存储库。
        /// </summary>
        public IFollowRepository FollowRepo { get; set; }

        #endregion

        #region 新建一个关注

        /// <summary>
        ///     新建一个关注。
        /// </summary>
        public async Task<object> Post(FollowCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                FollowCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var owner = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.OwnerId.ToString());
            if (owner == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.OwnerId));
            }
            var followerId = GetSession().UserAuthId.ToInt(0);
            var follower = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(followerId.ToString());
            if (follower == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, followerId));
            }
            var existingFollow = await FollowRepo.GetFollowAsync(request.OwnerId, followerId);
            if (existingFollow != null)
            {
                return new FollowCreateResponse
                       {
                           Follow = existingFollow.MapToFollowDto(owner, follower)
                       };
            }
            var newFollow = new Follow
                            {
                                Meta = new Dictionary<string, string>(),
                                OwnerId = request.OwnerId,
                                FollowerId = followerId
                            };
            var existingReversedFollow = await FollowRepo.GetFollowAsync(followerId, request.OwnerId);
            if (existingReversedFollow != null)
            {
                newFollow.IsBidirectional = true;
                var newReversedFollow = new Follow();
                newReversedFollow.PopulateWith(existingReversedFollow);
                newReversedFollow.Meta = existingReversedFollow.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingReversedFollow.Meta);
                newReversedFollow.IsBidirectional = true;
                await FollowRepo.UpdateFollowAsync(existingReversedFollow, newReversedFollow);
            }
            var follow = await FollowRepo.CreateFollowAsync(newFollow);
            ResetCache(follow);
            await NimClient.PostAsync(new FriendAddRequest
                                      {
                                          AccountId = followerId.ToString(),
                                          FriendAccountId = request.OwnerId.ToString(),
                                          Type = 1
                                      });
            return new FollowCreateResponse
                   {
                       Follow = follow.MapToFollowDto(owner, follower)
                   };
        }

        #endregion
    }
}