using System;
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
    ///     基于RethinkDb的城市的存储库。
    /// </summary>
    public class RethinkDbCityRepository : ICityRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbCityRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     城市的数据表名。
        /// </summary>
        private static readonly string s_CityTable = typeof(City).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbCityRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbCityRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbCityRepository).Name));
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
            if (tables.Contains(s_CityTable))
            {
                R.TableDrop(s_CityTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_CityTable))
            {
                R.TableCreate(s_CityTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_CityTable).IndexCreate("StateId_Name", row => R.Array(row.G("StateId"), row.G("Name"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_CityTable).IndexCreate("StateId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_CityTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_CityTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region ICityRepository 接口实现

        /// <inheritdoc />
        public City GetCity(string cityId)
        {
            if (cityId.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_CityTable).Get(cityId).RunResult<City>(_conn);
        }

        /// <inheritdoc />
        public Task<City> GetCityAsync(string cityId)
        {
            if (cityId.IsNullOrEmpty())
            {
                return Task.FromResult<City>(null);
            }
            return R.Table(s_CityTable).Get(cityId).RunResultAsync<City>(_conn);
        }

        /// <inheritdoc />
        public City GetCityByName(string stateId, string name)
        {
            if (stateId.IsNullOrEmpty() || name.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_CityTable).GetAll(R.Array(stateId, name)).OptArg("index", "StateId_Name").Nth(0).Default_(default(City)).RunResult<City>(_conn);
        }

        /// <inheritdoc />
        public Task<City> GetCityByNameAsync(string stateId, string name)
        {
            if (stateId.IsNullOrEmpty() || name.IsNullOrEmpty())
            {
                return Task.FromResult<City>(null);
            }
            return R.Table(s_CityTable).GetAll(R.Array(stateId, name)).OptArg("index", "StateId_Name").Nth(0).Default_(default(City)).RunResultAsync<City>(_conn);
        }

        /// <inheritdoc />
        public List<City> GetCitiesInState(string stateId)
        {
            if (stateId.IsNullOrEmpty())
            {
                return new List<City>();
            }
            return R.Table(s_CityTable).GetAll(stateId).OptArg("index", "StateId").OrderBy("Id").RunResult<List<City>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<City>> GetCitiesInStateAsync(string stateId)
        {
            if (stateId.IsNullOrEmpty())
            {
                return Task.FromResult(new List<City>());
            }
            return R.Table(s_CityTable).GetAll(stateId).OptArg("index", "StateId").OrderBy("Id").RunResultAsync<List<City>>(_conn);
        }

        /// <inheritdoc />
        public List<City> FindCitiesInStateByName(string stateId, string nameFilter)
        {
            if (stateId.IsNullOrEmpty() || nameFilter.IsNullOrEmpty())
            {
                return new List<City>();
            }
            return R.Table(s_CityTable).GetAll(stateId).OptArg("index", "StateId").Filter(row => row.G("Name").Match(nameFilter)).OrderBy("Id").RunResult<List<City>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<City>> FindCitiesInStateByNameAsync(string stateId, string nameFilter)
        {
            if (stateId.IsNullOrEmpty() || nameFilter.IsNullOrEmpty())
            {
                return Task.FromResult(new List<City>());
            }
            return R.Table(s_CityTable).GetAll(stateId).OptArg("index", "StateId").Filter(row => row.G("Name").Match(nameFilter)).OrderBy("Id").RunResultAsync<List<City>>(_conn);
        }

        /// <inheritdoc />
        public City CreateCity(City newCity)
        {
            newCity.Id.ThrowIfNullOrEmpty(nameof(newCity.Id));
            newCity.StateId.ThrowIfNullOrEmpty(nameof(newCity.StateId));
            newCity.Name.ThrowIfNullOrEmpty(nameof(newCity.Name));
            var result = R.Table(s_CityTable).Get(newCity.Id).Replace(newCity).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<City>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<City> CreateCityAsync(City newCity)
        {
            newCity.Id.ThrowIfNullOrEmpty(nameof(newCity.Id));
            newCity.StateId.ThrowIfNullOrEmpty(nameof(newCity.StateId));
            newCity.Name.ThrowIfNullOrEmpty(nameof(newCity.Name));
            var result = (await R.Table(s_CityTable).Get(newCity.Id).Replace(newCity).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<City>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteCity(string cityId)
        {
            if (cityId.IsNullOrEmpty())
            {
                return;
            }
            R.Table(s_CityTable).Get(cityId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteCityAsync(string cityId)
        {
            if (cityId.IsNullOrEmpty())
            {
                return;
            }
            (await R.Table(s_CityTable).Get(cityId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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