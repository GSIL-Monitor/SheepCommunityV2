using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Chapters;

namespace Sheep.ServiceInterface.Chapters
{
    /// <summary>
    ///     删除一条章注释服务接口。
    /// </summary>
    public class DeleteChapterAnnotationService : ChangeChapterAnnotationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteChapterAnnotationService));

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
        ///     获取及设置删除一条章注释的校验器。
        /// </summary>
        public IValidator<ChapterAnnotationDelete> ChapterAnnotationDeleteValidator { get; set; }

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

        #region 删除一条章注释

        /// <summary>
        ///     删除一条章注释。
        /// </summary>
        public async Task<object> Delete(ChapterAnnotationDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ChapterAnnotationDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            }
            var existingChapterAnnotation = await ChapterAnnotationRepo.GetChapterAnnotationAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.AnnotationNumber);
            if (existingChapterAnnotation == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterAnnotationNotFound, string.Format("{0}-{1}-{2}-{3}", request.BookId, request.VolumeNumber, request.ChapterNumber, request.AnnotationNumber)));
            }
            await ChapterAnnotationRepo.DeleteChapterAnnotationAsync(existingChapterAnnotation.Id);
            ResetCache(existingChapterAnnotation);
            return new ChapterAnnotationDeleteResponse();
        }

        #endregion
    }
}