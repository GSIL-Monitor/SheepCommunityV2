using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceInterface.ChapterReads.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.ChapterReads;

namespace Sheep.ServiceInterface.ChapterReads
{
    /// <summary>
    ///     新建一个阅读服务接口。
    /// </summary>
    public class CreateChapterReadService : ChangeChapterReadService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateChapterReadService));

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
        ///     获取及设置新建一个阅读的校验器。
        /// </summary>
        public IValidator<ChapterReadCreate> ChapterReadCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置阅读的存储库。
        /// </summary>
        public IChapterReadRepository ChapterReadRepo { get; set; }

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

        #endregion

        #region 新建一个阅读

        /// <summary>
        ///     新建一个阅读。
        /// </summary>
        public async Task<object> Post(ChapterReadCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ChapterReadCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var currentUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            var chapter = await ChapterRepo.GetChapterAsync(request.ChapterId);
            if (chapter == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterNotFound, request.ChapterId));
            }
            var book = await BookRepo.GetBookAsync(chapter.BookId);
            if (book == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BookNotFound, chapter.BookId));
            }
            var volume = await VolumeRepo.GetVolumeAsync(chapter.VolumeId);
            if (volume == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeNotFound, chapter.VolumeId));
            }
            var newChapterRead = new ChapterRead
                                 {
                                     BookId = chapter.BookId,
                                     VolumeId = chapter.VolumeId,
                                     ChapterId = chapter.Id,
                                     UserId = currentUserId
                                 };
            var chapterRead = await ChapterReadRepo.CreateChapterReadAsync(newChapterRead);
            ResetCache(chapterRead);
            return new ChapterReadCreateResponse
                   {
                       ChapterRead = chapterRead.MapToChapterReadDto(book, volume, chapter, currentUserAuth)
                   };
        }

        #endregion
    }
}