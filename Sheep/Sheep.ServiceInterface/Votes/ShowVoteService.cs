using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Votes.Mappers;
using Sheep.ServiceModel.Votes;

namespace Sheep.ServiceInterface.Votes
{
    /// <summary>
    ///     显示一个投票服务接口。
    /// </summary>
    public class ShowVoteService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowVoteService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个投票的校验器。
        /// </summary>
        public IValidator<VoteShow> VoteShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置投票的存储库。
        /// </summary>
        public IVoteRepository VoteRepo { get; set; }

        #endregion

        #region 显示一个投票

        /// <summary>
        ///     显示一个投票。
        /// </summary>
        public async Task<object> Get(VoteShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    VoteShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.UserId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserId));
            }
            var existingVote = await VoteRepo.GetVoteAsync(request.ParentId, request.UserId);
            if (existingVote == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VoteNotFound, request.ParentId));
            }
            var voteDto = existingVote.MapToVoteDto(user);
            return new VoteShowResponse
                   {
                       Vote = voteDto
                   };
        }

        #endregion
    }
}