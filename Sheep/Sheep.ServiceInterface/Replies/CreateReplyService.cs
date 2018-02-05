using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Replies.Mappers;
using Sheep.ServiceModel.Replies;

namespace Sheep.ServiceInterface.Replies
{
    /// <summary>
    ///     新建一个回复服务接口。
    /// </summary>
    public class CreateReplyService : ChangeReplyService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateReplyService));

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
        ///     获取及设置新建一个回复的校验器。
        /// </summary>
        public IValidator<ReplyCreate> ReplyCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        /// <summary>
        ///     获取及设置回复的存储库。
        /// </summary>
        public IReplyRepository ReplyRepo { get; set; }

        #endregion

        #region 新建一个回复

        /// <summary>
        ///     新建一个回复。
        /// </summary>
        public async Task<object> Post(ReplyCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ReplyCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var currentUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUser == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            var newReply = new Reply
                           {
                               ParentType = request.ParentType,
                               ParentId = request.ParentId,
                               UserId = currentUserId,
                               Content = request.Content?.Replace("\"", "'")
                           };
            var reply = await ReplyRepo.CreateReplyAsync(newReply);
            await ReplyRepo.UpdateReplyContentQualityAsync(reply.Id, ReplyRepo.CalculateReplyContentQuality(reply));
            ResetCache(reply);
            switch (reply.ParentType)
            {
                case "评论":
                    await CommentRepo.IncrementCommentRepliesCountAsync(reply.ParentId, 1);
                    break;
            }
            //await NimClient.PostAsync(new FriendAddRequest
            //                          {
            //                              AccountId = currentUserId.ToString(),
            //                              FriendAccountId = request.ParentId.ToString(),
            //                              Type = 1
            //                          });
            return new ReplyCreateResponse
                   {
                       Reply = reply.MapToReplyDto(currentUser, false, false)
                   };
        }

        #endregion
    }
}