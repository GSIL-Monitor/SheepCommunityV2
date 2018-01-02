using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceModel.Views;
using Sheep.ServiceModel.Views.Entities;

namespace Sheep.ServiceInterface.Views
{
    /// <summary>
    ///     根据用户统计一组阅读数量服务接口。
    /// </summary>
    public class CountViewByUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CountViewByUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置统计一组阅读的校验器。
        /// </summary>
        public IValidator<ViewCountByUser> ViewCountByUserValidator { get; set; }

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
        [CacheResponse(Duration = 3600)]
        public async Task<object> Get(ViewCountByUser request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ViewCountByUserValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var viewsCount = await ViewRepo.GetViewsCountByUserAsync(request.UserId, request.ParentType, request.ParentIdPrefix, request.CreatedSince);
            var parentsCount = await ViewRepo.GetParentsCountByUserAsync(request.UserId, request.ParentType, request.ParentIdPrefix, request.CreatedSince);
            var daysCount = await ViewRepo.GetDaysCountByUserAsync(request.UserId, request.ParentType, request.ParentIdPrefix, request.CreatedSince);
            return new ViewCountResponse
                   {
                       Counts = new ViewCountsDto
                                {
                                    ViewsCount = viewsCount,
                                    ParentsCount = parentsCount,
                                    DaysCount = daysCount
                                }
                   };
        }

        #endregion
    }
}