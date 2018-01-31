using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.Model.Friendship;
using Sheep.ServiceInterface.Likes.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Likes;

namespace Sheep.ServiceInterface.Likes
{
    /// <summary>
    ///     根据作者帖子列表列举一组点赞信息服务接口。
    /// </summary>
    public class ListLikeByPostsOfAuthorService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListLikeByPostsOfAuthorService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组点赞的校验器。
        /// </summary>
        public IValidator<LikeListByPostsOfAuthor> LikeListByPostsOfAuthorValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置关注的存储库。
        /// </summary>
        public IFollowRepository FollowRepo { get; set; }

        /// <summary>
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 列举一组点赞

        /// <summary>
        ///     列举一组点赞。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(LikeListByPostsOfAuthor request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    LikeListByPostsOfAuthorValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            List<Like> existingLikes;
            if (request.Following.HasValue && request.Following.Value)
            {
                var existingFollows = await FollowRepo.FindFollowsByFollowerAsync(currentUserId, null, null, null, null, null, null);
                if (existingFollows == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.FollowsNotFound));
                }
                existingLikes = await LikeRepo.FindLikesByPostsOfAuthorAsync(currentUserId, existingFollows.Select(follow => follow.OwnerId).Distinct().ToList(), request.Skip, request.Limit);
            }
            else
            {
                existingLikes = await LikeRepo.FindLikesByPostsOfAuthorAsync(currentUserId, null, request.Skip, request.Limit);
            }
            if (existingLikes == null)
            {
                throw HttpError.NotFound(string.Format(Resources.LikesNotFound));
            }
            var postsMap = (await PostRepo.GetPostsAsync(existingLikes.Where(like => like.ParentType == "帖子").Select(like => like.ParentId).Distinct().ToList())).ToDictionary(post => post.Id, post => post);
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingLikes.Select(like => like.UserId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var likesDto = existingLikes.Select(like => like.MapToLikeDto(like.ParentType == "帖子" ? postsMap.GetValueOrDefault(like.ParentId)?.Title : null, like.ParentType == "帖子" ? postsMap.GetValueOrDefault(like.ParentId)?.PictureUrl : null, like.ParentType == "帖子" ? postsMap.GetValueOrDefault(like.ParentId)?.ContentType : null, usersMap.GetValueOrDefault(like.UserId))).ToList();
            return new LikeListResponse
                   {
                       Likes = likesDto
                   };
        }

        #endregion
    }
}