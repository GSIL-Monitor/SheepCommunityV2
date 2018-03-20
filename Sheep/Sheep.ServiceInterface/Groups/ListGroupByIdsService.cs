﻿using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Model.Membership;
using Sheep.ServiceInterface.Groups.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Groups;

namespace Sheep.ServiceInterface.Groups
{
    /// <summary>
    ///     根据编号列表列举一组群组服务接口。
    /// </summary>
    [CompressResponse]
    public class ListGroupByIdsService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListGroupByIdsService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组群组的校验器。
        /// </summary>
        public IValidator<GroupListByIds> GroupListByIdsValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 列举一组群组

        /// <summary>
        ///     列举一组群组。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(GroupListByIds request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    GroupListByIdsValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingGroups = await GroupRepo.FindGroupsAsync(request.GroupIds.Select(groupId => groupId.ToString()).ToList(), request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingGroups == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupsNotFound));
            }
            var groupsDto = existingGroups.Select(groupGroup => groupGroup.MapToGroupDto()).ToList();
            return new GroupListResponse
                   {
                       Groups = groupsDto
                   };
        }

        #endregion
    }
}