using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.Model.Bookstore.Entities;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Paragraphs.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Paragraphs;

namespace Sheep.ServiceInterface.Paragraphs
{
    /// <summary>
    ///     搜索一组节服务接口。
    /// </summary>
    public class SearchParagraphService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(SearchParagraphService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置搜索一组节的校验器。
        /// </summary>
        public IValidator<ParagraphSearch> ParagraphSearchValidator { get; set; }

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

        #region 搜索一组节

        /// <summary>
        ///     搜索一组节。
        /// </summary>
        [CacheResponse(Duration = 3600, MaxAge = 1800)]
        public async Task<object> Get(ParagraphSearch request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ParagraphSearchValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingParagraphs = await ParagraphRepo.FindParagraphsAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.ContentFilter, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingParagraphs == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ParagraphsNotFound));
            }
            var paragraphAnnotationsMap = request.LoadAnnotations.HasValue && request.LoadAnnotations.Value ? (await ParagraphAnnotationRepo.FindParagraphAnnotationsByParagraphsAsync(existingParagraphs.Select(paragraph => paragraph.Id).ToList(), "ParagraphId", null, null, null)).GroupBy(paragraphAnnotation => paragraphAnnotation.ParagraphId, paragraphAnnotation => paragraphAnnotation).ToDictionary(grouping => grouping.Key, grouping => grouping.OrderBy(g => g.Number).ToList()) : new Dictionary<string, List<ParagraphAnnotation>>();
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var commentsMap = (await CommentRepo.GetCommentsCountByParentsAsync(existingParagraphs.Select(paragraph => paragraph.Id).ToList(), currentUserId, null, null, null, "审核通过")).ToDictionary(pair => pair.Key, pair => pair.Value);
            var paragraphsDto = existingParagraphs.Select(paragraph => paragraph.MapToParagraphDto(commentsMap.GetValueOrDefault(paragraph.Id) > 0, paragraphAnnotationsMap.GetValueOrDefault(paragraph.Id))).ToList();
            return new ParagraphSearchResponse
                   {
                       Paragraphs = paragraphsDto
                   };
        }

        #endregion
    }
}