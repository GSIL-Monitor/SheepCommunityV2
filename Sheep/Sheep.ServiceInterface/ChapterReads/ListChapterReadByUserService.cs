using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.ChapterReads.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.ChapterReads;

namespace Sheep.ServiceInterface.ChapterReads
{
    /// <summary>
    ///     根据用户列举一组阅读信息服务接口。
    /// </summary>
    public class ListChapterReadByUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListChapterReadByUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组阅读的校验器。
        /// </summary>
        public IValidator<ChapterReadListByUser> ChapterReadListByUserValidator { get; set; }

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

        #region 列举一组阅读

        /// <summary>
        ///     列举一组阅读。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(ChapterReadListByUser request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ChapterReadListByUserValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingChapterReads = await ChapterReadRepo.FindChapterReadsByUserAsync(request.UserId, request.BookId, request.CreatedSince?.FromUnixTime(), request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingChapterReads == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterReadsNotFound));
            }
            var booksMap = (await BookRepo.GetBooksAsync(existingChapterReads.Select(chapterRead => chapterRead.BookId).Distinct().ToList())).ToDictionary(book => book.Id, book => book);
            var volumesMap = (await VolumeRepo.GetVolumesAsync(existingChapterReads.Select(chapterRead => chapterRead.VolumeId).Distinct().ToList())).ToDictionary(volume => volume.Id, volume => volume);
            var chaptersMap = (await ChapterRepo.GetChaptersAsync(existingChapterReads.Select(chapterRead => chapterRead.ChapterId).Distinct().ToList())).ToDictionary(chapter => chapter.Id, chapter => chapter);
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingChapterReads.Select(chapterRead => chapterRead.UserId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var chapterReadsDto = existingChapterReads.Select(chapterRead => chapterRead.MapToChapterReadDto(booksMap.GetValueOrDefault(chapterRead.BookId), volumesMap.GetValueOrDefault(chapterRead.VolumeId), chaptersMap.GetValueOrDefault(chapterRead.ChapterId), usersMap.GetValueOrDefault(chapterRead.UserId))).ToList();
            return new ChapterReadListResponse
                   {
                       ChapterReads = chapterReadsDto
                   };
        }

        #endregion
    }
}