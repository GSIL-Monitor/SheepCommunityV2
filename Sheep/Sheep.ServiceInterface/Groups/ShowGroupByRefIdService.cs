using System;
using System.Collections.Generic;
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
    ///     根据关联的第三方编号显示一个群组服务接口。
    /// </summary>
    public class ShowGroupByRefIdService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowGroupByRefIdService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据关联的第三方编号显示一个群组的校验器。
        /// </summary>
        public IValidator<GroupShowByRefId> GroupShowByRefIdValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 根据关联的第三方编号显示一个群组

        /// <summary>
        ///     根据关联的第三方编号显示一个群组。
        /// </summary>
        [CacheResponse(Duration = 86400, MaxAge = 43200)]
        public async Task<object> Get(GroupShowByRefId request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                GroupShowByRefIdValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingGroup = await GroupRepo.GetGroupByRefIdAsync(request.RefId);
            if (existingGroup == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupNotFound, request.RefId));
            }
            BasicUserDto groupOwnerDto = null;
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var ownerUserAuth = await ((IUserAuthRepositoryExtended) authRepo).GetUserAuthAsync(existingGroup.OwnerId.ToString());
                if (ownerUserAuth != null)
                {
                    groupOwnerDto = MapToBasicUserDto(ownerUserAuth);
                }
            }
            var groupDto = MapToGroupDto(existingGroup, groupOwnerDto);
            return new GroupShowResponse
                   {
                       Group = groupDto
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