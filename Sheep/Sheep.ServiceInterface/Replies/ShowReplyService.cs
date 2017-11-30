using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Replies.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Replies;

namespace Sheep.ServiceInterface.Replies
{
    /// <summary>
    ///     显示一个回复服务接口。
    /// </summary>
    public class ShowReplyService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowReplyService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个回复的校验器。
        /// </summary>
        public IValidator<ReplyShow> ReplyShowValidator { get; set; }

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

        #region 显示一个回复

        /// <summary>
        ///     显示一个回复。
        /// </summary>
        public async Task<object> Get(ReplyShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ReplyShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingReply = await ReplyRepo.GetReplyAsync(request.ReplyId);
            if (existingReply == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ReplyNotFound, request.ReplyId));
            }
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingReply.UserId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, existingReply.UserId));
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var vote = await VoteRepo.GetVoteAsync(existingReply.Id, currentUserId);
            var replyDto = existingReply.MapToReplyDto(user, vote?.Value ?? false, !vote?.Value ?? false);
            return new ReplyShowResponse
                   {
                       Reply = replyDto
                   };
        }

        #endregion
    }
}