using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Geo.Entities;

namespace Sheep.Model.Geo
{
    /// <summary>
    ///     城市的存储库的接口定义。
    /// </summary>
    public interface IGeoCityRepository
    {
        #region 获取

        /// <summary>
        ///     获取城市。
        /// </summary>
        /// <param name="cityId">城市编号。</param>
        /// <returns>城市。</returns>
        GeoCity GetCity(string cityId);

        /// <summary>
        ///     异步获取城市。
        /// </summary>
        /// <param name="cityId">城市编号。</param>
        /// <returns>城市。</returns>
        Task<GeoCity> GetCityAsync(string cityId);

        /// <summary>
        ///     根据名称获取城市。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <param name="name">名称。</param>
        /// <returns>城市。</returns>
        GeoCity GetCityByName(string stateId, string name);

        /// <summary>
        ///     异步根据名称获取城市。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <param name="name">名称。</param>
        /// <returns>城市。</returns>
        Task<GeoCity> GetCityByNameAsync(string stateId, string name);

        /// <summary>
        ///     按省份获取的所有城市。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <returns>城市列表。</returns>
        List<GeoCity> GetCitiesInState(string stateId);

        /// <summary>
        ///     异步按省份获取的所有城市。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <returns>城市列表。</returns>
        Task<List<GeoCity>> GetCitiesInStateAsync(string stateId);

        /// <summary>
        ///     按省份根据名称查找城市。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <param name="nameFilter">名称过滤表达式。</param>
        /// <returns>省份列表。</returns>
        List<GeoCity> FindCitiesInStateByName(string stateId, string nameFilter);

        /// <summary>
        ///     异步按省份根据名称查找省份。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <param name="nameFilter">名称过滤表达式。</param>
        /// <returns>省份列表。</returns>
        Task<List<GeoCity>> FindCitiesInStateByNameAsync(string stateId, string nameFilter);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的城市。
        /// </summary>
        /// <param name="newCity">新的城市。</param>
        /// <returns>创建后的城市。</returns>
        GeoCity CreateCity(GeoCity newCity);

        /// <summary>
        ///     异步创建一个新的城市。
        /// </summary>
        /// <param name="newCity">新的城市。</param>
        /// <returns>创建后的城市。</returns>
        Task<GeoCity> CreateCityAsync(GeoCity newCity);

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