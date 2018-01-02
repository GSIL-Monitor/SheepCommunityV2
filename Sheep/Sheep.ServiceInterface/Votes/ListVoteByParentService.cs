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
using Sheep.ServiceInterface.Votes.Mappers;
using Sheep.ServiceModel.Votes;

namespace Sheep.ServiceInterface.Votes
{
    /// <summary>
    ///     根据上级列举一组投票信息服务接口。
    /// </summary>
    public class ListVoteByParentService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListVoteByParentService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组投票的校验器。
        /// </summary>
        public IValidator<VoteListByParent> VoteListByParentValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置投票的存储库。
        /// </summary>
        public IVoteRepository VoteRepo { get; set; }

        #endregion

        #region 列举一组投票

        /// <summary>
        ///     列举一组投票。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(VoteListByParent request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    VoteListByParentValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingVotes = await VoteRepo.FindVotesByParentAsync(request.ParentId, request.CreatedSince, request.ModifiedSince, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingVotes == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VotesNotFound));
            }
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingVotes.Select(vote => vote.UserId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var votesDto = existingVotes.Select(vote => vote.MapToVoteDto(usersMap.GetValueOrDefault(vote.UserId))).ToList();
            return new VoteListResponse
                   {
                       Votes = votesDto
                   };
        }

        #endregion
    }
}