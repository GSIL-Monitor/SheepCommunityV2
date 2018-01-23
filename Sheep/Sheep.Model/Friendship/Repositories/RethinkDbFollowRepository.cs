using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RethinkDb.Driver;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Logging;
using Sheep.Model.Friendship.Entities;
using Sheep.Model.Properties;

namespace Sheep.Model.Friendship.Repositories
{
    /// <summary>
    ///     基于RethinkDb的关注的存储库。
    /// </summary>
    public class RethinkDbFollowRepository : IFollowRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbFollowRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     关注的数据表名。
        /// </summary>
        private static readonly string s_FollowTable = typeof(Follow).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbFollowRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbFollowRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbFollowRepository).Name));
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
            if (tables.Contains(s_FollowTable))
            {
                R.TableDrop(s_FollowTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_FollowTable))
            {
                R.TableCreate(s_FollowTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_FollowTable).IndexCreate("OwnerId_FollowerId", row => R.Array(row.G("OwnerId"), row.G("FollowerId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_FollowTable).IndexCreate("OwnerId").RunResult(_conn).AssertNoErrors();
                R.Table(s_FollowTable).IndexCreate("FollowerId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_FollowTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_FollowTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测关注是否存在

        private void AssertNoExistingFollow(Follow newFollow, Follow exceptForExistingFollow = null)
        {
            var existingFollow = GetFollow(newFollow.OwnerId, newFollow.FollowerId);
            if (existingFollow != null && (exceptForExistingFollow == null || existingFollow.Id != exceptForExistingFollow.Id))
            {
                throw new ArgumentException(string.Format(Resources.OwnerWithFollowerAlreadyExists, newFollow.OwnerId, newFollow.FollowerId));
            }
        }

        private async Task AssertNoExistingFollowAsync(Follow newFollow, Follow exceptForExistingFollow = null)
        {
            var existingFollow = await GetFollowAsync(newFollow.OwnerId, newFollow.FollowerId);
            if (existingFollow != null && (exceptForExistingFollow == null || existingFollow.Id != exceptForExistingFollow.Id))
            {
                throw new ArgumentException(string.Format(Resources.OwnerWithFollowerAlreadyExists, newFollow.OwnerId, newFollow.FollowerId));
            }
        }

        #endregion

        #region IFollowRepository 接口实现

        /// <inheritdoc />
        public Follow GetFollow(int ownerId, int followerId)
        {
            return R.Table(s_FollowTable).GetAll(R.Array(ownerId, followerId)).OptArg("index", "OwnerId_FollowerId").Nth(0).Default_(default(Follow)).RunResult<Follow>(_conn);
        }

        /// <inheritdoc />
        public Task<Follow> GetFollowAsync(int ownerId, int followerId)
        {
            return R.Table(s_FollowTable).GetAll(R.Array(ownerId, followerId)).OptArg("index", "OwnerId_FollowerId").Nth(0).Default_(default(Follow)).RunResultAsync<Follow>(_conn);
        }

        /// <inheritdoc />
        public List<Follow> FindFollowsByOwner(int ownerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_FollowTable).GetAll(ownerId).OptArg("index", "OwnerId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Follow>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Follow>> FindFollowsByOwnerAsync(int ownerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_FollowTable).GetAll(ownerId).OptArg("index", "OwnerId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Follow>>(_conn);
        }

        /// <inheritdoc />
        public List<Follow> FindFollowsByFollower(int followerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_FollowTable).GetAll(followerId).OptArg("index", "FollowerId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Follow>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Follow>> FindFollowsByFollowerAsync(int followerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_FollowTable).GetAll(followerId).OptArg("index", "FollowerId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Follow>>(_conn);
        }

        /// <inheritdoc />
        public Follow CreateFollow(Follow newFollow)
        {
            newFollow.ThrowIfNull(nameof(newFollow));
            AssertNoExistingFollow(newFollow);
            newFollow.Id = string.Format("{0}-{1}", newFollow.OwnerId, newFollow.FollowerId);
            newFollow.CreatedDate = DateTime.UtcNow;
            newFollow.ModifiedDate = newFollow.CreatedDate;
            var result = R.Table(s_FollowTable).Get(newFollow.Id).Replace(newFollow).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Follow>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Follow> CreateFollowAsync(Follow newFollow)
        {
            newFollow.ThrowIfNull(nameof(newFollow));
            await AssertNoExistingFollowAsync(newFollow);
            newFollow.Id = string.Format("{0}-{1}", newFollow.OwnerId, newFollow.FollowerId);
            newFollow.CreatedDate = DateTime.UtcNow;
            newFollow.ModifiedDate = newFollow.CreatedDate;
            var result = (await R.Table(s_FollowTable).Get(newFollow.Id).Replace(newFollow).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Follow>()[0].NewValue;
        }

        /// <inheritdoc />
        public Follow UpdateFollow(Follow existingFollow, Follow newFollow)
        {
            existingFollow.ThrowIfNull(nameof(existingFollow));
            AssertNoExistingFollow(newFollow, existingFollow);
            newFollow.Id = existingFollow.Id;
            newFollow.CreatedDate = existingFollow.CreatedDate;
            newFollow.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_FollowTable).Get(newFollow.Id).Replace(newFollow).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Follow>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Follow> UpdateFollowAsync(Follow existingFollow, Follow newFollow)
        {
            existingFollow.ThrowIfNull(nameof(existingFollow));
            await AssertNoExistingFollowAsync(newFollow, existingFollow);
            newFollow.Id = existingFollow.Id;
            newFollow.CreatedDate = existingFollow.CreatedDate;
            newFollow.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_FollowTable).Get(newFollow.Id).Replace(newFollow).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Follow>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteFollow(int ownerId, int followerId)
        {
            R.Table(s_FollowTable).GetAll(R.Array(ownerId, followerId)).OptArg("index", "OwnerId_FollowerId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteFollowAsync(int ownerId, int followerId)
        {
            (await R.Table(s_FollowTable).GetAll(R.Array(ownerId, followerId)).OptArg("index", "OwnerId_FollowerId").Delete().RunResultAsync(_conn)).AssertNoErrors();
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