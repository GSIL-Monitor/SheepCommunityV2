using System;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Comments.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Comments;

namespace Sheep.ServiceInterface.Comments
{
    /// <summary>
    ///     根据上级列举一组评论信息服务接口。
    /// </summary>
    public class ListCommentByParentService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListCommentByParentService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组评论的校验器。
        /// </summary>
        public IValidator<CommentListByParent> CommentListByParentValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        /// <summary>
        ///     获取及设置投票的存储库。
        /// </summary>
        public IVoteRepository VoteRepo { get; set; }

        #endregion

        #region 列举一组评论

        /// <summary>
        ///     列举一组评论。
        /// </summary>
        //[CacheResponse(Duration = 600)]
        public async Task<object> Get(CommentListByParent request)
        {
            if (request.IsMine.HasValue && request.IsMine.Value && !IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                CommentListByParentValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var existingComments = await CommentRepo.FindCommentsByParentAsync(request.ParentId, request.IsMine.HasValue && request.IsMine.Value ? currentUserId : (int?) null, request.CreatedSince, request.ModifiedSince, request.IsFeatured, "审核通过", request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingComments == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CommentsNotFound));
            }
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingComments.Select(comment => comment.UserId.ToString()).Distinct())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            //var currentUserId = GetSession().UserAuthId.ToInt(0);
            var votesMap = (await VoteRepo.GetVotesAsync(existingComments.Select(comment => new Tuple<string, int>(comment.Id, currentUserId)))).ToDictionary(vote => vote.ParentId, vote => vote);
            var commentsDto = existingComments.Select(comment => comment.MapToCommentDto(usersMap.GetValueOrDefault(comment.UserId), votesMap.GetValueOrDefault(comment.Id)?.Value ?? false, !votesMap.GetValueOrDefault(comment.Id)?.Value ?? false)).ToList();
            return new CommentListResponse
                   {
                       Comments = commentsDto
                   };
        }

        #endregion
    }
}