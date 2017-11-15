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
    ///     根据关联的第三方编号显示一个群组基本信息服务接口。
    /// </summary>
    public class ShowBasicGroupByRefIdService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowBasicGroupByRefIdService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据关联的第三方编号显示一个群组基本信息的校验器。
        /// </summary>
        public IValidator<BasicGroupShowByRefId> BasicGroupShowByRefIdValidator { get; set; }

        /// <summary>
        ///     获取及设置群组基本信息的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 根据关联的第三方编号显示一个群组基本信息

        /// <summary>
        ///     根据关联的第三方编号显示一个群组基本信息。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(BasicGroupShowByRefId request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BasicGroupShowByRefIdValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingGroup = await GroupRepo.GetGroupByRefIdAsync(request.RefId);
            if (existingGroup == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupNotFound, request.RefId));
            }
            var groupDto = existingGroup.MapToBasicGroupDto();
            return new BasicGroupShowResponse
                   {
                       Group = groupDto
                   };
        }

        #endregion
    }
}