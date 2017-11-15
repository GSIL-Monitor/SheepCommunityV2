using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Corp;
using Sheep.ServiceInterface.Groups.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Groups;

namespace Sheep.ServiceInterface.Groups
{
    /// <summary>
    ///     显示一个群组服务接口。
    /// </summary>
    public class ShowGroupService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowGroupService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个群组的校验器。
        /// </summary>
        public IValidator<GroupShow> GroupShowValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 显示一个群组

        /// <summary>
        ///     显示一个群组。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(GroupShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                GroupShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingGroup = await GroupRepo.GetGroupAsync(request.GroupId);
            if (existingGroup == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupNotFound, request.GroupId));
            }
            var groupDto = existingGroup.MapToGroupDto();
            return new GroupShowResponse
                   {
                       Group = groupDto
                   };
        }

        #endregion
    }
}