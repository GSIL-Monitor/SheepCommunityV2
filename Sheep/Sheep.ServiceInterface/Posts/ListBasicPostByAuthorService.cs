using System.Linq;
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
    ///     根据作者列举一组帖子基本信息服务接口。
    /// </summary>
    public class ListBasicPostByAuthorService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListBasicPostByAuthorService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据作者列举一组帖子基本信息的校验器。
        /// </summary>
        public IValidator<BasicPostListByAuthor> BasicPostListByAuthorValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 根据作者列举一组帖子基本信息

        /// <summary>
        ///     根据作者列举一组帖子基本信息。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(BasicPostListByAuthor request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    BasicPostListByAuthorValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingPosts = await PostRepo.FindPostsByAuthorAsync(request.AuthorId, request.Tag, request.ContentType, request.CreatedSince, request.ModifiedSince, request.PublishedSince, request.IsPublished, request.IsFeatured, "审核通过", request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingPosts == null)
            {
                throw HttpError.NotFound(string.Format(Resources.PostsNotFound));
            }
            var authorsMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingPosts.Select(post => post.AuthorId.ToString()).Distinct().ToList())).ToDictionary(author => author.Id, author => author);
            var postsDto = existingPosts.Select(post => post.MapToBasicPostDto(authorsMap.GetValueOrDefault(post.AuthorId))).ToList();
            return new BasicPostListResponse
                   {
                       Posts = postsDto
                   };
        }

        #endregion
    }
}