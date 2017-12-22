using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Content;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Likes;

namespace Sheep.ServiceInterface.Likes
{
    /// <summary>
    ///     取消一个点赞服务接口。
    /// </summary>
    public class DeleteLikeService : ChangeLikeService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteLikeService));

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
        ///     获取及设置取消一个点赞的校验器。
        /// </summary>
        public IValidator<LikeDelete> LikeDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        #endregion

        #region 取消一个点赞

        /// <summary>
        ///     取消一个点赞。
        /// </summary>
        public async Task<object> Delete(LikeDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                LikeDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            }
            var userId = GetSession().UserAuthId.ToInt(0);
            var existingLike = await LikeRepo.GetLikeAsync(request.ParentId, userId);
            if (existingLike == null)
            {
                throw HttpError.NotFound(string.Format(Resources.LikeNotFound, request.ParentId));
            }
            if (existingLike.UserId != userId)
            {
                throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            }
            await LikeRepo.DeleteLikeAsync(request.ParentId, userId);
            ResetCache(existingLike);
            switch (existingLike.ParentType)
            {
                case "帖子":
                    await PostRepo.IncrementPostLikesCountAsync(existingLike.ParentId, -1);
                    break;
                case "章":
                    await ChapterRepo.IncrementChapterLikesCountAsync(existingLike.ParentId, -1);
                    break;
                case "节":
                    await ParagraphRepo.IncrementParagraphLikesCountAsync(existingLike.ParentId, -1);
                    break;
            }
            return new LikeDeleteResponse();
        }

        #endregion
    }
}