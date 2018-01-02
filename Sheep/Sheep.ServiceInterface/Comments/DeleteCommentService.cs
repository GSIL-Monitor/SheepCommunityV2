using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Comments;

namespace Sheep.ServiceInterface.Comments
{
    /// <summary>
    ///     删除一个评论服务接口。
    /// </summary>
    public class DeleteCommentService : ChangeCommentService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteCommentService));

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
        ///     获取及设置删除一个评论的校验器。
        /// </summary>
        public IValidator<CommentDelete> CommentDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        #endregion

        #region 删除一个评论

        /// <summary>
        ///     删除一个评论。
        /// </summary>
        public async Task<object> Delete(CommentDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    CommentDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var existingComment = await CommentRepo.GetCommentAsync(request.CommentId);
            if (existingComment == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CommentNotFound, request.CommentId));
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            if (existingComment.UserId != currentUserId)
            {
                throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            }
            await CommentRepo.DeleteCommentAsync(request.CommentId);
            ResetCache(existingComment);
            switch (existingComment.ParentType)
            {
                case "帖子":
                    await PostRepo.IncrementPostCommentsCountAsync(existingComment.ParentId, -1);
                    break;
                case "章":
                    await ChapterRepo.IncrementChapterCommentsCountAsync(existingComment.ParentId, -1);
                    break;
                case "节":
                    await ParagraphRepo.IncrementParagraphCommentsCountAsync(existingComment.ParentId, -1);
                    break;
            }
            return new CommentDeleteResponse();
        }

        #endregion
    }
}