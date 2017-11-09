using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Geo.Entities;

namespace Sheep.Model.Geo
{
    /// <summary>
    ///     城市的存储库的接口定义。
    /// </summary>
    public interface ICityRepository
    {
        #region 获取

        /// <summary>
        ///     获取城市。
        /// </summary>
        /// <param name="cityId">城市编号。</param>
        /// <returns>城市。</returns>
        City GetCity(string cityId);

        /// <summary>
        ///     异步获取城市。
        /// </summary>
        /// <param name="cityId">城市编号。</param>
        /// <returns>城市。</returns>
        Task<City> GetCityAsync(string cityId);

        /// <summary>
        ///     根据名称获取城市。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <param name="name">名称。</param>
        /// <returns>城市。</returns>
        City GetCityByName(string stateId, string name);

        /// <summary>
        ///     异步根据名称获取城市。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <param name="name">名称。</param>
        /// <returns>城市。</returns>
        Task<City> GetCityByNameAsync(string stateId, string name);

        /// <summary>
        ///     按省份获取的所有城市。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <returns>城市列表。</returns>
        List<City> GetCitiesInState(string stateId);

        /// <summary>
        ///     异步按省份获取的所有城市。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <returns>城市列表。</returns>
        Task<List<City>> GetCitiesInStateAsync(string stateId);

        /// <summary>
        ///     按省份根据名称查找城市。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <param name="nameFilter">名称过滤表达式。</param>
        /// <returns>省份列表。</returns>
        List<City> FindCitiesInStateByName(string stateId, string nameFilter);

        /// <summary>
        ///     异步按省份根据名称查找省份。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <param name="nameFilter">名称过滤表达式。</param>
        /// <returns>省份列表。</returns>
        Task<List<City>> FindCitiesInStateByNameAsync(string stateId, string nameFilter);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的城市。
        /// </summary>
        /// <param name="newCity">新的城市。</param>
        /// <returns>创建后的城市。</returns>
        City CreateCity(City newCity);

        /// <summary>
        ///     异步创建一个新的城市。
        /// </summary>
        /// <param name="newCity">新的城市。</param>
        /// <returns>创建后的城市。</returns>
        Task<City> CreateCityAsync(City newCity);

        /// <summary>
        ///     删除一个城市。
        /// </summary>
        /// <param name="cityId">城市编号。</param>
        void DeleteCity(string cityId);

        /// <summary>
        ///     异步删除一个城市。
        /// </summary>
        /// <param name="cityId">城市编号。</param>
        Task DeleteCityAsync(string cityId);

        #endregion
    }
}