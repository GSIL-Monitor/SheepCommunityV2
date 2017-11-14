using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Geo;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.States.Mappers;
using Sheep.ServiceModel.States;

namespace Sheep.ServiceInterface.States
{
    /// <summary>
    ///     根据省份名称显示一个省份服务接口。
    /// </summary>
    public class ShowStateByNameService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowStateByNameService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据省份名称显示一个省份的校验器。
        /// </summary>
        public IValidator<StateShowByName> StateShowByNameValidator { get; set; }

        /// <summary>
        ///     获取及设置省份的存储库。
        /// </summary>
        public IStateRepository StateRepo { get; set; }

        #endregion

        #region 根据省份名称显示一个省份

        /// <summary>
        ///     根据省份名称显示一个省份。
        /// </summary>
        [CacheResponse(Duration = 86400, MaxAge = 43200)]
        public async Task<object> Get(StateShowByName request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                StateShowByNameValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingState = await StateRepo.GetStateByNameAsync(request.CountryId, request.Name);
            if (existingState == null)
            {
                throw HttpError.NotFound(string.Format(Resources.StateNotFound, request.Name));
            }
            var stateDto = existingState.MapToStateDto();
            return new StateShowResponse
                   {
                       State = stateDto
                   };
        }

        #endregion
    }
}