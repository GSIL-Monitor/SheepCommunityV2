﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Geo;
using Sheep.Model.Geo.Entities;
using Sheep.ServiceInterface.Countries.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Countries;

namespace Sheep.ServiceInterface.Countries
{
    /// <summary>
    ///     列举一组国家服务接口。
    /// </summary>
    [CompressResponse]
    public class ListCountryService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListCountryService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组国家的校验器。
        /// </summary>
        public IValidator<CountryList> CountryListValidator { get; set; }

        /// <summary>
        ///     获取及设置国家的存储库。
        /// </summary>
        public ICountryRepository CountryRepo { get; set; }

        #endregion

        #region 列举一组国家

        /// <summary>
        ///     列举一组国家。
        /// </summary>
        [CacheResponse(Duration = 31536000, MaxAge = 86400)]
        public async Task<object> Get(CountryList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    CountryListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            List<Country> existingCountries;
            if (request.NameFilter.IsNullOrEmpty())
            {
                existingCountries = await CountryRepo.GetCountriesAsync();
            }
            else
            {
                existingCountries = await CountryRepo.FindCountriesByNameAsync(request.NameFilter);
            }
            if (existingCountries == null)
            {
                throw HttpError.NotFound(string.Format(Resources.CountriesNotFound));
            }
            var countriesDto = existingCountries.Select(country => country.MapToCountryDto()).ToList();
            return new CountryListResponse
                   {
                       Countries = countriesDto
                   };
        }

        #endregion
    }
}