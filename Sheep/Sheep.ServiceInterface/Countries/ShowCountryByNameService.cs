using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Geo;
using Sheep.Model.Geo.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Countries;
using Sheep.ServiceModel.Countries.Entities;

namespace Sheep.ServiceInterface.Countries
{
    /// <summary>
    ///     根据国家名称显示一个国家服务接口。
    /// </summary>
    public class ShowCountryByNameService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowCountryByNameService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据国家名称显示一个国家的校验器。
        /// </summary>
        public IValidator<CountryShowByName> CountryShowByNameValidator { get; set; }

        /// <summary>
        ///     获取及设置国家的存储库。
        /// </summary>
        public ICountryRepository CountryRepo { get; set; }

        #endregion

        #region 根据国家名称显示一个国家

        /// <summary>
        ///     根据国家名称显示一个国家。
        /// </summary>
        [CacheResponse(Duration = 86400, MaxAge = 43200)]
        public async Task<object> Get(CountryShowByName request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                CountryShowByNameValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingCountry = await CountryRepo.GetCountryByNameAsync(request.Name);
            if (existingCountry == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CountryNotFound, request.Name));
            }
            var countryDto = MapToCountryDto(existingCountry);
            return new CountryShowResponse
                   {
                       Country = countryDto
                   };
        }

        #endregion

        #region 转换

        private CountryDto MapToCountryDto(Country country)
        {
            var countryDto = new CountryDto
                             {
                                 Id = country.Id,
                                 Name = country.Name
                             };
            return countryDto;
        }

        #endregion
    }
}