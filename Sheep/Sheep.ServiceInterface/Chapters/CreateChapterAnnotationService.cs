using System.Collections.Generic;
using System.Threading.Tasks;
using Aliyun.OSS;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceInterface.Chapters.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Chapters;

namespace Sheep.ServiceInterface.Chapters
{
    /// <summary>
    ///     新建一条章注释服务接口。
    /// </summary>
    public class CreateChapterAnnotationService : ChangeChapterAnnotationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateChapterAnnotationService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置阿里云对象存储客户端。
        /// </summary>
        public IOss OssClient { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置新建一条章注释的校验器。
        /// </summary>
        public IValidator<ChapterAnnotationCreate> ChapterAnnotationCreateValidator { get; set; }

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

        #region 新建一条章注释

        /// <summary>
        ///     新建一条章注释。
        /// </summary>
        public async Task<object> Post(ChapterAnnotationCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ChapterAnnotationCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var existingChapterAnnotation = await ChapterAnnotationRepo.GetChapterAnnotationAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.AnnotationNumber);
            if (existingChapterAnnotation != null)
            {
                return new ChapterAnnotationCreateResponse
                       {
                           ChapterAnnotation = existingChapterAnnotation.MapToChapterAnnotationDto()
                       };
            }
            var existingChapter = await ChapterRepo.GetChapterAsync(request.BookId, request.VolumeNumber, request.ChapterNumber);
            if (existingChapter == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterNotFound, string.Format("{0}-{1}-{2}", request.BookId, request.VolumeNumber, request.ChapterNumber)));
            }
            var newChapterAnnotation = new ChapterAnnotation
                                       {
                                           Meta = new Dictionary<string, string>(),
                                           BookId = existingChapter.BookId,
                                           VolumeId = existingChapter.VolumeId,
                                           VolumeNumber = existingChapter.VolumeNumber,
                                           ChapterId = existingChapter.Id,
                                           ChapterNumber = existingChapter.Number,
                                           Number = request.AnnotationNumber,
                                           Title = request.Title?.Replace("\"", "'"),
                                           Annotation = request.Annotation?.Replace("\"", "'")
                                       };
            var chapterAnnotation = await ChapterAnnotationRepo.CreateChapterAnnotationAsync(newChapterAnnotation);
            ResetCache(chapterAnnotation);
            return new ChapterAnnotationCreateResponse
                   {
                       ChapterAnnotation = chapterAnnotation.MapToChapterAnnotationDto()
                   };
        }

        #endregion
    }
}