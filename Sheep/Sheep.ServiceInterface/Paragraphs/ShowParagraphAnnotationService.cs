using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Paragraphs.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Paragraphs;

namespace Sheep.ServiceInterface.Paragraphs
{
    /// <summary>
    ///     显示一条节注释服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowParagraphAnnotationService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowParagraphAnnotationService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一条节注释的校验器。
        /// </summary>
        public IValidator<ParagraphAnnotationShow> ParagraphAnnotationShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置书籍的存储库。
        /// </summary>
        public IBookRepository BookRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        /// <summary>
        ///     获取及设置节注释的存储库。
        /// </summary>
        public IParagraphAnnotationRepository ParagraphAnnotationRepo { get; set; }

        #endregion

        #region 显示一条节注释

        /// <summary>
        ///     显示一条节注释。
        /// </summary>
        [CacheResponse(Duration = 31536000, MaxAge = 86400)]
        public async Task<object> Get(ParagraphAnnotationShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ParagraphAnnotationShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingParagraphAnnotation = await ParagraphAnnotationRepo.GetParagraphAnnotationAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber, request.AnnotationNumber);
            if (existingParagraphAnnotation == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ParagraphAnnotationNotFound, string.Format("{0}-{1}-{2}-{3}-{4}", request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber, request.AnnotationNumber)));
            }
            var paragraphAnnotationDto = existingParagraphAnnotation.MapToParagraphAnnotationDto();
            return new ParagraphAnnotationShowResponse
                   {
                       ParagraphAnnotation = paragraphAnnotationDto
                   };
        }

        #endregion
    }
}