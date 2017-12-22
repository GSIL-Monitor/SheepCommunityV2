using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.OSS;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Content;
using Sheep.Model.Bookstore;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceInterface.Chapters.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Chapters;

namespace Sheep.ServiceInterface.Chapters
{
    /// <summary>
    ///     更新一章服务接口。
    /// </summary>
    public class UpdateChapterService : ChangeChapterService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdateChapterService));

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
        ///     获取及设置更新一章的校验器。
        /// </summary>
        public IValidator<ChapterUpdate> ChapterUpdateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置书籍的存储库。
        /// </summary>
        public IBookRepository BookRepo { get; set; }

        /// <summary>
        ///     获取及设置卷的存储库。
        /// </summary>
        public IVolumeRepository VolumeRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置章注释的存储库。
        /// </summary>
        public IChapterAnnotationRepository ChapterAnnotationRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        /// <summary>
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

        #endregion

        #region 更新一章

        /// <summary>
        ///     更新一章。
        /// </summary>
        public async Task<object> Put(ChapterUpdate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ChapterUpdateValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var existingChapter = await ChapterRepo.GetChapterAsync(request.BookId, request.VolumeNumber, request.ChapterNumber);
            if (existingChapter == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterNotFound, string.Format("{0}-{1}-{2}", request.BookId, request.VolumeNumber, request.ChapterNumber)));
            }
            var newChapter = new Chapter();
            newChapter.PopulateWith(existingChapter);
            newChapter.Meta = existingChapter.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingChapter.Meta);
            newChapter.Title = request.Title?.Replace("\"", "'");
            newChapter.Content = request.Content?.Replace("\"", "'");
            var chapter = await ChapterRepo.UpdateChapterAsync(existingChapter, newChapter);
            var chapterAnnotations = await ChapterAnnotationRepo.FindChapterAnnotationsByChapterAsync(chapter.Id, null, null, null, null);
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var paragraphs = await ParagraphRepo.FindParagraphsByChapterAsync(chapter.Id, null, null, null, null);
            var paragraphCommentsMap = (await CommentRepo.GetCommentsCountByParentsAsync(paragraphs.Select(paragraph => paragraph.Id), currentUserId, null, null, null, "审核通过")).ToDictionary(pair => pair.Key, pair => pair.Value);
            ResetCache(chapter);
            return new ChapterUpdateResponse
                   {
                       Chapter = chapter.MapToChapterDto(chapterAnnotations, paragraphs, paragraphCommentsMap)
                   };
        }

        #endregion
    }
}