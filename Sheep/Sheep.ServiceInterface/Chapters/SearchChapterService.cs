using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Read;
using Sheep.Model.Read.Entities;
using Sheep.ServiceInterface.Chapters.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Chapters;

namespace Sheep.ServiceInterface.Chapters
{
    /// <summary>
    ///     搜索一组章服务接口。
    /// </summary>
    public class SearchChapterService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(SearchChapterService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置搜索一组章的校验器。
        /// </summary>
        public IValidator<ChapterSearch> ChapterSearchValidator { get; set; }

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

        #region 搜索一组章

        /// <summary>
        ///     搜索一组章。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(ChapterSearch request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ChapterSearchValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingChapters = await ChapterRepo.FindChaptersAsync(request.BookId, request.VolumeNumber, request.ContentFilter, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingChapters == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChaptersNotFound));
            }
            var chapterAnnotationsMap = request.LoadAnnotations.HasValue && request.LoadAnnotations.Value ? (await ChapterAnnotationRepo.FindChapterAnnotationsByChaptersAsync(existingChapters.Select(chapter => chapter.Id), "ChapterId", null, null, null)).GroupBy(chapterAnnotation => chapterAnnotation.ChapterId, chapterAnnotation => chapterAnnotation).ToDictionary(grouping => grouping.Key, grouping => grouping.OrderBy(g => g.Number).ToList()) : new Dictionary<string, List<ChapterAnnotation>>();
            var chaptersDto = existingChapters.Select(chapter => chapter.MapToChapterDto(chapterAnnotationsMap.GetValueOrDefault(chapter.Id))).ToList();
            return new ChapterSearchResponse
                   {
                       Chapters = chaptersDto
                   };
        }

        #endregion
    }
}