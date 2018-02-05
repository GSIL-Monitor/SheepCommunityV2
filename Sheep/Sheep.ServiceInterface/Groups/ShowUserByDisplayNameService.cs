using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Friendship;
using Sheep.ServiceInterface.Groups.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Groups;

namespace Sheep.ServiceInterface.Groups
{
    /// <summary>
    ///     根据显示名称显示一个群组服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowGroupByDisplayNameService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowGroupByDisplayNameService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据显示名称显示一个群组的校验器。
        /// </summary>
        public IValidator<GroupShowByDisplayName> GroupShowByDisplayNameValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        #endregion

        #region 根据显示名称显示一个群组

        /// <summary>
        ///     根据显示名称显示一个群组。
        /// </summary>
        [CacheResponse(Duration = 3600)]
        public async Task<object> Get(GroupShowByDisplayName request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    GroupShowByDisplayNameValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingGroup = await GroupRepo.GetGroupByDisplayNameAsync(request.DisplayName);
            if (existingGroup == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupNotFound, request.DisplayName));
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