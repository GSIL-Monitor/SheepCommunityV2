using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Corp;
using Sheep.Model.Corp.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Groups;

namespace Sheep.ServiceInterface.Groups
{
    /// <summary>
    ///     更改所在地服务接口。
    /// </summary>
    public class ChangeGroupLocationService : ChangeGroupService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeGroupLocationService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置更改所在地的校验器。
        /// </summary>
        public IValidator<GroupChangeLocation> GroupChangeLocationValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 更改所在地

        /// <summary>
        ///     更改所在地。
        /// </summary>
        public async Task<object> Put(GroupChangeLocation request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                GroupChangeLocationValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var existingGroup = await GroupRepo.GetGroupAsync(request.GroupId);
            if (existingGroup == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupNotFound, request.GroupId));
            }
            var newGroup = new Group();
            newGroup.PopulateWith(existingGroup);
            newGroup.Meta = existingGroup.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingGroup.Meta);
            newGroup.Country = request.Country;
            newGroup.State = request.State;
            newGroup.City = request.City;
            var group = await GroupRepo.UpdateGroupAsync(existingGroup, newGroup);
            ResetCache(group);
            return new GroupChangeLocationResponse();
        }

        #endregion
    }
}