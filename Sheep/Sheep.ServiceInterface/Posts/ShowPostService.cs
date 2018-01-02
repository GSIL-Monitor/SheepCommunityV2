using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Posts.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Posts;

namespace Sheep.ServiceInterface.Posts
{
    /// <summary>
    ///     显示一个帖子服务接口。
    /// </summary>
    public class ShowPostService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowPostService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个帖子的校验器。
        /// </summary>
        public IValidator<PostShow> PostShowValidator { get; set; }

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

        #endregion

        #region 显示一个帖子

        /// <summary>
        ///     显示一个帖子。
        /// </summary>
        //[CacheResponse(Duration = 600)]
        public async Task<object> Get(PostShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    PostShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingPost = await PostRepo.GetPostAsync(request.PostId);
            if (existingPost == null)
            {
                throw HttpError.NotFound(string.Format(Resources.PostNotFound, request.PostId));
            }
            var author = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingPost.AuthorId.ToString());
            if (author == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, existingPost.AuthorId));
            }
            await PostRepo.IncrementPostViewsCountAsync(existingPost.Id, 1);
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var commentsCount = await CommentRepo.GetCommentsCountByParentAsync(existingPost.Id, currentUserId, null, null, null, "审核通过");
            var postDto = existingPost.MapToPostDto(author, commentsCount > 0);
            return new PostShowResponse
                   {
                       Post = postDto
                   };
        }

        #endregion
    }
}