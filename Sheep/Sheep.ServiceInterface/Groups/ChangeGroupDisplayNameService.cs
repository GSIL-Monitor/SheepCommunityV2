using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Friendship;
using Sheep.Model.Friendship.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Groups;

namespace Sheep.ServiceInterface.Groups
{
    /// <summary>
    ///     更改显示名称服务接口。
    /// </summary>
    public class ChangeGroupDisplayNameService : ChangeGroupService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeGroupDisplayNameService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置更改显示名称的校验器。
        /// </summary>
        public IValidator<GroupChangeDisplayName> GroupChangeDisplayNameValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 更改显示名称

        /// <summary>
        ///     更改显示名称。
        /// </summary>
        public async Task<object> Put(GroupChangeDisplayName request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    GroupChangeDisplayNameValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingGroup = await GroupRepo.GetGroupAsync(request.GroupId);
            if (existingGroup == null)
            {
                existingGroup = new Group
                                {
                                    Id = request.GroupId,
                                    DisplayName = $"Group{request.GroupId}",
                                    CreatedDate = DateTime.UtcNow
                                };
                existingGroup.ModifiedDate = existingGroup.CreatedDate;
            }
            var newGroup = new Group();
            newGroup.PopulateWith(existingGroup);
            newGroup.Meta = existingGroup.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingGroup.Meta);
            newGroup.DisplayName = request.DisplayName?.Replace("\"", "'");
            var group = await GroupRepo.UpdateGroupAsync(existingGroup, newGroup);
            ResetCache(group);
            return new GroupChangeDisplayNameResponse();
        }

        #endregion
    }
}