using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Chapters.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Chapters;

namespace Sheep.ServiceInterface.Chapters
{
    /// <summary>
    ///     列举一组章注释服务接口。
    /// </summary>
    public class ListChapterAnnotationService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListChapterAnnotationService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组章注释的校验器。
        /// </summary>
        public IValidator<ChapterAnnotationList> ChapterAnnotationListValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置书籍的存储库。
        /// </summary>
        public IBookRepository BookRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置章注释的存储库。
        /// </summary>
        public IChapterAnnotationRepository ChapterAnnotationRepo { get; set; }

        #endregion

        #region 列举一组章注释

        /// <summary>
        ///     列举一组章注释。
        /// </summary>
        [CacheResponse(Duration = 31536000, MaxAge = 86400)]
        public async Task<object> Get(ChapterAnnotationList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ChapterAnnotationListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingChapterAnnotations = await ChapterAnnotationRepo.FindChapterAnnotationsAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.AnnotationFilter, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingChapterAnnotations == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterAnnotationsNotFound));
            }
            var chapterAnnotationsDto = existingChapterAnnotations.Select(chapterAnnotation => chapterAnnotation.MapToChapterAnnotationDto()).ToList();
            return new ChapterAnnotationListResponse
                   {
                       ChapterAnnotations = chapterAnnotationsDto
                   };
        }

        #endregion
    }
}