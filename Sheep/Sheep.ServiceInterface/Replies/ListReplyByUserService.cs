using System;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Replies.Mappers;
using Sheep.ServiceModel.Replies;

namespace Sheep.ServiceInterface.Replies
{
    /// <summary>
    ///     根据用户列举一组回复信息服务接口。
    /// </summary>
    public class ListReplyByUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListReplyByUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组回复的校验器。
        /// </summary>
        public IValidator<ReplyListByUser> ReplyListByUserValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置回复的存储库。
        /// </summary>
        public IReplyRepository ReplyRepo { get; set; }

        /// <summary>
        ///     获取及设置投票的存储库。
        /// </summary>
        public IVoteRepository VoteRepo { get; set; }

        #endregion

        #region 列举一组回复

        /// <summary>
        ///     列举一组回复。
        /// </summary>
        //[CacheResponse(Duration = 600)]
        public async Task<object> Get(ReplyListByUser request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ReplyListByUserValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingReplies = await ReplyRepo.FindRepliesByUserAsync(request.UserId, request.ParentType, request.CreatedSince, request.ModifiedSince, "审核通过", request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingReplies == null)
            {
                throw HttpError.NotFound(string.Format(Resources.RepliesNotFound));
            }
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingReplies.Select(reply => reply.UserId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var votesMap = (await VoteRepo.GetVotesAsync(existingReplies.Select(reply => new Tuple<string, int>(reply.Id, currentUserId)).ToList())).ToDictionary(vote => vote.ParentId, vote => vote);
            var repliesDto = existingReplies.Select(reply => reply.MapToReplyDto(usersMap.GetValueOrDefault(reply.UserId), votesMap.GetValueOrDefault(reply.Id)?.Value ?? false, !votesMap.GetValueOrDefault(reply.Id)?.Value ?? false)).ToList();
            return new ReplyListResponse
                   {
                       Replies = repliesDto
                   };
        }

        #endregion
    }
}