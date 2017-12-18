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
using Sheep.Model.Content;
using Sheep.Model.Read;
using Sheep.Model.Read.Entities;
using Sheep.ServiceInterface.Paragraphs.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Paragraphs;

namespace Sheep.ServiceInterface.Paragraphs
{
    /// <summary>
    ///     更新一节服务接口。
    /// </summary>
    public class UpdateParagraphService : ChangeParagraphService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdateParagraphService));

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
        ///     获取及设置更新一节的校验器。
        /// </summary>
        public IValidator<ParagraphUpdate> ParagraphUpdateValidator { get; set; }

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

        #region 更新一节

        /// <summary>
        ///     更新一节。
        /// </summary>
        public async Task<object> Put(ParagraphUpdate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ParagraphUpdateValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var existingParagraph = await ParagraphRepo.GetParagraphAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber);
            if (existingParagraph == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ParagraphNotFound, string.Format("{0}-{1}-{2}-{3}", request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber)));
            }
            var newParagraph = new Paragraph();
            newParagraph.PopulateWith(existingParagraph);
            newParagraph.Meta = existingParagraph.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingParagraph.Meta);
            newParagraph.Content = request.Content?.Replace("\"", "'");
            var paragraph = await ParagraphRepo.UpdateParagraphAsync(existingParagraph, newParagraph);
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var commentsCount = await CommentRepo.GetCommentsCountByParentAsync(paragraph.Id, currentUserId, null, null, null, "审核通过");
            var paragraphAnnotations = await ParagraphAnnotationRepo.FindParagraphAnnotationsByParagraphAsync(paragraph.Id, null, null, null, null);
            ResetCache(paragraph);
            return new ParagraphUpdateResponse
                   {
                       Paragraph = paragraph.MapToParagraphDto(commentsCount > 0, paragraphAnnotations)
                   };
        }

        #endregion
    }
}