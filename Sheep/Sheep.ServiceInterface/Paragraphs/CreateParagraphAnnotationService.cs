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
using Sheep.ServiceInterface.Paragraphs.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Paragraphs;

namespace Sheep.ServiceInterface.Paragraphs
{
    /// <summary>
    ///     新建一条节注释服务接口。
    /// </summary>
    public class CreateParagraphAnnotationService : ChangeParagraphAnnotationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateParagraphAnnotationService));

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
        ///     获取及设置新建一条节注释的校验器。
        /// </summary>
        public IValidator<ParagraphAnnotationCreate> ParagraphAnnotationCreateValidator { get; set; }

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

        #region 新建一条节注释

        /// <summary>
        ///     新建一条节注释。
        /// </summary>
        public async Task<object> Post(ParagraphAnnotationCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ParagraphAnnotationCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var existingParagraphAnnotation = await ParagraphAnnotationRepo.GetParagraphAnnotationAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber, request.AnnotationNumber);
            if (existingParagraphAnnotation != null)
            {
                return new ParagraphAnnotationCreateResponse
                       {
                           ParagraphAnnotation = existingParagraphAnnotation.MapToParagraphAnnotationDto()
                       };
            }
            var existingParagraph = await ParagraphRepo.GetParagraphAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber);
            if (existingParagraph == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ParagraphNotFound, string.Format("{0}-{1}-{2}-{3}", request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber)));
            }
            var newParagraphAnnotation = new ParagraphAnnotation
                                         {
                                             Meta = new Dictionary<string, string>(),
                                             BookId = existingParagraph.BookId,
                                             VolumeId = existingParagraph.VolumeId,
                                             VolumeNumber = existingParagraph.VolumeNumber,
                                             ChapterId = existingParagraph.ChapterId,
                                             ChapterNumber = existingParagraph.ChapterNumber,
                                             ParagraphId = existingParagraph.Id,
                                             ParagraphNumber = existingParagraph.Number,
                                             Number = request.AnnotationNumber,
                                             Title = request.Title?.Replace("\"", "'"),
                                             Annotation = request.Annotation?.Replace("\"", "'")
                                         };
            var paragraphAnnotation = await ParagraphAnnotationRepo.CreateParagraphAnnotationAsync(newParagraphAnnotation);
            ResetCache(paragraphAnnotation);
            return new ParagraphAnnotationCreateResponse
                   {
                       ParagraphAnnotation = paragraphAnnotation.MapToParagraphAnnotationDto()
                   };
        }

        #endregion
    }
}