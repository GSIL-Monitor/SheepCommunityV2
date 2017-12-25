using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Replies;

namespace Sheep.ServiceInterface.Replies
{
    /// <summary>
    ///     删除一个回复服务接口。
    /// </summary>
    public class DeleteReplyService : ChangeReplyService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteReplyService));

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
        ///     获取及设置删除一个回复的校验器。
        /// </summary>
        public IValidator<ReplyDelete> ReplyDeleteValidator { get; set; }

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

        #region 删除一个回复

        /// <summary>
        ///     删除一个回复。
        /// </summary>
        public async Task<object> Delete(ReplyDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ReplyDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            }
            var existingReply = await ReplyRepo.GetReplyAsync(request.ReplyId);
            if (existingReply == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ReplyNotFound, request.ReplyId));
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            if (existingReply.UserId != currentUserId)
            {
                throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            }
            await ReplyRepo.DeleteReplyAsync(request.ReplyId);
            ResetCache(existingReply);
            switch (existingReply.ParentType)
            {
                case "评论":
                    await CommentRepo.IncrementCommentRepliesCountAsync(existingReply.ParentId, -1);
                    break;
            }
            return new ReplyDeleteResponse();
        }

        #endregion
    }
}