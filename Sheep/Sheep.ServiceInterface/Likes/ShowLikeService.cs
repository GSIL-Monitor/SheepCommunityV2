using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Likes.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Likes;

namespace Sheep.ServiceInterface.Likes
{
    /// <summary>
    ///     显示一个点赞服务接口。
    /// </summary>
    public class ShowLikeService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowLikeService));

        #endregion

        #region 属性

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个点赞的校验器。
        /// </summary>
        public IValidator<LikeShow> LikeShowValidator { get; set; }

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

        #region 显示一个点赞

        /// <summary>
        ///     显示一个点赞。
        /// </summary>
        public async Task<object> Get(LikeShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                LikeShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.UserId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserId));
            }
            var title = string.Empty;
            var existingLike = await LikeRepo.GetLikeAsync(request.ParentId, request.UserId);
            if (existingLike == null)
            {
                throw HttpError.NotFound(string.Format(Resources.LikeNotFound, request.ParentId));
            }
            switch (existingLike.ParentType)
            {
                case "帖子":
                    title = (await PostRepo.GetPostAsync(existingLike.ParentId))?.Title;
                    break;
                case "章":
                    title = (await ChapterRepo.GetChapterAsync(existingLike.ParentId))?.Title;
                    break;
                case "节":
                    title = (await ParagraphRepo.GetParagraphAsync(existingLike.ParentId))?.Content;
                    break;
            }
            var likeDto = existingLike.MapToLikeDto(user, title);
            return new LikeShowResponse
                   {
                       Like = likeDto
                   };
        }

        #endregion
    }
}