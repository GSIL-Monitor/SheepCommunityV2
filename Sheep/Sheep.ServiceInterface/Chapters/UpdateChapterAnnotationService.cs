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
    ///     更新一条章注释服务接口。
    /// </summary>
    [CompressResponse]
    public class UpdateChapterAnnotationService : ChangeChapterAnnotationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdateChapterAnnotationService));

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
        ///     获取及设置更新一条章注释的校验器。
        /// </summary>
        public IValidator<ChapterAnnotationUpdate> ChapterAnnotationUpdateValidator { get; set; }

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

        #region 更新一条章注释

        /// <summary>
        ///     更新一条章注释。
        /// </summary>
        public async Task<object> Put(ChapterAnnotationUpdate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ChapterAnnotationUpdateValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingChapterAnnotation = await ChapterAnnotationRepo.GetChapterAnnotationAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.AnnotationNumber);
            if (existingChapterAnnotation == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterAnnotationNotFound, string.Format("{0}-{1}-{2}-{3}", request.BookId, request.VolumeNumber, request.ChapterNumber, request.AnnotationNumber)));
            }
            var newChapterAnnotation = new ChapterAnnotation();
            newChapterAnnotation.PopulateWith(existingChapterAnnotation);
            newChapterAnnotation.Meta = existingChapterAnnotation.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingChapterAnnotation.Meta);
            newChapterAnnotation.Title = request.Title?.Replace("\"", "'");
            newChapterAnnotation.Annotation = request.Annotation?.Replace("\"", "'");
            var chapterAnnotation = await ChapterAnnotationRepo.UpdateChapterAnnotationAsync(existingChapterAnnotation, newChapterAnnotation);
            ResetCache(chapterAnnotation);
            return new ChapterAnnotationUpdateResponse
                   {
                       ChapterAnnotation = chapterAnnotation.MapToChapterAnnotationDto()
                   };
        }

        #endregion
    }
}