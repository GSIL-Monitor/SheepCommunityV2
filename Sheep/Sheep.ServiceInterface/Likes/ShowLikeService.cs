using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
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
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

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
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    LikeShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.UserId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserId));
            }
            var existingLike = await LikeRepo.GetLikeAsync(request.ParentId, request.UserId);
            if (existingLike == null)
            {
                throw HttpError.NotFound(string.Format(Resources.LikeNotFound, request.ParentId));
            }
            var title = string.Empty;
            var pictureUrl = string.Empty;
            switch (existingLike.ParentType)
            {
                case "帖子":
                    var post = await PostRepo.GetPostAsync(existingLike.ParentId);
                    if (post != null)
                    {
                        title = post.Title;
                        pictureUrl = post.PictureUrl;
                    }
                    break;
                case "章":
                    var chapter = await ChapterRepo.GetChapterAsync(existingLike.ParentId);
                    if (chapter != null)
                    {
                        title = chapter.Title;
                    }
                    break;
                case "节":
                    var paragraph = await ParagraphRepo.GetParagraphAsync(existingLike.ParentId);
                    if (paragraph != null)
                    {
                        title = paragraph.Content;
                    }
                    break;
            }
            var likeDto = existingLike.MapToLikeDto(user, title, pictureUrl);
            return new LikeShowResponse
                   {
                       Like = likeDto
                   };
        }

        #endregion
    }
}