using System.Collections.Generic;
using System.Linq;
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
using Sheep.ServiceModel.Views.Entities;

namespace Sheep.ServiceInterface.Views
{
    /// <summary>
    ///     根据用户列表统计一组阅读数量服务接口。
    /// </summary>
    public class CountViewByUsersService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CountViewByUsersService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置统计一组阅读的校验器。
        /// </summary>
        public IValidator<ViewCountByUsers> ViewCountByUsersValidator { get; set; }

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
        public async Task<object> Get(ViewCountByUsers request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ViewCountByUsersValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var viewsCountsMap = (await ViewRepo.GetViewsCountByUsersAsync(request.UserIds, request.ParentType, request.ParentIdPrefix, request.CreatedSince)).ToDictionary(pair => pair.Key, pair => pair.Value);
            var parentsCountsMap = (await ViewRepo.GetParentsCountByUsersAsync(request.UserIds, request.ParentType, request.ParentIdPrefix, request.CreatedSince)).ToDictionary(pair => pair.Key, pair => pair.Value);
            var daysCountsMap = (await ViewRepo.GetDaysCountByUsersAsync(request.UserIds, request.ParentType, request.ParentIdPrefix, request.CreatedSince)).ToDictionary(pair => pair.Key, pair => pair.Value);
            var usersViewCountsDto = request.UserIds.Select(userId => new KeyValuePair<int, ViewCountsDto>(userId, new ViewCountsDto
                                                                                                                   {
                                                                                                                       ViewsCount = viewsCountsMap.GetValueOrDefault(userId),
                                                                                                                       ParentsCount = parentsCountsMap.GetValueOrDefault(userId),
                                                                                                                       DaysCount = daysCountsMap.GetValueOrDefault(userId)
                                                                                                                   }))
                                            .ToDictionary(pair => pair.Key, pair => pair.Value);
            return new ViewCountByUsersResponse
                   {
                       UsersCounts = usersViewCountsDto
                   };
        }

        #endregion
    }
}