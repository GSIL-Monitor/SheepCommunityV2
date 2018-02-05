using System;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Comments.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Comments;

namespace Sheep.ServiceInterface.Comments
{
    /// <summary>
    ///     根据用户列举一组评论信息服务接口。
    /// </summary>
    [CompressResponse]
    public class ListCommentByUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListCommentByUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组评论的校验器。
        /// </summary>
        public IValidator<CommentListByUser> CommentListByUserValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

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
        public async Task<object> Get(CommentListByUser request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    CommentListByUserValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingComments = await CommentRepo.FindCommentsByUserAsync(request.UserId, request.ParentType, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.IsFeatured, "审核通过", request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingComments == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CommentsNotFound));
            }
            var postsMap = (await PostRepo.GetPostsAsync(existingComments.Where(comment => comment.ParentType == "帖子").Select(comment => comment.ParentId).Distinct().ToList())).ToDictionary(post => post.Id, post => post);
            var chaptersMap = (await ChapterRepo.GetChaptersAsync(existingComments.Where(comment => comment.ParentType == "章").Select(comment => comment.ParentId).Distinct().ToList())).ToDictionary(chapter => chapter.Id, chapter => chapter);
            var paragraphsMap = (await ParagraphRepo.GetParagraphsAsync(existingComments.Where(comment => comment.ParentType == "节").Select(comment => comment.ParentId).Distinct().ToList())).ToDictionary(paragraph => paragraph.Id, paragraph => paragraph);
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingComments.Select(comment => comment.UserId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var votesMap = (await VoteRepo.GetVotesAsync(existingComments.Select(comment => new Tuple<string, int>(comment.Id, currentUserId)).ToList())).ToDictionary(vote => vote.ParentId, vote => vote);
            var commentsDto = existingComments.Select(comment => comment.MapToCommentDto(comment.ParentType == "帖子" ? postsMap.GetValueOrDefault(comment.ParentId)?.Title : (comment.ParentType == "章" ? chaptersMap.GetValueOrDefault(comment.ParentId)?.Title : paragraphsMap.GetValueOrDefault(comment.ParentId)?.Content), comment.ParentType == "帖子" ? postsMap.GetValueOrDefault(comment.ParentId)?.PictureUrl : null, comment.ParentType == "帖子" ? postsMap.GetValueOrDefault(comment.ParentId)?.ContentType : null, usersMap.GetValueOrDefault(comment.UserId), votesMap.GetValueOrDefault(comment.Id)?.Value ?? false, !votesMap.GetValueOrDefault(comment.Id)?.Value ?? false)).ToList();
            return new CommentListResponse
                   {
                       Comments = commentsDto
                   };
        }

        #endregion
    }
}