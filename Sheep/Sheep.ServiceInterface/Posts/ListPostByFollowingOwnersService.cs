using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.Model.Friendship;
using Sheep.ServiceInterface.Posts.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Posts;

namespace Sheep.ServiceInterface.Posts
{
    /// <summary>
    ///     根据已关注的作者列表列举一组帖子服务接口。
    /// </summary>
    public class ListPostByFollowingOwnersService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListPostByFollowingOwnersService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据已关注的作者列表列举一组帖子的校验器。
        /// </summary>
        public IValidator<PostListByFollowingOwners> PostListByFollowingOwnersValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置关注的存储库。
        /// </summary>
        public IFollowRepository FollowRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        /// <summary>
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

        #endregion

        #region 根据已关注的作者列表列举一组帖子

        /// <summary>
        ///     根据已关注的作者列表列举一组帖子。
        /// </summary>
        //[CacheResponse(Duration = 600)]
        public async Task<object> Get(PostListByFollowingOwners request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                PostListByFollowingOwnersValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var existingFollows = await FollowRepo.FindFollowsByFollowerAsync(currentUserId, null, null, null, null, null, null);
            if (existingFollows == null)
            {
                throw HttpError.NotFound(string.Format(Resources.FollowsNotFound));
            }
            var existingPosts = await PostRepo.FindPostsByAuthorsAsync(existingFollows.Select(follow => follow.OwnerId).Distinct().ToList(), request.Tag, request.ContentType, request.CreatedSince, request.ModifiedSince, request.PublishedSince, request.IsPublished ?? true, request.IsFeatured, "审核通过", request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingPosts == null)
            {
                throw HttpError.NotFound(string.Format(Resources.PostsNotFound));
            }
            var authorsMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingPosts.Select(post => post.AuthorId.ToString()).Distinct().ToList())).ToDictionary(author => author.Id, author => author);
            var commentsMap = (await CommentRepo.GetCommentsCountByParentsAsync(existingPosts.Select(post => post.Id).ToList(), currentUserId, null, null, null, "审核通过")).ToDictionary(pair => pair.Key, pair => pair.Value);
            var postsDto = existingPosts.Select(post => post.MapToPostDto(authorsMap.GetValueOrDefault(post.AuthorId), commentsMap.GetValueOrDefault(post.Id) > 0)).ToList();
            return new PostListResponse
                   {
                       Posts = postsDto
                   };
        }

        #endregion
    }
}