using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.PostBlocks;

namespace Sheep.ServiceInterface.PostBlocks
{
    /// <summary>
    ///     删除一个帖子屏蔽服务接口。
    /// </summary>
    public class DeletePostBlockService : ChangePostBlockService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeletePostBlockService));

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
        ///     获取及设置删除一个帖子屏蔽的校验器。
        /// </summary>
        public IValidator<PostBlockDelete> PostBlockDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子屏蔽的存储库。
        /// </summary>
        public IPostBlockRepository PostBlockRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 删除一个帖子屏蔽

        /// <summary>
        ///     删除一个帖子屏蔽。
        /// </summary>
        public async Task<object> Delete(PostBlockDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    PostBlockDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
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
            //var blockerId = GetSession().UserAuthId.ToInt(0);
            //if (existingPostBlock.BlockerId != blockerId)
            //{
            //    throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            //}
            await PostBlockRepo.DeletePostBlockAsync(existingPostBlock.Id);
            ResetCache(existingPostBlock);
            return new PostBlockDeleteResponse();
        }

        #endregion
    }
}