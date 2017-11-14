using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Geo;
using Sheep.ServiceInterface.Countries.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Countries;

namespace Sheep.ServiceInterface.Countries
{
    /// <summary>
    ///     显示一个国家服务接口。
    /// </summary>
    public class ShowCountryService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowCountryService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个国家的校验器。
        /// </summary>
        public IValidator<CountryShow> CountryShowValidator { get; set; }

        /// <summary>
        ///     获取及设置国家的存储库。
        /// </summary>
        public ICountryRepository CountryRepo { get; set; }

        #endregion

        #region 显示一个国家

        /// <summary>
        ///     显示一个国家。
        /// </summary>
        [CacheResponse(Duration = 86400, MaxAge = 43200)]
        public async Task<object> Get(CountryShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                CountryShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingCountry = await CountryRepo.GetCountryAsync(request.CountryId);
            if (existingCountry == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CountryNotFound, request.CountryId));
            }
            var countryDto = existingCountry.MapToCountryDto();
            return new CountryShowResponse
                   {
                       Country = countryDto
                   };
        }

        #endregion
    }
}