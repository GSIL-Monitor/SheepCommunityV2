using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Membership;
using Sheep.ServiceInterface.Groups.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Groups;

namespace Sheep.ServiceInterface.Groups
{
    /// <summary>
    ///     根据编号列表列举一组群组排行服务接口。
    /// </summary>
    [CompressResponse]
    public class ListGroupRankByIdsService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListGroupRankByIdsService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据编号列表列举一组群组排行的校验器。
        /// </summary>
        public IValidator<GroupRankListByIds> GroupRankListByIdsValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        /// <summary>
        ///     获取及设置群组排行的存储库。
        /// </summary>
        public IGroupRankRepository GroupRankRepo { get; set; }

        #endregion

        #region 列举一组群组排行

        /// <summary>
        ///     根据编号列表列举一组群组排行。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(GroupRankListByIds request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    GroupRankListByIdsValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingGroupRanks = await GroupRankRepo.FindGroupRanksAsync(request.GroupIds, null, null, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingGroupRanks == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupRanksNotFound));
            }
            var groupsMap = (await GroupRepo.FindGroupsAsync(existingGroupRanks.Select(groupRank => groupRank.Id).ToList(), null, null, null, null, null, null)).ToDictionary(group => group.Id, group => group);
            var groupRanksDto = existingGroupRanks.Select(groupRank => groupRank.MapToGroupRankDto(groupsMap.GetValueOrDefault(groupRank.Id))).ToList();
            return new GroupRankListResponse
                   {
                       GroupRanks = groupRanksDto
                   };
        }

        #endregion
    }
}