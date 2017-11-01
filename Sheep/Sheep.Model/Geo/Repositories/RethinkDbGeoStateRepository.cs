﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RethinkDb.Driver;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Logging;
using Sheep.Model.Geo.Entities;

namespace Sheep.Model.Geo.Repositories
{
    /// <summary>
    ///     基于RethinkDb的省份的存储库。
    /// </summary>
    public class RethinkDbGeoStateRepository : IGeoStateRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbGeoStateRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     省份的数据表名。
        /// </summary>
        private static readonly string s_GeoStateTable = typeof(GeoState).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbGeoStateRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbGeoStateRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
        {
            _conn = conn;
            _shards = shards;
            _replicas = replicas;
            // 创建数据表。
            if (createMissingTables)
            {
                CreateTables();
            }
            // 检测指定的数据表是否存在。
            if (!TablesExists())
            {
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbGeoStateRepository).Name));
            }
        }

        #endregion

        #region 数据表检测及创建

        /// <summary>
        ///     删除并重新创建数据表。
        /// </summary>
        public void DropAndReCreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (tables.Contains(s_GeoStateTable))
            {
                R.TableDrop(s_GeoStateTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_GeoStateTable))
            {
                R.TableCreate(s_GeoStateTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_GeoStateTable).IndexCreate("CountryId_Name", row => R.Array(row.G("CountryId"), row.G("Name"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_GeoStateTable).IndexCreate("CountryId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_GeoStateTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_GeoStateTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region IGeoStateRepository 接口实现

        /// <inheritdoc />
        public GeoState GetState(string stateId)
        {
            if (stateId.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_GeoStateTable).Get(stateId).RunResult<GeoState>(_conn);
        }

        /// <inheritdoc />
        public Task<GeoState> GetStateAsync(string stateId)
        {
            if (stateId.IsNullOrEmpty())
            {
                return Task.FromResult<GeoState>(null);
            }
            return R.Table(s_GeoStateTable).Get(stateId).RunResultAsync<GeoState>(_conn);
        }

        /// <inheritdoc />
        public GeoState GetStateByName(string countryId, string name)
        {
            if (countryId.IsNullOrEmpty() || name.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_GeoStateTable).GetAll(R.Array(countryId, name)).OptArg("index", "CountryId_Name").Nth(0).Default_(default(GeoState)).RunResult<GeoState>(_conn);
        }

        /// <inheritdoc />
        public Task<GeoState> GetStateByNameAsync(string countryId, string name)
        {
            if (countryId.IsNullOrEmpty() || name.IsNullOrEmpty())
            {
                return Task.FromResult<GeoState>(null);
            }
            return R.Table(s_GeoStateTable).GetAll(R.Array(countryId, name)).OptArg("index", "CountryId_Name").Nth(0).Default_(default(GeoState)).RunResultAsync<GeoState>(_conn);
        }

        /// <inheritdoc />
        public List<GeoState> GetStatesInCountry(string countryId)
        {
            if (countryId.IsNullOrEmpty())
            {
                return new List<GeoState>();
            }
            return R.Table(s_GeoStateTable).GetAll(countryId).OptArg("index", "CountryId").OrderBy("Id") .RunResult<List<GeoState>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<GeoState>> GetStatesInCountryAsync(string countryId)
        {
            if (countryId.IsNullOrEmpty())
            {
                return Task.FromResult(new List<GeoState>());
            }
            return R.Table(s_GeoStateTable).GetAll(countryId).OptArg("index", "CountryId").OrderBy("Id").RunResultAsync<List<GeoState>>(_conn);
        }

        /// <inheritdoc />
        public List<GeoState> FindStatesInCountryByName(string countryId, string nameFilter)
        {
            if (countryId.IsNullOrEmpty() || nameFilter.IsNullOrEmpty())
            {
                return new List<GeoState>();
            }
            return R.Table(s_GeoStateTable).GetAll(countryId).OptArg("index", "CountryId").Filter(row => row.G("Name").Match(nameFilter)).OrderBy("Id").RunResult<List<GeoState>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<GeoState>> FindStatesInCountryByNameAsync(string countryId, string nameFilter)
        {
            if (countryId.IsNullOrEmpty() || nameFilter.IsNullOrEmpty())
            {
                return Task.FromResult(new List<GeoState>());
            }
            return R.Table(s_GeoStateTable).GetAll(countryId).OptArg("index", "CountryId").Filter(row => row.G("Name").Match(nameFilter)).OrderBy("Id").RunResultAsync<List<GeoState>>(_conn);
        }

        /// <inheritdoc />
        public GeoState CreateState(GeoState newState)
        {
            newState.Id.ThrowIfNullOrEmpty(nameof(newState.Id));
            newState.CountryId.ThrowIfNullOrEmpty(nameof(newState.CountryId));
            newState.Name.ThrowIfNullOrEmpty(nameof(newState.Name));
            var result = R.Table(s_GeoStateTable).Get(newState.Id).Replace(newState).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<GeoState>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<GeoState> CreateStateAsync(GeoState newState)
        {
            newState.Id.ThrowIfNullOrEmpty(nameof(newState.Id));
            newState.CountryId.ThrowIfNullOrEmpty(nameof(newState.CountryId));
            newState.Name.ThrowIfNullOrEmpty(nameof(newState.Name));
            var result = (await R.Table(s_GeoStateTable).Get(newState.Id).Replace(newState).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<GeoState>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteState(string stateId)
        {
            if (stateId.IsNullOrEmpty())
            {
                return;
            }
            R.Table(s_GeoStateTable).Get(stateId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteStateAsync(string stateId)
        {
            if (stateId.IsNullOrEmpty())
            {
                return;
            }
            (await R.Table(s_GeoStateTable).Get(stateId).Delete().RunResultAsync(_conn)).AssertNoErrors();
        }

        #endregion

        #region IClearable 接口实现

        /// <inheritdoc />
        public void Clear()
        {
            DropAndReCreateTables();
        }

        #endregion
    }
}