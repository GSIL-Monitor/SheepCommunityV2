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
    ///     显示一个评论服务接口。
    /// </summary>
    public class ShowCommentService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowCommentService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个评论的校验器。
        /// </summary>
        public IValidator<CommentShow> CommentShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        #endregion

        #region 显示一个评论

        /// <summary>
        ///     显示一个评论。
        /// </summary>
        public async Task<object> Get(CommentShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                CommentShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingComment = await CommentRepo.GetCommentAsync(request.CommentId);
            if (existingComment == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CommentNotFound, request.CommentId));
            }
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingComment.UserId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, existingComment.UserId));
            }
            var commentDto = existingComment.MapToCommentDto(user);
            return new CommentShowResponse
                   {
                       Comment = commentDto
                   };
        }

        #endregion
    }
}