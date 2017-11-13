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
    ///     创建群组服务接口。
    /// </summary>
    public class CreateGroupService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateGroupService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置创建群组的校验器。
        /// </summary>
        public IValidator<GroupCreate> GroupCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 创建群组

        /// <summary>
        ///     创建群组。
        /// </summary>
        public async Task<object> Post(GroupCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                GroupCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var newGroup = MapToGroup(request);
            var group = await GroupRepo.CreateGroupAsync(newGroup, GetSession().UserAuthId.ToInt(0));
            BasicUserDto groupOwnerDto = null;
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var ownerUserAuth = await ((IUserAuthRepositoryExtended) authRepo).GetUserAuthAsync(group.OwnerId.ToString());
                if (ownerUserAuth != null)
                {
                    groupOwnerDto = MapToBasicUserDto(ownerUserAuth);
                }
            }
            return new GroupCreateResponse
                   {
                       Group = MapToGroupDto(group, groupOwnerDto)
                   };
        }

        #endregion

        #region 转换

        /// <summary>
        ///     将注册身份的请求转换成群组身份。
        /// </summary>
        public Group MapToGroup(GroupCreate request)
        {
            var newGroup = new Group
                           {
                               Meta = new Dictionary<string, string>(),
                               DisplayName = request.DisplayName,
                               Description = request.Description,
                               RefId = request.RefId,
                               JoinMode = !request.JoinMode.IsNullOrEmpty() ? request.JoinMode : "Direct",
                               IsPublic = request.IsPublic.HasValue && request.IsPublic.Value,
                               EnableMessages = request.EnableMessages.HasValue && request.EnableMessages.Value
                           };
            return newGroup;
        }

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