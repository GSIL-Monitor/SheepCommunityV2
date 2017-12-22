using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Bookmarks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Bookmarks;

namespace Sheep.ServiceInterface.Bookmarks
{
    /// <summary>
    ///     根据用户列举一组收藏信息服务接口。
    /// </summary>
    public class ListBookmarkByUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListBookmarkByUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组收藏的校验器。
        /// </summary>
        public IValidator<BookmarkListByUser> BookmarkListByUserValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置收藏的存储库。
        /// </summary>
        public IBookmarkRepository BookmarkRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        #endregion

        #region 列举一组收藏

        /// <summary>
        ///     列举一组收藏。
        /// </summary>
        //[CacheResponse(Duration = 600)]
        public async Task<object> Get(BookmarkListByUser request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BookmarkListByUserValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingBookmarks = await BookmarkRepo.FindBookmarksByUserAsync(request.UserId, request.ParentType, request.CreatedSince, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingBookmarks == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BookmarksNotFound));
            }
            var postTitlesMap = (await PostRepo.GetPostsAsync(existingBookmarks.Where(bookmark => bookmark.ParentType == "帖子").Select(bookmark => bookmark.ParentId).Distinct())).ToDictionary(post => post.Id, post => post.Title);
            var chapterTitlesMap = (await ChapterRepo.GetChaptersAsync(existingBookmarks.Where(bookmark => bookmark.ParentType == "章").Select(bookmark => bookmark.ParentId).Distinct())).ToDictionary(chapter => chapter.Id, chapter => chapter.Title);
            var paragraphTitlesMap = (await ParagraphRepo.GetParagraphsAsync(existingBookmarks.Where(bookmark => bookmark.ParentType == "节").Select(bookmark => bookmark.ParentId).Distinct())).ToDictionary(paragraph => paragraph.Id, paragraph => paragraph.Content);
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingBookmarks.Select(bookmark => bookmark.UserId.ToString()).Distinct())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var bookmarksDto = existingBookmarks.Select(bookmark => bookmark.MapToBookmarkDto(usersMap.GetValueOrDefault(bookmark.UserId), bookmark.ParentType == "帖子" ? postTitlesMap.GetValueOrDefault(bookmark.ParentId) : (bookmark.ParentType == "章" ? chapterTitlesMap.GetValueOrDefault(bookmark.ParentId) : paragraphTitlesMap.GetValueOrDefault(bookmark.ParentId)))).ToList();
            return new BookmarkListResponse
                   {
                       Bookmarks = bookmarksDto
                   };
        }

        #endregion
    }
}