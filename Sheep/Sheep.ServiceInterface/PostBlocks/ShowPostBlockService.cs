using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.PostBlocks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.PostBlocks;

namespace Sheep.ServiceInterface.PostBlocks
{
    /// <summary>
    ///     显示一个帖子屏蔽服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowPostBlockService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowPostBlockService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个帖子屏蔽的校验器。
        /// </summary>
        public IValidator<PostBlockShow> PostBlockShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子屏蔽的存储库。
        /// </summary>
        public IPostBlockRepository PostBlockRepo { get; set; }

        #endregion

        #region 显示一个帖子屏蔽

        /// <summary>
        ///     显示一个帖子屏蔽。
        /// </summary>
        public async Task<object> Get(PostBlockShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    PostBlockShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var blockerId = GetSession().UserAuthId.ToInt(0);
            var blocker = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(blockerId.ToString());
            if (blocker == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, blockerId));
            }
            var existingPostBlock = await PostBlockRepo.GetPostBlockAsync(request.PostId, blockerId);
            if (existingPostBlock == null)
            {
                throw HttpError.NotFound(string.Format(Resources.PostBlockNotFound, request.PostId));
            }
            var post = await PostRepo.GetPostAsync(existingPostBlock.PostId);
            var postAuthor = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(post.AuthorId.ToString());
            var postBlockDto = existingPostBlock.MapToPostBlockDto(post, postAuthor, blocker);
            return new PostBlockShowResponse
                   {
                       PostBlock = postBlockDto
                   };
        }

        #endregion
    }
}