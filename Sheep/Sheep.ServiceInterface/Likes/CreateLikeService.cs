using System.Threading.Tasks;
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
using Sheep.ServiceInterface.Likes.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Likes;

namespace Sheep.ServiceInterface.Likes
{
    /// <summary>
    ///     新建一个点赞服务接口。
    /// </summary>
    public class CreateLikeService : ChangeLikeService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateLikeService));

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
        ///     获取及设置新建一个点赞的校验器。
        /// </summary>
        public IValidator<LikeCreate> LikeCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        #endregion

        #region 新建一个点赞

        /// <summary>
        ///     新建一个点赞。
        /// </summary>
        public async Task<object> Post(LikeCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    LikeCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var currentUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            var title = string.Empty;
            var existingLike = await LikeRepo.GetLikeAsync(request.ParentId, currentUserId);
            if (existingLike != null)
            {
                switch (existingLike.ParentType)
                {
                    case "帖子":
                        var post = await PostRepo.GetPostAsync(existingLike.ParentId);
                        if (post != null)
                        {
                            title = post.Title;
                        }
                        break;
                    case "章":
                        var chapter = await ChapterRepo.GetChapterAsync(existingLike.ParentId);
                        if (chapter != null)
                        {
                            title = chapter.Title;
                        }
                        break;
                    case "节":
                        var paragraph = await ParagraphRepo.GetParagraphAsync(existingLike.ParentId);
                        if (paragraph != null)
                        {
                            title = paragraph.Content;
                        }
                        break;
                }
                return new LikeCreateResponse
                       {
                           Like = existingLike.MapToLikeDto(currentUserAuth, title)
                       };
            }
            var newLike = new Like
                          {
                              ParentType = request.ParentType,
                              ParentId = request.ParentId,
                              UserId = currentUserId
                          };
            var like = await LikeRepo.CreateLikeAsync(newLike);
            ResetCache(like);
            switch (like.ParentType)
            {
                case "帖子":
                    await PostRepo.IncrementPostLikesCountAsync(like.ParentId, 1);
                    var post = await PostRepo.GetPostAsync(like.ParentId);
                    if (post != null)
                    {
                        title = post.Title;
                        await NimClient.PostAsync(new MessageSendAttachRequest
                                                  {
                                                      FromAccountId = currentUserId.ToString(),
                                                      MessageType = 0,
                                                      ToId = post.AuthorId.ToString(),
                                                      Attach = string.Format("{{\"Type\" : \"Like\", \"UserId\" : \"{0}\", \"UserDisplayName\" : \"{1}\", \"UserAvatarUrl\" : \"{2}\", \"PostId\" : \"{3}\", \"PostTitle\" : \"{4}\", \"PostPictureUrl\" : \"{5}\", \"PostContentType\" : \"{6}\", \"LikeId\" : \"{7}\", \"LikeCreatedDate\" : \"{8}\"}}", currentUserId, currentUserAuth.DisplayName, currentUserAuth.Meta?.GetValueOrDefault("AvatarUrl"), post.Id, post.Title, post.PictureUrl, post.ContentType, like.Id, like.CreatedDate.ToUnixTime()),
                                                      PushContent = string.Format("{0}赞了你的帖子《{1}》", currentUserAuth.DisplayName, post.Title),
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
                    await ChapterRepo.IncrementChapterLikesCountAsync(like.ParentId, 1);
                    var chapter = await ChapterRepo.GetChapterAsync(like.ParentId);
                    if (chapter != null)
                    {
                        title = chapter.Title;
                    }
                    break;
                case "节":
                    await ParagraphRepo.IncrementParagraphLikesCountAsync(like.ParentId, 1);
                    var paragraph = await ParagraphRepo.GetParagraphAsync(like.ParentId);
                    if (paragraph != null)
                    {
                        title = paragraph.Content;
                    }
                    break;
            }
            return new LikeCreateResponse
                   {
                       Like = like.MapToLikeDto(currentUserAuth, title)
                   };
        }

        #endregion
    }
}