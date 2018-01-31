using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.Model.Friendship;
using Sheep.ServiceInterface.Comments.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Comments;

namespace Sheep.ServiceInterface.Comments
{
    /// <summary>
    ///     根据作者帖子列表列举一组评论信息服务接口。
    /// </summary>
    public class ListCommentByPostsOfAuthorService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListCommentByPostsOfAuthorService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组评论的校验器。
        /// </summary>
        public IValidator<CommentListByPostsOfAuthor> CommentListByPostsOfAuthorValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置关注的存储库。
        /// </summary>
        public IFollowRepository FollowRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

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
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(CommentListByPostsOfAuthor request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    CommentListByPostsOfAuthorValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            List<Comment> existingComments;
            if (request.Following.HasValue && request.Following.Value)
            {
                var existingFollows = await FollowRepo.FindFollowsByFollowerAsync(currentUserId, null, null, null, null, null, null);
                if (existingFollows == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.FollowsNotFound));
                }
                existingComments = await CommentRepo.FindCommentsByPostsOfAuthorAsync(currentUserId, existingFollows.Select(follow => follow.OwnerId).Distinct().ToList(), request.Skip, request.Limit);
            }
            else
            {
                existingComments = await CommentRepo.FindCommentsByPostsOfAuthorAsync(currentUserId, null, request.Skip, request.Limit);
            }
            if (existingComments == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CommentsNotFound));
            }
            var postsMap = (await PostRepo.GetPostsAsync(existingComments.Where(comment => comment.ParentType == "帖子").Select(comment => comment.ParentId).Distinct().ToList())).ToDictionary(post => post.Id, post => post);
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingComments.Select(comment => comment.UserId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var votesMap = (await VoteRepo.GetVotesAsync(existingComments.Select(comment => new Tuple<string, int>(comment.Id, currentUserId)).ToList())).ToDictionary(vote => vote.ParentId, vote => vote);
            var commentsDto = existingComments.Select(comment => comment.MapToCommentDto(comment.ParentType == "帖子" ? postsMap.GetValueOrDefault(comment.ParentId)?.Title : null, comment.ParentType == "帖子" ? postsMap.GetValueOrDefault(comment.ParentId)?.PictureUrl : null, comment.ParentType == "帖子" ? postsMap.GetValueOrDefault(comment.ParentId)?.ContentType : null, usersMap.GetValueOrDefault(comment.UserId), votesMap.GetValueOrDefault(comment.Id)?.Value ?? false, !votesMap.GetValueOrDefault(comment.Id)?.Value ?? false)).ToList();
            return new CommentListResponse
                   {
                       Comments = commentsDto
                   };
        }

        #endregion
    }
}