using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funcular.IdGenerators.Base36;
using RethinkDb.Driver;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Logging;
using Sheep.Model.Membership.Entities;

namespace Sheep.Model.Membership.Repositories
{
    /// <summary>
    ///     基于RethinkDb的用户排行的存储库。
    /// </summary>
    public class RethinkDbUserRankRepository : IUserRankRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbUserRankRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     用户排行的数据表名。
        /// </summary>
        private static readonly string s_UserRankTable = typeof(UserRank).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbUserRankRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbUserRankRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbUserRankRepository).Name));
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
            if (tables.Contains(s_UserRankTable))
            {
                R.TableDrop(s_UserRankTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_UserRankTable))
            {
                R.TableCreate(s_UserRankTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                //R.Table(s_UserRankTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_UserRankTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测用户排行是否存在

        #endregion

        #region IUserRankRepository 接口实现

        /// <inheritdoc />
        public UserRank GetUserRank(int userId)
        {
            return R.Table(s_UserRankTable).Get(userId).RunResult<UserRank>(_conn);
        }

        /// <inheritdoc />
        public Task<UserRank> GetUserRankAsync(int userId)
        {
            return R.Table(s_UserRankTable).Get(userId).RunResultAsync<UserRank>(_conn);
        }

        /// <inheritdoc />
        public List<UserRank> FindUserRanks(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_UserRankTable).Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<UserRank>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<UserRank>> FindUserRanksAsync(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_UserRankTable).Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<UserRank>>(_conn);
        }

        /// <inheritdoc />
        public UserRank CreateUserRank(UserRank newUserRank)
        {
            newUserRank.ThrowIfNull(nameof(newUserRank));
            newUserRank.CreatedDate = DateTime.UtcNow;
            newUserRank.ModifiedDate = newUserRank.CreatedDate;
            var result = R.Table(s_UserRankTable).Get(newUserRank.Id).Replace(newUserRank).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<UserRank>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<UserRank> CreateUserRankAsync(UserRank newUserRank)
        {
            newUserRank.ThrowIfNull(nameof(newUserRank));
            newUserRank.CreatedDate = DateTime.UtcNow;
            newUserRank.ModifiedDate = newUserRank.CreatedDate;
            var result = (await R.Table(s_UserRankTable).Get(newUserRank.Id).Replace(newUserRank).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<UserRank>()[0].NewValue;
        }

        /// <inheritdoc />
        public UserRank UpdateUserRank(UserRank existingUserRank, UserRank newUserRank)
        {
            existingUserRank.ThrowIfNull(nameof(existingUserRank));
            newUserRank.ThrowIfNull(nameof(newUserRank));
            newUserRank.Id = existingUserRank.Id;
            newUserRank.CreatedDate = existingUserRank.CreatedDate;
            newUserRank.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_UserRankTable).Get(newUserRank.Id).Replace(newUserRank).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<UserRank>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<UserRank> UpdateUserRankAsync(UserRank existingUserRank, UserRank newUserRank)
        {
            existingUserRank.ThrowIfNull(nameof(existingUserRank));
            newUserRank.ThrowIfNull(nameof(newUserRank));
            newUserRank.Id = existingUserRank.Id;
            newUserRank.CreatedDate = existingUserRank.CreatedDate;
            newUserRank.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_UserRankTable).Get(newUserRank.Id).Replace(newUserRank).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<UserRank>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteUserRank(int userId)
        {
            R.Table(s_UserRankTable).Get(userId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteUserRankAsync(int userId)
        {
            (await R.Table(s_UserRankTable).Get(userId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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