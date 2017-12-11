using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Read;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Paragraphs;

namespace Sheep.ServiceInterface.Paragraphs
{
    /// <summary>
    ///     删除一节服务接口。
    /// </summary>
    public class DeleteParagraphService : ChangeParagraphService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteParagraphService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置删除一节的校验器。
        /// </summary>
        public IValidator<ParagraphDelete> ParagraphDeleteValidator { get; set; }

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

        #region 删除一节

        /// <summary>
        ///     删除一节。
        /// </summary>
        public async Task<object> Delete(ParagraphDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ParagraphDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            }
            var existingParagraph = await ParagraphRepo.GetParagraphAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber);
            if (existingParagraph == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ParagraphNotFound, string.Format("{0}-{1}-{2}-{3}", request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber)));
            }
            await ParagraphRepo.DeleteParagraphAsync(existingParagraph.Id);
            await ChapterRepo.IncrementChapterParagraphsCountAsync(existingParagraph.ChapterId, -1);
            ResetCache(existingParagraph);
            return new ParagraphDeleteResponse();
        }

        #endregion
    }
}