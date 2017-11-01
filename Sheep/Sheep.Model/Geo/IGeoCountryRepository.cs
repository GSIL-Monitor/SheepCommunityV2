using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Geo.Entities;

namespace Sheep.Model.Geo
{
    /// <summary>
    ///     国家的存储库的接口定义。
    /// </summary>
    public interface IGeoCountryRepository
    {
        #region 获取

        /// <summary>
        ///     获取国家。
        /// </summary>
        /// <param name="countryId">国家编号。</param>
        /// <returns>国家。</returns>
        GeoCountry GetCountry(string countryId);

        /// <summary>
        ///     异步获取国家。
        /// </summary>
        /// <param name="countryId">国家编号。</param>
        /// <returns>国家。</returns>
        Task<GeoCountry> GetCountryAsync(string countryId);

        /// <summary>
        ///     根据名称获取国家。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <returns>国家。</returns>
        GeoCountry GetCountryByName(string name);

        /// <summary>
        ///     异步根据名称获取国家。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <returns>国家。</returns>
        Task<GeoCountry> GetCountryByNameAsync(string name);

        /// <summary>
        ///     获取所有国家。
        /// </summary>
        /// <returns>国家列表。</returns>
        List<GeoCountry> GetCountries();

        /// <summary>
        ///     异步获取所有国家。
        /// </summary>
        /// <returns>国家列表。</returns>
        Task<List<GeoCountry>> GetCountriesAsync();

        /// <summary>
        ///     根据名称查找国家。
        /// </summary>
        /// <param name="nameFilter">名称过滤表达式。</param>
        /// <returns>国家列表。</returns>
        List<GeoCountry> FindCountriesByName(string nameFilter);

        /// <summary>
        ///     异步根据名称查找国家。
        /// </summary>
        /// <param name="nameFilter">名称过滤表达式。</param>
        /// <returns>国家列表。</returns>
        Task<List<GeoCountry>> FindCountriesByNameAsync(string nameFilter);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的国家。
        /// </summary>
        /// <param name="newCountry">新的国家。</param>
        /// <returns>创建后的国家。</returns>
        GeoCountry CreateCountry(GeoCountry newCountry);

        /// <summary>
        ///     异步创建一个新的国家。
        /// </summary>
        /// <param name="newCountry">新的国家。</param>
        /// <returns>创建后的国家。</returns>
        Task<GeoCountry> CreateCountryAsync(GeoCountry newCountry);

        /// <summary>
        ///     删除一个国家。
        /// </summary>
        /// <param name="countryId">国家编号。</param>
        void DeleteCountry(string countryId);

        /// <summary>
        ///     异步删除一个国家。
        /// </summary>
        /// <param name="countryId">国家编号。</param>
        Task DeleteCountryAsync(string countryId);

        #endregion
    }
}