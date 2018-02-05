using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Bookmarks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Bookmarks;

namespace Sheep.ServiceInterface.Bookmarks
{
    /// <summary>
    ///     显示一个收藏服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowBookmarkService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowBookmarkService));

        #endregion

        #region 属性

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个收藏的校验器。
        /// </summary>
        public IValidator<BookmarkShow> BookmarkShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置收藏的存储库。
        /// </summary>
        public IBookmarkRepository BookmarkRepo { get; set; }

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

        #region 显示一个收藏

        /// <summary>
        ///     显示一个收藏。
        /// </summary>
        public async Task<object> Get(BookmarkShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    BookmarkShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.UserId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserId));
            }
            var existingBookmark = await BookmarkRepo.GetBookmarkAsync(request.ParentId, request.UserId);
            if (existingBookmark == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BookmarkNotFound, request.ParentId));
            }
            var title = string.Empty;
            switch (existingBookmark.ParentType)
            {
                case "帖子":
                    title = (await PostRepo.GetPostAsync(existingBookmark.ParentId))?.Title;
                    break;
                case "章":
                    title = (await ChapterRepo.GetChapterAsync(existingBookmark.ParentId))?.Title;
                    break;
                case "节":
                    title = (await ParagraphRepo.GetParagraphAsync(existingBookmark.ParentId))?.Content;
                    break;
            }
            var bookmarkDto = existingBookmark.MapToBookmarkDto(user, title);
            return new BookmarkShowResponse
                   {
                       Bookmark = bookmarkDto
                   };
        }

        #endregion
    }
}