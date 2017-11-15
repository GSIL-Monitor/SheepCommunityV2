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
    ///     创建关注服务接口。
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
        ///     获取及设置创建关注的校验器。
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

        #region 创建关注

        /// <summary>
        ///     创建关注。
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
            var followerId = GetSession().UserAuthId.ToInt(0);
            var existingFollow = await FollowRepo.GetFollowAsync(request.FollowingUserId, followerId);
            if (existingFollow != null)
            {
                return new FollowCreateResponse
                       {
                           Follow = existingFollow.MapToFollowDto(await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingFollow.FollowingUserId.ToString()), await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingFollow.FollowerId.ToString()))
                       };
            }
            var newFollow = new Follow
                            {
                                Meta = new Dictionary<string, string>(),
                                FollowingUserId = request.FollowingUserId,
                                FollowerId = followerId
                            };
            var existingReversedFollow = await FollowRepo.GetFollowAsync(followerId, request.FollowingUserId);
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
            var friendAddResponse = await NimClient.PostAsync(new FriendAddRequest
                                                              {
                                                                  AccountId = followerId.ToString(),
                                                                  FriendAccountId = request.FollowingUserId.ToString(),
                                                                  Type = 1
                                                              });
            if (friendAddResponse.Code != 200)
            {
                Log.WarnFormat("NimClient friend add error: {0}, AccountId={1} FriendAccountId={2}", friendAddResponse.Code, followerId, request.FollowingUserId);
            }
            return new FollowCreateResponse
                   {
                       Follow = follow.MapToFollowDto(await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(follow.FollowingUserId.ToString()), await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(follow.FollowerId.ToString()))
                   };
        }

        #endregion
    }
}