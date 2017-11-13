using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Geo;
using Sheep.Model.Geo.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Cities;
using Sheep.ServiceModel.Cities.Entities;

namespace Sheep.ServiceInterface.Cities
{
    /// <summary>
    ///     列举一组城市服务接口。
    /// </summary>
    public class ListCityService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListCityService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组城市的校验器。
        /// </summary>
        public IValidator<CityList> CityListValidator { get; set; }

        /// <summary>
        ///     获取及设置城市的存储库。
        /// </summary>
        public ICityRepository CityRepo { get; set; }

        #endregion

        #region 列举一组城市

        /// <summary>
        ///     列举一组城市。
        /// </summary>
        [CacheResponse(Duration = 600, MaxAge = 300)]
        public async Task<object> Get(CityList request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                CityListValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            List<City> existingCities;
            if (request.NameFilter.IsNullOrEmpty())
            {
                existingCities = await CityRepo.GetCitiesInStateAsync(request.StateId);
            }
            else
            {
                existingCities = await CityRepo.FindCitiesInStateByNameAsync(request.StateId, request.NameFilter);
            }
            if (existingCities == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CitiesNotFound));
            }
            var citiesDto = existingCities.Select(MapToCityDto).ToList();
            return new CityListResponse
                   {
                       Cities = citiesDto
                   };
        }

        #endregion

        #region 转换

        private CityDto MapToCityDto(City city)
        {
            var cityDto = new CityDto
                          {
                              Id = city.Id,
                              Name = city.Name
                          };
            return cityDto;
        }

        #endregion
    }
}