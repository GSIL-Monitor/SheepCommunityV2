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
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Votes.Mappers;
using Sheep.ServiceModel.Votes;

namespace Sheep.ServiceInterface.Votes
{
    /// <summary>
    ///     新建一个投票服务接口。
    /// </summary>
    public class CreateVoteService : ChangeVoteService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateVoteService));

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
        ///     获取及设置新建一个投票的校验器。
        /// </summary>
        public IValidator<VoteCreate> VoteCreateValidator { get; set; }

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

        #region 新建一个投票

        /// <summary>
        ///     新建一个投票。
        /// </summary>
        public async Task<object> Post(VoteCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                VoteCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var currentUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            var existingVote = await VoteRepo.GetVoteAsync(request.ParentId, currentUserId);
            if (existingVote != null)
            {
                if (existingVote.Value == request.Value)
                {
                    return new VoteCreateResponse
                           {
                               Vote = existingVote.MapToVoteDto(currentUserAuth)
                           };
                }
                var newVote = new Vote();
                newVote.PopulateWith(existingVote);
                newVote.Value = request.Value;
                var vote = await VoteRepo.UpdateVoteAsync(existingVote, newVote);
                ResetCache(vote);
                switch (request.ParentType)
                {
                    case "评论":
                        if (vote.Value)
                        {
                            await CommentRepo.IncrementCommentYesVotesCountAsync(vote.ParentId, 1);
                            await CommentRepo.IncrementCommentNoVotesCountAsync(vote.ParentId, -1);
                        }
                        else
                        {
                            await CommentRepo.IncrementCommentYesVotesCountAsync(vote.ParentId, -1);
                            await CommentRepo.IncrementCommentNoVotesCountAsync(vote.ParentId, 1);
                        }
                        break;
                    case "回复":
                        if (vote.Value)
                        {
                            await ReplyRepo.IncrementReplyYesVotesCountAsync(vote.ParentId, 1);
                            await ReplyRepo.IncrementReplyNoVotesCountAsync(vote.ParentId, -1);
                        }
                        else
                        {
                            await ReplyRepo.IncrementReplyYesVotesCountAsync(vote.ParentId, -1);
                            await ReplyRepo.IncrementReplyNoVotesCountAsync(vote.ParentId, 1);
                        }
                        break;
                }
                //await NimClient.PostAsync(new FriendAddRequest
                //                          {
                //                              AccountId = currentUserId.ToString(),
                //                              FriendAccountId = request.ParentId.ToString(),
                //                              Type = 1
                //                          });
                return new VoteCreateResponse
                       {
                           Vote = vote.MapToVoteDto(currentUserAuth)
                       };
            }
            else
            {
                var newVote = new Vote
                              {
                                  ParentType = request.ParentType,
                                  ParentId = request.ParentId,
                                  Value = request.Value,
                                  UserId = currentUserId
                              };
                var vote = await VoteRepo.CreateVoteAsync(newVote);
                ResetCache(vote);
                switch (request.ParentType)
                {
                    case "评论":
                        if (vote.Value)
                        {
                            await CommentRepo.IncrementCommentVotesAndYesVotesCountAsync(vote.ParentId, 1);
                        }
                        else
                        {
                            await CommentRepo.IncrementCommentVotesAndNoVotesCountAsync(vote.ParentId, 1);
                        }
                        break;
                    case "回复":
                        if (vote.Value)
                        {
                            await ReplyRepo.IncrementReplyVotesAndYesVotesCountAsync(vote.ParentId, 1);
                        }
                        else
                        {
                            await ReplyRepo.IncrementReplyVotesAndNoVotesCountAsync(vote.ParentId, 1);
                        }
                        break;
                }
                //await NimClient.PostAsync(new FriendAddRequest
                //                          {
                //                              AccountId = currentUserId.ToString(),
                //                              FriendAccountId = request.ParentId.ToString(),
                //                              Type = 1
                //                          });
                return new VoteCreateResponse
                       {
                           Vote = vote.MapToVoteDto(currentUserAuth)
                       };
            }
        }

        #endregion
    }
}