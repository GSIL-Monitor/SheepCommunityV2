using System.Collections.Generic;
using System.Linq;
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
    ///     根据上级列表统计一组评论数量服务接口。
    /// </summary>
    public class CountCommentByParentsService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CountCommentByParentsService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置统计一组评论的校验器。
        /// </summary>
        public IValidator<CommentCountByParents> CommentCountByParentsValidator { get; set; }

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
        public async Task<object> Get(CommentCountByParents request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    CommentCountByParentsValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var commentsCountsMap = (await CommentRepo.GetCommentsCountByParentsAsync(request.ParentIds, request.IsMine.HasValue && request.IsMine.Value ? currentUserId : (int?) null, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.IsFeatured, "审核通过")).ToDictionary(pair => pair.Key, pair => pair.Value);
            var parentsCommentCountsDto = request.ParentIds.Select(parentId => new KeyValuePair<string, CommentCountsDto>(parentId, new CommentCountsDto
                                                                                                                                    {
                                                                                                                                        CommentsCount = commentsCountsMap.GetValueOrDefault(parentId)
                                                                                                                                    }))
                                                 .ToDictionary(pair => pair.Key, pair => pair.Value);
            return new CommentCountByParentsResponse
                   {
                       ParentsCounts = parentsCommentCountsDto
                   };
        }

        #endregion
    }
}