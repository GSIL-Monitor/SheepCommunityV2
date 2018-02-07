using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Bookmarks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Bookmarks;

namespace Sheep.ServiceInterface.Bookmarks
{
    /// <summary>
    ///     新建一个收藏服务接口。
    /// </summary>
    public class CreateBookmarkService : ChangeBookmarkService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateBookmarkService));

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
        ///     获取及设置新建一个收藏的校验器。
        /// </summary>
        public IValidator<BookmarkCreate> BookmarkCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置收藏的存储库。
        /// </summary>
        public IBookmarkRepository BookmarkRepo { get; set; }

        /// <summary>
        ///     获取及设置书籍的存储库。
        /// </summary>
        public IBookRepository BookRepo { get; set; }

        /// <summary>
        ///     获取及设置卷的存储库。
        /// </summary>
        public IVolumeRepository VolumeRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 新建一个收藏

        /// <summary>
        ///     新建一个收藏。
        /// </summary>
        public async Task<object> Post(BookmarkCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    BookmarkCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var currentUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUser == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            string catalog = null;
            string category = null;
            string title = null;
            string pictureUrl = null;
            var existingBookmark = await BookmarkRepo.GetBookmarkAsync(request.ParentId, currentUserId);
            if (existingBookmark != null)
            {
                switch (existingBookmark.ParentType)
                {
                    case "帖子":
                        var post = await PostRepo.GetPostAsync(existingBookmark.ParentId);
                        if (post != null)
                        {
                            title = post.Title;
                            pictureUrl = post.PictureUrl;
                        }
                        break;
                    case "章":
                        var chapter = await ChapterRepo.GetChapterAsync(existingBookmark.ParentId);
                        if (chapter != null)
                        {
                            catalog = (await BookRepo.GetBookAsync(chapter.BookId))?.Title;
                            category = (await VolumeRepo.GetVolumeAsync(chapter.VolumeId))?.Title;
                            title = chapter.Title;
                        }
                        break;
                    case "节":
                        var paragraph = await ParagraphRepo.GetParagraphAsync(existingBookmark.ParentId);
                        if (paragraph != null)
                        {
                            catalog = (await BookRepo.GetBookAsync(paragraph.BookId))?.Title;
                            category = string.Format("{0} {1}", (await VolumeRepo.GetVolumeAsync(paragraph.VolumeId))?.Title, (await ChapterRepo.GetChapterAsync(paragraph.ChapterId))?.Title);
                            title = paragraph.Content;
                        }
                        break;
                }
                return new BookmarkCreateResponse
                       {
                           Bookmark = existingBookmark.MapToBookmarkDto(catalog, category, title, pictureUrl, currentUser)
                       };
            }
            var newBookmark = new Bookmark
                              {
                                  ParentType = request.ParentType,
                                  ParentId = request.ParentId,
                                  UserId = currentUserId
                              };
            var bookmark = await BookmarkRepo.CreateBookmarkAsync(newBookmark);
            ResetCache(bookmark);
            switch (bookmark.ParentType)
            {
                case "帖子":
                    await PostRepo.IncrementPostBookmarksCountAsync(bookmark.ParentId, 1);
                    var post = await PostRepo.GetPostAsync(bookmark.ParentId);
                    if (post != null)
                    {
                        title = post.Title;
                        pictureUrl = post.PictureUrl;
                    }
                    break;
                case "章":
                    await ChapterRepo.IncrementChapterBookmarksCountAsync(bookmark.ParentId, 1);
                    var chapter = await ChapterRepo.GetChapterAsync(bookmark.ParentId);
                    if (chapter != null)
                    {
                        catalog = (await BookRepo.GetBookAsync(chapter.BookId))?.Title;
                        category = (await VolumeRepo.GetVolumeAsync(chapter.VolumeId))?.Title;
                        title = chapter.Title;
                    }
                    break;
                case "节":
                    await ParagraphRepo.IncrementParagraphBookmarksCountAsync(bookmark.ParentId, 1);
                    var paragraph = await ParagraphRepo.GetParagraphAsync(bookmark.ParentId);
                    if (paragraph != null)
                    {
                        catalog = (await BookRepo.GetBookAsync(paragraph.BookId))?.Title;
                        category = string.Format("{0} {1}", (await VolumeRepo.GetVolumeAsync(paragraph.VolumeId))?.Title, (await ChapterRepo.GetChapterAsync(paragraph.ChapterId))?.Title);
                        title = paragraph.Content;
                    }
                    break;
            }
            return new BookmarkCreateResponse
                   {
                       Bookmark = bookmark.MapToBookmarkDto(catalog, category, title, pictureUrl, currentUser)
                   };
        }

        #endregion
    }
}