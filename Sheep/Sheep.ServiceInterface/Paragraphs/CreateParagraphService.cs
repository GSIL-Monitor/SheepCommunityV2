using System.Collections.Generic;
using System.Threading.Tasks;
using Aliyun.OSS;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Read;
using Sheep.Model.Read.Entities;
using Sheep.ServiceInterface.Paragraphs.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Paragraphs;

namespace Sheep.ServiceInterface.Paragraphs
{
    /// <summary>
    ///     新建一节服务接口。
    /// </summary>
    public class CreateParagraphService : ChangeParagraphService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateParagraphService));

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
        ///     获取及设置新建一节的校验器。
        /// </summary>
        public IValidator<ParagraphCreate> ParagraphCreateValidator { get; set; }

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
        ///     获取及设置主题的存储库。
        /// </summary>
        public ISubjectRepository SubjectRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        /// <summary>
        ///     获取及设置节注释的存储库。
        /// </summary>
        public IParagraphAnnotationRepository ParagraphAnnotationRepo { get; set; }

        #endregion

        #region 新建一节

        /// <summary>
        ///     新建一节。
        /// </summary>
        public async Task<object> Post(ParagraphCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ParagraphCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var existingParagraph = await ParagraphRepo.GetParagraphAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber);
            if (existingParagraph != null)
            {
                var paragraphAnnotations = await ParagraphAnnotationRepo.FindParagraphAnnotationsByParagraphAsync(existingParagraph.Id, null, null, null, null);
                return new ParagraphCreateResponse
                       {
                           Paragraph = existingParagraph.MapToParagraphDto(paragraphAnnotations)
                       };
            }
            var existingChapter = await ChapterRepo.GetChapterAsync(request.BookId, request.VolumeNumber, request.ChapterNumber);
            if (existingChapter == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterNotFound, string.Format("{0}-{1}-{2}", request.BookId, request.VolumeNumber, request.ChapterNumber)));
            }
            Subject existingSubject = null;
            if (request.SubjectNumber.HasValue)
            {
                existingSubject = await SubjectRepo.GetSubjectAsync(request.BookId, request.VolumeNumber, request.SubjectNumber.Value);
            }
            var newParagraph = new Paragraph
                               {
                                   Meta = new Dictionary<string, string>(),
                                   BookId = existingChapter.BookId,
                                   VolumeId = existingChapter.VolumeId,
                                   VolumeNumber = existingChapter.VolumeNumber,
                                   ChapterId = existingChapter.Id,
                                   ChapterNumber = existingChapter.Number,
                                   SubjectId = existingSubject?.Id,
                                   SubjectNumber = existingSubject?.Number,
                                   Number = request.ParagraphNumber,
                                   Content = request.Content?.Replace("\"", "'")
                               };
            var paragraph = await ParagraphRepo.CreateParagraphAsync(newParagraph);
            await ChapterRepo.IncrementChapterParagraphsCountAsync(paragraph.ChapterId, 1);
            ResetCache(paragraph);
            return new ParagraphCreateResponse
                   {
                       Paragraph = paragraph.MapToParagraphDto(new List<ParagraphAnnotation>())
                   };
        }

        #endregion
    }
}