using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Job.ServiceInterface.Properties;
using Sheep.Job.ServiceModel.Users;
using Sheep.Model.Content;

namespace Sheep.Job.ServiceInterface.Users
{
    /// <summary>
    ///     计算一组用户声望服务接口。
    /// </summary>
    public class CalculateUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CalculateUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置计算一组用户声望的校验器。
        /// </summary>
        public IValidator<UserCalculate> UserCalculateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

        /// <summary>
        ///     获取及设置收藏的存储库。
        /// </summary>
        public IBookmarkRepository BookmarkRepo { get; set; }

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

        #region 计算一组用户声望

        /// <summary>
        ///     计算一组用户声望。
        /// </summary>
        public async Task<object> Put(UserCalculate request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    UserCalculateValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingUserAuths = await ((IUserAuthRepositoryExtended) AuthRepo).FindUserAuthsAsync(request.UserNameFilter, request.NameFilter, request.CreatedSince, request.ModifiedSince, request.LockedSince, null, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingUserAuths == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UsersNotFound));
            }
            foreach (var existingUser in existingUserAuths)
            {
                var postsScore = await PostRepo.CalculateAuthorPostsScoreAsync(existingUser.Id, null, null, DateTime.UtcNow.Date.AddDays(-90), null, null, true, null, "审核通过");
                var likesScore = await LikeRepo.CalculateUserLikesScoreAsync(existingUser.Id, null, DateTime.UtcNow.Date.AddDays(-90));
                var bookmarksScore = await BookmarkRepo.CalculateUserBookmarksScoreAsync(existingUser.Id, null, DateTime.UtcNow.Date.AddDays(-90));
                var commentsScore = await CommentRepo.CalculateUserCommentsScoreAsync(existingUser.Id, null, DateTime.UtcNow.Date.AddDays(-90), null, null, "审核通过");
                var repliesScore = await ReplyRepo.CalculateUserRepliesScoreAsync(existingUser.Id, null, DateTime.UtcNow.Date.AddDays(-180), null, "审核通过");
                var votesScore = await VoteRepo.CalculateUserVotesScoreAsync(existingUser.Id, null, DateTime.UtcNow.Date.AddDays(-90), null);
                await ((IUserAuthRepositoryExtended) AuthRepo).UpdateUserAuthReputationAsync(existingUser.Id.ToString(), postsScore + likesScore + bookmarksScore + commentsScore + repliesScore + votesScore);
            }
            return new UserCalculateResponse();
        }

        #endregion
    }
}