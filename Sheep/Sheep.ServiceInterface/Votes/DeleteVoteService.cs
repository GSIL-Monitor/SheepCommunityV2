using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Votes;

namespace Sheep.ServiceInterface.Votes
{
    /// <summary>
    ///     取消一个投票服务接口。
    /// </summary>
    public class DeleteVoteService : ChangeVoteService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteVoteService));

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
        ///     获取及设置取消一个投票的校验器。
        /// </summary>
        public IValidator<VoteDelete> VoteDeleteValidator { get; set; }

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

        /// <summary>
        ///     获取及设置投票的存储库。
        /// </summary>
        public IVoteRepository VoteRepo { get; set; }

        #endregion

        #region 取消一个投票

        /// <summary>
        ///     取消一个投票。
        /// </summary>
        public async Task<object> Delete(VoteDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    VoteDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var existingVote = await VoteRepo.GetVoteAsync(request.ParentId, currentUserId);
            if (existingVote == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VoteNotFound, request.ParentId));
            }
            //if (existingVote.UserId != currentUserId)
            //{
            //    throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            //}
            await VoteRepo.DeleteVoteAsync(request.ParentId, currentUserId);
            ResetCache(existingVote);
            switch (existingVote.ParentType)
            {
                case "评论":
                    if (existingVote.Value)
                    {
                        await CommentRepo.IncrementCommentVotesAndYesVotesCountAsync(existingVote.ParentId, -1);
                    }
                    else
                    {
                        await CommentRepo.IncrementCommentVotesAndNoVotesCountAsync(existingVote.ParentId, -1);
                    }
                    break;
                case "回复":
                    if (existingVote.Value)
                    {
                        await ReplyRepo.IncrementReplyVotesAndYesVotesCountAsync(existingVote.ParentId, -1);
                    }
                    else
                    {
                        await ReplyRepo.IncrementReplyVotesAndNoVotesCountAsync(existingVote.ParentId, -1);
                    }
                    break;
            }
            return new VoteDeleteResponse();
        }

        #endregion
    }
}