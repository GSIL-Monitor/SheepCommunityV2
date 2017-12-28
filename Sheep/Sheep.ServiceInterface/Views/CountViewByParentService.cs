using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceModel.Views;

namespace Sheep.ServiceInterface.Views
{
    /// <summary>
    ///     根据上级统计一组阅读数量服务接口。
    /// </summary>
    public class CountViewByParentService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CountViewByParentService));

        #endregion

        #region 属性

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置统计一组阅读的校验器。
        /// </summary>
        public IValidator<ViewCountByParent> ViewCountByParentValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置阅读的存储库。
        /// </summary>
        public IViewRepository ViewRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        #endregion

        #region 统计一组阅读

        /// <summary>
        ///     统计一组阅读。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(ViewCountByParent request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ViewCountByParentValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var viewsCount = await ViewRepo.GetViewsCountByParentAsync(request.ParentId, request.IsMine.HasValue && request.IsMine.Value ? currentUserId : (int?) null, request.CreatedSince);
            var parentsCount = 1;
            var daysCount = await ViewRepo.GetDaysCountByParentAsync(request.ParentId, request.IsMine.HasValue && request.IsMine.Value ? currentUserId : (int?) null, request.CreatedSince);
            return new ViewCountResponse
                   {
                       ViewsCount = viewsCount,
                       ParentsCount = parentsCount,
                       DaysCount = daysCount
                   };
        }

        #endregion
    }
}