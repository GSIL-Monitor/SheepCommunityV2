using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Job.ServiceInterface.Properties;
using Sheep.Job.ServiceModel.Comments;
using Sheep.Model.Content;

namespace Sheep.Job.ServiceInterface.Comments
{
    /// <summary>
    ///     计算一组评论分数服务接口。
    /// </summary>
    public class CalculateCommentService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CalculateCommentService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置计算一组评论分数的校验器。
        /// </summary>
        public IValidator<CommentCalculate> CommentCalculateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        #endregion

        #region 计算一组评论分数

        /// <summary>
        ///     计算一组评论分数。
        /// </summary>
        public async Task<object> Put(CommentCalculate request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    CommentCalculateValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingComments = await CommentRepo.FindCommentsAsync(request.ParentType, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.IsFeatured, "审核通过", request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingComments == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CommentsNotFound));
            }
            foreach (var existingComment in existingComments)
            {
                await CommentRepo.UpdateCommentContentQualityAsync(existingComment.Id, CommentRepo.CalculateCommentContentQuality(existingComment));
            }
            return new CommentCalculateResponse();
        }

        #endregion
    }
}