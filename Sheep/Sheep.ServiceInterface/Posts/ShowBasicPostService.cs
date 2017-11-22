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
    ///     显示一个帖子基本信息服务接口。
    /// </summary>
    public class ShowBasicPostService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowBasicPostService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个帖子基本信息的校验器。
        /// </summary>
        public IValidator<BasicPostShow> BasicPostShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 显示一个帖子基本信息

        /// <summary>
        ///     显示一个帖子基本信息。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(BasicPostShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BasicPostShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
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
            var postDto = existingPost.MapToBasicPostDto(author);
            return new BasicPostShowResponse
                   {
                       Post = postDto
                   };
        }

        #endregion
    }
}