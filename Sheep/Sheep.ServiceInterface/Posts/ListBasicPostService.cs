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
using Sheep.ServiceInterface.Posts.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Posts;

namespace Sheep.ServiceInterface.Posts
{
    /// <summary>
    ///     列举一组帖子基本信息服务接口。
    /// </summary>
    public class ListBasicPostService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListBasicPostService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组帖子基本信息的校验器。
        /// </summary>
        public IValidator<BasicPostList> BasicPostListValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 列举一组帖子基本信息

        /// <summary>
        ///     列举一组帖子基本信息。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(BasicPostList request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BasicPostListValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingPosts = await PostRepo.FindPostsAsync(request.TitleFilter, request.Tag, request.ContentType, request.CreatedSince, request.ModifiedSince, request.PublishedSince, request.IsPublished, request.IsFeatured, "审核通过", request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingPosts == null)
            {
                throw HttpError.NotFound(string.Format(Resources.PostsNotFound));
            }
            var authorsMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingPosts.Select(post => post.AuthorId.ToString()).Distinct())).ToDictionary(author => author.Id, author => author);
            var postsDto = existingPosts.Select(post => post.MapToBasicPostDto(authorsMap.GetValueOrDefault(post.AuthorId))).ToList();
            return new BasicPostListResponse
                   {
                       Posts = postsDto
                   };
        }

        #endregion
    }
}