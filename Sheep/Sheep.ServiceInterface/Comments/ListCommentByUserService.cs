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
    ///     根据用户列举一组评论信息服务接口。
    /// </summary>
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
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        #endregion

        #region 列举一组用户

        /// <summary>
        ///     列举一组用户。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(CommentListByUser request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                CommentListByUserValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingComments = await CommentRepo.FindCommentsByUserAsync(request.UserId, request.CreatedSince, request.ModifiedSince, request.IsFeatured, "审核通过", request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingComments == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CommentsNotFound));
            }
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingComments.Select(comment => comment.UserId.ToString()).Distinct())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var commentsDto = existingComments.Select(comment => comment.MapToCommentDto(usersMap.GetValueOrDefault(comment.UserId))).ToList();
            return new CommentListResponse
                   {
                       Comments = commentsDto
                   };
        }

        #endregion
    }
}