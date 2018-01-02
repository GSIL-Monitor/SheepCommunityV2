using System.Collections.Generic;
using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Friendship;
using Sheep.Model.Friendship.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Follows;

namespace Sheep.ServiceInterface.Follows
{
    /// <summary>
    ///     取消一个关注服务接口。
    /// </summary>
    public class DeleteFollowService : ChangeFollowService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteFollowService));

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
        ///     获取及设置取消一个关注的校验器。
        /// </summary>
        public IValidator<FollowDelete> FollowDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置关注的存储库。
        /// </summary>
        public IFollowRepository FollowRepo { get; set; }

        #endregion

        #region 取消一个关注

        /// <summary>
        ///     取消一个关注。
        /// </summary>
        public async Task<object> Delete(FollowDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    FollowDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var followerId = GetSession().UserAuthId.ToInt(0);
            var existingFollow = await FollowRepo.GetFollowAsync(request.OwnerId, followerId);
            if (existingFollow == null)
            {
                throw HttpError.NotFound(string.Format(Resources.FollowNotFound, request.OwnerId));
            }
            await FollowRepo.DeleteFollowAsync(request.OwnerId, followerId);
            ResetCache(existingFollow);
            await NimClient.PostAsync(new FriendDeleteRequest
                                      {
                                          AccountId = followerId.ToString(),
                                          FriendAccountId = request.OwnerId.ToString()
                                      });
            var existingReversedFollow = await FollowRepo.GetFollowAsync(followerId, request.OwnerId);
            if (existingReversedFollow != null)
            {
                var newReversedFollow = new Follow();
                newReversedFollow.PopulateWith(existingReversedFollow);
                newReversedFollow.Meta = existingReversedFollow.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingReversedFollow.Meta);
                newReversedFollow.IsBidirectional = false;
                await FollowRepo.UpdateFollowAsync(existingReversedFollow, newReversedFollow);
            }
            return new FollowDeleteResponse();
        }

        #endregion
    }
}