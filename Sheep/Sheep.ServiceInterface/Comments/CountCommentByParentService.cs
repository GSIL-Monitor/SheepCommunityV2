using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Model.Content;
using Sheep.ServiceModel.Comments;
using Sheep.ServiceModel.Comments.Entities;

namespace Sheep.ServiceInterface.Comments
{
    /// <summary>
    ///     根据上级统计一组评论数量服务接口。
    /// </summary>
    public class CountCommentByParentService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CountCommentByParentService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置统计一组评论的校验器。
        /// </summary>
        public IValidator<CommentCountByParent> CommentCountByParentValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        #endregion

        #region 统计一组评论

        /// <summary>
        ///     统计一组评论。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(CommentCountByParent request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    CommentCountByParentValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var commentsCount = await CommentRepo.GetCommentsCountByParentAsync(request.ParentId, request.IsMine.HasValue && request.IsMine.Value ? currentUserId : (int?) null, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.IsFeatured, "审核通过");
            return new CommentCountResponse
                   {
                       Counts = new CommentCountsDto
                                {
                                    CommentsCount = commentsCount
                                }
                   };
        }

        #endregion
    }
}