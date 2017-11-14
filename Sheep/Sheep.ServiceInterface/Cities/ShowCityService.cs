using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Geo;
using Sheep.ServiceInterface.Cities.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Cities;

namespace Sheep.ServiceInterface.Cities
{
    /// <summary>
    ///     显示一个城市服务接口。
    /// </summary>
    public class ShowCityService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowCityService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个城市的校验器。
        /// </summary>
        public IValidator<CityShow> CityShowValidator { get; set; }

        /// <summary>
        ///     获取及设置城市的存储库。
        /// </summary>
        public ICityRepository CityRepo { get; set; }

        #endregion

        #region 显示一个城市

        /// <summary>
        ///     显示一个城市。
        /// </summary>
        [CacheResponse(Duration = 86400, MaxAge = 43200)]
        public async Task<object> Get(CityShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                CityShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingCity = await CityRepo.GetCityAsync(request.CityId);
            if (existingCity == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CityNotFound, request.CityId));
            }
            var cityDto = existingCity.MapToCityDto();
            return new CityShowResponse
                   {
                       City = cityDto
                   };
        }

        #endregion
    }
}