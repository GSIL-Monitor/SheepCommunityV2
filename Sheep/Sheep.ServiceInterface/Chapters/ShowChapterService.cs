using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Read;
using Sheep.ServiceInterface.Chapters.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Chapters;

namespace Sheep.ServiceInterface.Chapters
{
    /// <summary>
    ///     显示一章服务接口。
    /// </summary>
    public class ShowChapterService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowChapterService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一章的校验器。
        /// </summary>
        public IValidator<ChapterShow> ChapterShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

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
        ///     获取及设置章注释的存储库。
        /// </summary>
        public IChapterAnnotationRepository ChapterAnnotationRepo { get; set; }

        #endregion

        #region 显示一章

        /// <summary>
        ///     显示一章。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(ChapterShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ChapterShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingChapter = await ChapterRepo.GetChapterAsync(request.BookId, request.VolumeNumber, request.ChapterNumber);
            if (existingChapter == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterNotFound, string.Format("{0}-{1}-{2}", request.BookId, request.VolumeNumber, request.ChapterNumber)));
            }
            var chapterAnnotations = await ChapterAnnotationRepo.FindChapterAnnotationsByChapterAsync(existingChapter.Id, null, null, null, null);
            var chapterDto = existingChapter.MapToChapterDto(chapterAnnotations);
            return new ChapterShowResponse
                   {
                       Chapter = chapterDto
                   };
        }

        #endregion
    }
}