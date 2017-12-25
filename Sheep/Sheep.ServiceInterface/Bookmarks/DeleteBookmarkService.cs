using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Bookmarks;

namespace Sheep.ServiceInterface.Bookmarks
{
    /// <summary>
    ///     取消一个收藏服务接口。
    /// </summary>
    public class DeleteBookmarkService : ChangeBookmarkService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteBookmarkService));

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
        ///     获取及设置取消一个收藏的校验器。
        /// </summary>
        public IValidator<BookmarkDelete> BookmarkDeleteValidator { get; set; }

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

        #region 取消一个收藏

        /// <summary>
        ///     取消一个收藏。
        /// </summary>
        public async Task<object> Delete(BookmarkDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BookmarkDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var existingBookmark = await BookmarkRepo.GetBookmarkAsync(request.ParentId, currentUserId);
            if (existingBookmark == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BookmarkNotFound, request.ParentId));
            }
            if (existingBookmark.UserId != currentUserId)
            {
                throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            }
            await BookmarkRepo.DeleteBookmarkAsync(request.ParentId, currentUserId);
            ResetCache(existingBookmark);
            switch (existingBookmark.ParentType)
            {
                case "帖子":
                    await PostRepo.IncrementPostBookmarksCountAsync(existingBookmark.ParentId, -1);
                    break;
                case "章":
                    await ChapterRepo.IncrementChapterBookmarksCountAsync(existingBookmark.ParentId, -1);
                    break;
                case "节":
                    await ParagraphRepo.IncrementParagraphBookmarksCountAsync(existingBookmark.ParentId, -1);
                    break;
            }
            return new BookmarkDeleteResponse();
        }

        #endregion
    }
}