using System.Collections.Generic;
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
using Sheep.ServiceModel.Views;
using Sheep.ServiceModel.Views.Entities;

namespace Sheep.ServiceInterface.Views
{
    /// <summary>
    ///     根据所有用户列表统计一组查看数量服务接口。
    /// </summary>
    [CompressResponse]
    public class CountViewByAllUsersService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CountViewByAllUsersService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置统计一组查看的校验器。
        /// </summary>
        public IValidator<ViewCountByAllUsers> ViewCountByAllUsersValidator { get; set; }

        /// <summary>
        ///     获取及设置所有用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置查看的存储库。
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

        #region 统计一组查看

        /// <summary>
        ///     统计一组查看。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(ViewCountByAllUsers request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ViewCountByAllUsersValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var userIds = await ((IUserAuthRepositoryExtended) AuthRepo).GetAllUserAuthIdsAsync();
            var viewsCountsMap = (await ViewRepo.GetViewsCountByAllUsersAsync(request.ParentType, request.ParentIdPrefix, request.CreatedSince?.FromUnixTime())).ToDictionary(pair => pair.Key, pair => pair.Value);
            var parentsCountsMap = (await ViewRepo.GetParentsCountByAllUsersAsync(request.ParentType, request.ParentIdPrefix, request.CreatedSince?.FromUnixTime())).ToDictionary(pair => pair.Key, pair => pair.Value);
            var daysCountsMap = (await ViewRepo.GetDaysCountByAllUsersAsync(request.ParentType, request.ParentIdPrefix, request.CreatedSince?.FromUnixTime())).ToDictionary(pair => pair.Key, pair => pair.Value);
            var usersViewCountsDto = userIds.Select(userId => new KeyValuePair<int, ViewCountsDto>(userId, new ViewCountsDto
                                                                                                           {
                                                                                                               ViewsCount = viewsCountsMap.GetValueOrDefault(userId),
                                                                                                               ParentsCount = parentsCountsMap.GetValueOrDefault(userId),
                                                                                                               DaysCount = daysCountsMap.GetValueOrDefault(userId)
                                                                                                           }))
                                            //.Where(kv => kv.Value.ViewsCount > 0 || kv.Value.ParentsCount > 0 || kv.Value.DaysCount > 0)
                                            .ToDictionary(pair => pair.Key, pair => pair.Value);
            return new ViewCountByAllUsersResponse
                   {
                       UsersCounts = usersViewCountsDto
                   };
        }

        #endregion
    }
}