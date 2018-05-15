using System.Threading;
using System.Threading.Tasks;
using Aliyun.Green;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Comments.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Comments;

namespace Sheep.ServiceInterface.Comments
{
    /// <summary>
    ///     新建一个评论服务接口。
    /// </summary>
    public class CreateCommentService : ChangeCommentService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateCommentService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置阿里云内容安全服务客户端。
        /// </summary>
        public IGreenClient GreenClient { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置新建一个评论的校验器。
        /// </summary>
        public IValidator<CommentCreate> CommentCreateValidator { get; set; }

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

        #region 新建一个评论

        /// <summary>
        ///     新建一个评论。
        /// </summary>
        public async Task<object> Post(CommentCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    CommentCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var currentUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUser == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            string title = null;
            string pictureUrl = null;
            string contentType = null;
            var newComment = new Comment
                             {
                                 ParentType = request.ParentType,
                                 ParentId = request.ParentId,
                                 UserId = currentUserId,
                                 Content = request.Content?.Replace("\"", "'")
                             };
            if (!newComment.Content.IsNullOrEmpty() && !await AliyunHelper.IsTextValidAsync(GreenClient, newComment.Content, "comment", "new"))
            {
                throw HttpError.Forbidden(string.Format(Resources.InvalidContent, newComment.Content));
            }
            var comment = await CommentRepo.CreateCommentAsync(newComment);
            await CommentRepo.UpdateCommentContentQualityAsync(comment.Id, CommentRepo.CalculateCommentContentQuality(comment));
            ResetCache(comment);
            switch (comment.ParentType)
            {
                case "帖子":
                    await PostRepo.IncrementPostCommentsCountAsync(comment.ParentId, 1);
                    var post = await PostRepo.GetPostAsync(comment.ParentId);
                    if (post != null)
                    {
                        title = post.Title;
                        pictureUrl = post.PictureUrl;
                        contentType = post.ContentType;
                        await NimClient.PostAsync(new MessageSendAttachRequest
                                                  {
                                                      FromAccountId = currentUserId.ToString(),
                                                      MessageType = 0,
                                                      ToId = post.AuthorId.ToString(),
                                                      Attach = string.Format("{{\"Type\" : \"Comment\", \"UserId\" : \"{0}\", \"UserDisplayName\" : \"{1}\", \"UserAvatarUrl\" : \"{2}\", \"PostId\" : \"{3}\", \"PostTitle\" : \"{4}\", \"PostPictureUrl\" : \"{5}\", \"PostContentType\" : \"{6}\", \"CommentId\" : \"{7}\", \"CommentContent\" : \"{8}\", \"CommentCreatedDate\" : \"{9}\"}}", currentUserId, currentUser.DisplayName, currentUser.Meta?.GetValueOrDefault("AvatarUrl"), post.Id, post.Title, post.PictureUrl, post.ContentType, comment.Id, comment.Content, comment.CreatedDate.ToUnixTime()),
                                                      PushContent = string.Format("{0}评论了你的帖子《{1}》", currentUser.DisplayName, post.Title),
                                                      Option = new MessageSendAttachOption
                                                               {
                                                                   Badge = true,
                                                                   NeedPushNick = false,
                                                                   Route = false
                                                               }
                                                  });
                    }
                    break;
                case "章":
                    await ChapterRepo.IncrementChapterCommentsCountAsync(comment.ParentId, 1);
                    var chapter = await ChapterRepo.GetChapterAsync(comment.ParentId);
                    if (chapter != null)
                    {
                        title = chapter.Title;
                    }
                    break;
                case "节":
                    await ParagraphRepo.IncrementParagraphCommentsCountAsync(comment.ParentId, 1);
                    var paragraph = await ParagraphRepo.GetParagraphAsync(comment.ParentId);
                    if (paragraph != null)
                    {
                        title = paragraph.Content;
                    }
                    break;
            }
            return new CommentCreateResponse
                   {
                       Comment = comment.MapToCommentDto(title, pictureUrl, contentType, currentUser, false, false)
                   };
        }

        #endregion
    }
}