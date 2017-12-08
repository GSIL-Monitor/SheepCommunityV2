using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
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
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                CommentCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var userId = GetSession().UserAuthId.ToInt(0);
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(userId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, userId));
            }
            var newComment = new Comment
                             {
                                 ParentType = request.ParentType,
                                 ParentId = request.ParentId,
                                 UserId = userId,
                                 Content = request.Content?.Replace("\"", "'")
                             };
            var comment = await CommentRepo.CreateCommentAsync(newComment);
            ResetCache(comment);
            switch (comment.ParentType)
            {
                case "帖子":
                    await PostRepo.IncrementPostCommentsCountAsync(comment.ParentId, 1);
                    break;
            }
            //await NimClient.PostAsync(new FriendAddRequest
            //                          {
            //                              AccountId = userId.ToString(),
            //                              FriendAccountId = request.ParentId.ToString(),
            //                              Type = 1
            //                          });
            return new CommentCreateResponse
                   {
                       Comment = comment.MapToCommentDto(user, false, false)
                   };
        }

        #endregion
    }
}