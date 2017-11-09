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
    ///     基于RethinkDb的国家的存储库。
    /// </summary>
    public class RethinkDbCountryRepository : ICountryRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbCountryRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     国家的数据表名。
        /// </summary>
        private static readonly string s_CountryTable = typeof(Country).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbCountryRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbCountryRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbCountryRepository).Name));
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
            if (tables.Contains(s_CountryTable))
            {
                R.TableDrop(s_CountryTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_CountryTable))
            {
                R.TableCreate(s_CountryTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_CountryTable).IndexCreate("Name").RunResult(_conn).AssertNoErrors();
                //R.Table(s_CountryTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_CountryTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region ICountryRepository 接口实现

        /// <inheritdoc />
        public Country GetCountry(string countryId)
        {
            if (countryId.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_CountryTable).Get(countryId).RunResult<Country>(_conn);
        }

        /// <inheritdoc />
        public Task<Country> GetCountryAsync(string countryId)
        {
            if (countryId.IsNullOrEmpty())
            {
                return Task.FromResult<Country>(null);
            }
            return R.Table(s_CountryTable).Get(countryId).RunResultAsync<Country>(_conn);
        }

        /// <inheritdoc />
        public Country GetCountryByName(string name)
        {
            if (name.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_CountryTable).GetAll(name).OptArg("index", "Name").Nth(0).Default_(default(Country)).RunResult<Country>(_conn);
        }

        /// <inheritdoc />
        public Task<Country> GetCountryByNameAsync(string name)
        {
            if (name.IsNullOrEmpty())
            {
                return Task.FromResult<Country>(null);
            }
            return R.Table(s_CountryTable).GetAll(name).OptArg("index", "Name").Nth(0).Default_(default(Country)).RunResultAsync<Country>(_conn);
        }

        /// <inheritdoc />
        public List<Country> GetCountries()
        {
            return R.Table(s_CountryTable).OrderBy().OptArg("index", "Id").RunResult<List<Country>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Country>> GetCountriesAsync()
        {
            return R.Table(s_CountryTable).OrderBy().OptArg("index", "Id").RunResultAsync<List<Country>>(_conn);
        }

        /// <inheritdoc />
        public List<Country> FindCountriesByName(string nameFilter)
        {
            if (nameFilter.IsNullOrEmpty())
            {
                return new List<Country>();
            }
            return R.Table(s_CountryTable).OrderBy().OptArg("index", "Id").Filter(row => row.G("Name").Match(nameFilter)).RunResult<List<Country>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Country>> FindCountriesByNameAsync(string nameFilter)
        {
            if (nameFilter.IsNullOrEmpty())
            {
                return Task.FromResult(new List<Country>());
            }
            return R.Table(s_CountryTable).OrderBy().OptArg("index", "Id").Filter(row => row.G("Name").Match(nameFilter)).RunResultAsync<List<Country>>(_conn);
        }

        /// <inheritdoc />
        public Country CreateCountry(Country newCountry)
        {
            newCountry.Id.ThrowIfNullOrEmpty(nameof(newCountry.Id));
            newCountry.Name.ThrowIfNullOrEmpty(nameof(newCountry.Name));
            var result = R.Table(s_CountryTable).Get(newCountry.Id).Replace(newCountry).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Country>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Country> CreateCountryAsync(Country newCountry)
        {
            newCountry.Id.ThrowIfNullOrEmpty(nameof(newCountry.Id));
            newCountry.Name.ThrowIfNullOrEmpty(nameof(newCountry.Name));
            var result = (await R.Table(s_CountryTable).Get(newCountry.Id).Replace(newCountry).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Country>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteCountry(string countryId)
        {
            if (countryId.IsNullOrEmpty())
            {
                return;
            }
            R.Table(s_CountryTable).Get(countryId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteCountryAsync(string countryId)
        {
            if (countryId.IsNullOrEmpty())
            {
                return;
            }
            (await R.Table(s_CountryTable).Get(countryId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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