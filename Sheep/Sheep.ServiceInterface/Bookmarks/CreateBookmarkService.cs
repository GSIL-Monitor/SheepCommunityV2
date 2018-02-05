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
            var title = string.Empty;
            var existingBookmark = await BookmarkRepo.GetBookmarkAsync(request.ParentId, currentUserId);
            if (existingBookmark != null)
            {
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
                return new BookmarkCreateResponse
                       {
                           Bookmark = existingBookmark.MapToBookmarkDto(currentUser, title)
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
                    title = (await PostRepo.GetPostAsync(bookmark.ParentId))?.Title;
                    await PostRepo.IncrementPostBookmarksCountAsync(bookmark.ParentId, 1);
                    break;
                case "章":
                    title = (await ChapterRepo.GetChapterAsync(bookmark.ParentId))?.Title;
                    await ChapterRepo.IncrementChapterBookmarksCountAsync(bookmark.ParentId, 1);
                    break;
                case "节":
                    title = (await ParagraphRepo.GetParagraphAsync(bookmark.ParentId))?.Content;
                    await ParagraphRepo.IncrementParagraphBookmarksCountAsync(bookmark.ParentId, 1);
                    break;
            }
            //await NimClient.PostAsync(new FriendAddRequest
            //                          {
            //                              AccountId = currentUserId.ToString(),
            //                              FriendAccountId = request.ParentId.ToString(),
            //                              Type = 1
            //                          });
            return new BookmarkCreateResponse
                   {
                       Bookmark = bookmark.MapToBookmarkDto(currentUser, title)
                   };
        }

        #endregion
    }
}