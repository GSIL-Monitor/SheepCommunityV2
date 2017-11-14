using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Corp;
using Sheep.Model.Corp.Entities;
using Sheep.ServiceInterface.Groups.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Groups;

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
            var group = await GroupRepo.CreateGroupAsync(newGroup);
            return new GroupCreateResponse
                   {
                       Group = group.MapToGroupDto()
                   };
        }

        #endregion
    }
}