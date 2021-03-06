﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Geo.Entities;

namespace Sheep.Model.Geo
{
    /// <summary>
    ///     省份的存储库的接口定义。
    /// </summary>
    public interface IStateRepository
    {
        #region 获取

        /// <summary>
        ///     获取省份。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <returns>省份。</returns>
        State GetState(string stateId);

        /// <summary>
        ///     异步获取省份。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        /// <returns>省份。</returns>
        Task<State> GetStateAsync(string stateId);

        /// <summary>
        ///     根据名称获取省份。
        /// </summary>
        /// <param name="countryId">国家编号。</param>
        /// <param name="name">名称。</param>
        /// <returns>省份。</returns>
        State GetStateByName(string countryId, string name);

        /// <summary>
        ///     异步根据名称获取省份。
        /// </summary>
        /// <param name="countryId">国家编号。</param>
        /// <param name="name">名称。</param>
        /// <returns>省份。</returns>
        Task<State> GetStateByNameAsync(string countryId, string name);

        /// <summary>
        ///     按国家获取的所有省份。
        /// </summary>
        /// <param name="countryId">国家编号。</param>
        /// <returns>省份列表。</returns>
        List<State> GetStatesInCountry(string countryId);

        /// <summary>
        ///     异步按国家获取的所有省份。
        /// </summary>
        /// <param name="countryId">国家编号。</param>
        /// <returns>省份列表。</returns>
        Task<List<State>> GetStatesInCountryAsync(string countryId);

        /// <summary>
        ///     按国家根据名称查找省份。
        /// </summary>
        /// <param name="countryId">国家编号。</param>
        /// <param name="nameFilter">名称过滤表达式。</param>
        /// <returns>国家列表。</returns>
        List<State> FindStatesInCountryByName(string countryId, string nameFilter);

        /// <summary>
        ///     异步按国家根据名称查找国家。
        /// </summary>
        /// <param name="countryId">国家编号。</param>
        /// <param name="nameFilter">名称过滤表达式。</param>
        /// <returns>国家列表。</returns>
        Task<List<State>> FindStatesInCountryByNameAsync(string countryId, string nameFilter);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的省份。
        /// </summary>
        /// <param name="newState">新的省份。</param>
        /// <returns>创建后的省份。</returns>
        State CreateState(State newState);

        /// <summary>
        ///     异步创建一个新的省份。
        /// </summary>
        /// <param name="newState">新的省份。</param>
        /// <returns>创建后的省份。</returns>
        Task<State> CreateStateAsync(State newState);

        /// <summary>
        ///     删除一个省份。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        void DeleteState(string stateId);

        /// <summary>
        ///     异步删除一个省份。
        /// </summary>
        /// <param name="stateId">省份编号。</param>
        Task DeleteStateAsync(string stateId);

        #endregion
    }
}