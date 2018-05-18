using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.Model.Friendship;
using Sheep.ServiceInterface.Posts.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Posts;

namespace Sheep.ServiceInterface.Posts
{
    /// <summary>
    ///     列举一组帖子服务接口。
    /// </summary>
    [CompressResponse]
    public class ListPostService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListPostService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组帖子的校验器。
        /// </summary>
        public IValidator<PostList> PostListValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

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

        /// <summary>
        ///     获取及设置屏蔽的存储库。
        /// </summary>
        public IBlockRepository BlockRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子屏蔽的存储库。
        /// </summary>
        public IPostBlockRepository PostBlockRepo { get; set; }

        #endregion

        #region 列举一组帖子

        /// <summary>
        ///     列举一组帖子。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(PostList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    PostListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var existingBlocks = await BlockRepo.FindBlocksByBlockerAsync(currentUserId, null, null, null, null, null, null);
            var blockedUserIds = existingBlocks.Select(block => block.BlockeeId).Distinct().ToList();
            var existingPostBlocks = await PostBlockRepo.FindPostBlocksByBlockerAsync(currentUserId, null, null, null, null, null, null);
            var blockedPostIds = existingPostBlocks.Select(postBlock => postBlock.PostId).Distinct().ToList();
            var existingPosts = await PostRepo.FindPostsAsync(request.TitleFilter, request.Tag, request.ContentType, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.PublishedSince?.FromUnixTime(),
                                                              request.IsPublished ?? true, request.IsFeatured, "审核通过", blockedUserIds, blockedPostIds, request.OrderBy, request.Descending, request.Skip, request.Limit);
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