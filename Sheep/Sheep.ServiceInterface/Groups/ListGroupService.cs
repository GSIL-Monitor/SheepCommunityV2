using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Corp;
using Sheep.Model.Corp.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Groups;
using Sheep.ServiceModel.Groups.Entities;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceInterface.Groups
{
    /// <summary>
    ///     列举一组群组服务接口。
    /// </summary>
    public class ListGroupService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListGroupService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组群组的校验器。
        /// </summary>
        public IValidator<GroupList> GroupListValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 列举一组群组

        /// <summary>
        ///     列举一组群组。
        /// </summary>
        [CacheResponse(Duration = 600, MaxAge = 300)]
        public async Task<object> Get(GroupList request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                GroupListValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingGroups = await GroupRepo.FindGroupsAsync(request.NameFilter, request.CreatedSince, request.ModifiedSince, request.JoinMode, request.IsPublic, request.AccountStatus, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingGroups == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupsNotFound));
            }
            var groupOwnerDtoMap = new Dictionary<int, BasicUserDto>();
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var ownerUserAuths = await ((IUserAuthRepositoryExtended) authRepo).GetUserAuthsAsync(existingGroups.Select(g => g.OwnerId.ToString()).Distinct().ToArray());
                if (ownerUserAuths != null)
                {
                    groupOwnerDtoMap = ownerUserAuths.ToDictionary(userAuth => userAuth.Id, MapToBasicUserDto);
                }
            }
            var groupsDto = existingGroups.Select(group => MapToGroupDto(group, groupOwnerDtoMap.GetValue(group.OwnerId, () => null))).ToList();
            return new GroupListResponse
                   {
                       Groups = groupsDto
                   };
        }

        #endregion

        #region 转换

        public GroupDto MapToGroupDto(Group group, BasicUserDto groupOwnerDto)
        {
            if (group.Meta == null)
            {
                group.Meta = new Dictionary<string, string>();
            }
            var groupDto = new GroupDto
                           {
                               Id = group.Id,
                               Type = group.Meta.GetValueOrDefault("Type"),
                               DisplayName = group.DisplayName,
                               FullName = group.FullName,
                               FullNameVerified = group.FullNameVerified,
                               Description = group.Description,
                               IconUrl = group.IconUrl,
                               CoverPhotoUrl = group.CoverPhotoUrl,
                               RefId = group.RefId,
                               Country = group.Country,
                               State = group.State,
                               City = group.City,
                               JoinMode = group.JoinMode,
                               IsPublic = group.IsPublic,
                               EnableMessages = group.EnableMessages,
                               AccountStatus = group.AccountStatus,
                               BanReason = group.BanReason,
                               BannedUntil = group.BannedUntil?.ToString("u"),
                               CreatedDate = group.CreatedDate.ToString("u"),
                               ModifiedDate = group.ModifiedDate.ToString("u"),
                               Owner = groupOwnerDto,
                               TotalMembers = 0
                           };
            return groupDto;
        }

        public BasicUserDto MapToBasicUserDto(IUserAuth userAuth)
        {
            if (userAuth.Meta == null)
            {
                userAuth.Meta = new Dictionary<string, string>();
            }
            var userDto = new BasicUserDto
                          {
                              Id = userAuth.Id,
                              UserName = userAuth.UserName,
                              DisplayName = userAuth.DisplayName,
                              AvatarUrl = userAuth.Meta.GetValueOrDefault("AvatarUrl"),
                              Gender = userAuth.Gender
                          };
            return userDto;
        }

        #endregion
    }
}