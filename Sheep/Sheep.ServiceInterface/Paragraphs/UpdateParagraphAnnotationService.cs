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
    ///     更新一条节注释服务接口。
    /// </summary>
    public class UpdateParagraphAnnotationService : ChangeParagraphAnnotationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdateParagraphAnnotationService));

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
        ///     获取及设置更新一条节注释的校验器。
        /// </summary>
        public IValidator<ParagraphAnnotationUpdate> ParagraphAnnotationUpdateValidator { get; set; }

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

        #region 更新一条节注释

        /// <summary>
        ///     更新一条节注释。
        /// </summary>
        public async Task<object> Put(ParagraphAnnotationUpdate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ParagraphAnnotationUpdateValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingParagraphAnnotation = await ParagraphAnnotationRepo.GetParagraphAnnotationAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber, request.AnnotationNumber);
            if (existingParagraphAnnotation == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ParagraphAnnotationNotFound, string.Format("{0}-{1}-{2}-{3}-{4}", request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber, request.AnnotationNumber)));
            }
            var newParagraphAnnotation = new ParagraphAnnotation();
            newParagraphAnnotation.PopulateWith(existingParagraphAnnotation);
            newParagraphAnnotation.Meta = existingParagraphAnnotation.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingParagraphAnnotation.Meta);
            newParagraphAnnotation.Title = request.Title?.Replace("\"", "'");
            newParagraphAnnotation.Annotation = request.Annotation?.Replace("\"", "'");
            var paragraphAnnotation = await ParagraphAnnotationRepo.UpdateParagraphAnnotationAsync(existingParagraphAnnotation, newParagraphAnnotation);
            ResetCache(paragraphAnnotation);
            return new ParagraphAnnotationUpdateResponse
                   {
                       ParagraphAnnotation = paragraphAnnotation.MapToParagraphAnnotationDto()
                   };
        }

        #endregion
    }
}