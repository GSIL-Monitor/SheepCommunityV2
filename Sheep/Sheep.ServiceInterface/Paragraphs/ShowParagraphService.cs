using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Paragraphs.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Paragraphs;

namespace Sheep.ServiceInterface.Paragraphs
{
    /// <summary>
    ///     显示一节服务接口。
    /// </summary>
    public class ShowParagraphService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowParagraphService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一节的校验器。
        /// </summary>
        public IValidator<ParagraphShow> ParagraphShowValidator { get; set; }

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

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        /// <summary>
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

        #endregion

        #region 显示一节

        /// <summary>
        ///     显示一节。
        /// </summary>
        [CacheResponse(Duration = 3600, MaxAge = 1800)]
        public async Task<object> Get(ParagraphShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ParagraphShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingParagraph = await ParagraphRepo.GetParagraphAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber);
            if (existingParagraph == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ParagraphNotFound, string.Format("{0}-{1}-{2}-{3}", request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber)));
            }
            await ParagraphRepo.IncrementParagraphViewsCountAsync(existingParagraph.Id, 1);
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var commentsCount = await CommentRepo.GetCommentsCountByParentAsync(existingParagraph.Id, currentUserId, null, null, null, "审核通过");
            var paragraphAnnotations = await ParagraphAnnotationRepo.FindParagraphAnnotationsByParagraphAsync(existingParagraph.Id, null, null, null, null);
            var paragraphDto = existingParagraph.MapToParagraphDto(commentsCount > 0, paragraphAnnotations);
            return new ParagraphShowResponse
                   {
                       Paragraph = paragraphDto
                   };
        }

        #endregion
    }
}