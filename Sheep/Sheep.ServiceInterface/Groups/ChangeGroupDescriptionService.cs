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
using Sheep.Model.Membership;
using Sheep.Model.Membership.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Groups;

namespace Sheep.ServiceInterface.Groups
{
    /// <summary>
    ///     更改简介服务接口。
    /// </summary>
    public class ChangeGroupDescriptionService : ChangeGroupService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeGroupDescriptionService));

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
        ///     获取及设置更改简介的校验器。
        /// </summary>
        public IValidator<GroupChangeDescription> GroupChangeDescriptionValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 更改简介

        /// <summary>
        ///     更改简介。
        /// </summary>
        public async Task<object> Put(GroupChangeDescription request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    GroupChangeDescriptionValidator.ValidateAndThrow(request, ApplyTo.Put);
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
            newGroup.Description = request.Description?.Replace("\"", "'");
            var group = await GroupRepo.UpdateGroupAsync(existingGroup, newGroup);
            ResetCache(group);
            return new GroupChangeDescriptionResponse();
        }

        #endregion
    }
}