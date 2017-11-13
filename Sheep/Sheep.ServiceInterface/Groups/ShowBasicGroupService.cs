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
    ///     显示一个群组基本信息服务接口。
    /// </summary>
    public class ShowBasicGroupService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowBasicGroupService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个群组基本信息的校验器。
        /// </summary>
        public IValidator<BasicGroupShow> BasicGroupShowValidator { get; set; }

        /// <summary>
        ///     获取及设置群组基本信息的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 显示一个群组基本信息

        /// <summary>
        ///     显示一个群组基本信息。
        /// </summary>
        [CacheResponse(Duration = 600, MaxAge = 300)]
        public async Task<object> Get(BasicGroupShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BasicGroupShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingGroup = await GroupRepo.GetGroupAsync(request.GroupId);
            if (existingGroup == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupNotFound, request.GroupId));
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
            var groupDto = MapToBasicGroupDto(existingGroup, groupOwnerDto);
            return new BasicGroupShowResponse
                   {
                       Group = groupDto
                   };
        }

        #endregion

        #region 转换

        public BasicGroupDto MapToBasicGroupDto(Group group, BasicUserDto groupOwnerDto)
        {
            if (group.Meta == null)
            {
                group.Meta = new Dictionary<string, string>();
            }
            var groupDto = new BasicGroupDto
                           {
                               Id = group.Id,
                               DisplayName = group.DisplayName,
                               IconUrl = group.IconUrl,
                               RefId = group.RefId,
                               JoinMode = group.JoinMode
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