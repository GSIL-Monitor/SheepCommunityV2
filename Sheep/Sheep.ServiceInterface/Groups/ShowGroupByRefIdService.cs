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
        [CacheResponse(Duration = 600)]
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
            var groupDto = existingGroup.MapToGroupDto();
            return new GroupShowResponse
                   {
                       Group = groupDto
                   };
        }

        #endregion
    }
}