using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
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
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                LikeCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var userId = GetSession().UserAuthId.ToInt(0);
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(userId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, userId));
            }
            var title = string.Empty;
            var existingLike = await LikeRepo.GetLikeAsync(request.ParentId, userId);
            if (existingLike != null)
            {
                switch (existingLike.ParentType)
                {
                    case "帖子":
                        title = (await PostRepo.GetPostAsync(existingLike.ParentId))?.Title;
                        break;
                    case "章":
                        title = (await ChapterRepo.GetChapterAsync(existingLike.ParentId))?.Title;
                        break;
                    case "节":
                        title = (await ParagraphRepo.GetParagraphAsync(existingLike.ParentId))?.Content;
                        break;
                }
                return new LikeCreateResponse
                       {
                           Like = existingLike.MapToLikeDto(user, title)
                       };
            }
            var newLike = new Like
                          {
                              ParentType = request.ParentType,
                              ParentId = request.ParentId,
                              UserId = userId
                          };
            var like = await LikeRepo.CreateLikeAsync(newLike);
            ResetCache(like);
            switch (like.ParentType)
            {
                case "帖子":
                    title = (await PostRepo.GetPostAsync(like.ParentId))?.Title;
                    await PostRepo.IncrementPostLikesCountAsync(like.ParentId, 1);
                    break;
                case "章":
                    title = (await ChapterRepo.GetChapterAsync(like.ParentId))?.Title;
                    await ChapterRepo.IncrementChapterLikesCountAsync(like.ParentId, 1);
                    break;
                case "节":
                    title = (await ParagraphRepo.GetParagraphAsync(like.ParentId))?.Content;
                    await ParagraphRepo.IncrementParagraphLikesCountAsync(like.ParentId, 1);
                    break;
            }
            //await NimClient.PostAsync(new FriendAddRequest
            //                          {
            //                              AccountId = userId.ToString(),
            //                              FriendAccountId = request.ParentId.ToString(),
            //                              Type = 1
            //                          });
            return new LikeCreateResponse
                   {
                       Like = like.MapToLikeDto(user, title)
                   };
        }

        #endregion
    }
}