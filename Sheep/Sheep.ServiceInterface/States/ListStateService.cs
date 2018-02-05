using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Geo;
using Sheep.Model.Geo.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.States.Mappers;
using Sheep.ServiceModel.States;

namespace Sheep.ServiceInterface.States
{
    /// <summary>
    ///     列举一组省份服务接口。
    /// </summary>
    [CompressResponse]
    public class ListStateService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListStateService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组省份的校验器。
        /// </summary>
        public IValidator<StateList> StateListValidator { get; set; }

        /// <summary>
        ///     获取及设置省份的存储库。
        /// </summary>
        public IStateRepository StateRepo { get; set; }

        #endregion

        #region 列举一组省份

        /// <summary>
        ///     列举一组省份。
        /// </summary>
        [CacheResponse(Duration = 31536000, MaxAge = 86400)]
        public async Task<object> Get(StateList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    StateListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            List<State> existingStates;
            if (request.NameFilter.IsNullOrEmpty())
            {
                existingStates = await StateRepo.GetStatesInCountryAsync(request.CountryId);
            }
            else
            {
                existingStates = await StateRepo.FindStatesInCountryByNameAsync(request.CountryId, request.NameFilter);
            }
            if (existingStates == null)
            {
                throw HttpError.NotFound(string.Format(Resources.StatesNotFound));
            }
            var statesDto = existingStates.Select(state => state.MapToStateDto()).ToList();
            return new StateListResponse
                   {
                       States = statesDto
                   };
        }

        #endregion
    }
}