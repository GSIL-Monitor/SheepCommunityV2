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
using Sheep.Model.Content.Entities;
using Sheep.Model.Properties;

namespace Sheep.Model.Content.Repositories
{
    /// <summary>
    ///     基于RethinkDb的投票的存储库。
    /// </summary>
    public class RethinkDbVoteRepository : IVoteRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbVoteRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     投票的数据表名。
        /// </summary>
        private static readonly string s_VoteTable = typeof(Vote).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbVoteRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbVoteRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbVoteRepository).Name));
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
            if (tables.Contains(s_VoteTable))
            {
                R.TableDrop(s_VoteTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_VoteTable))
            {
                R.TableCreate(s_VoteTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_VoteTable).IndexCreate("ParentId_UserId", row => R.Array(row.G("ParentId"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_VoteTable).IndexCreate("ParentId").RunResult(_conn).AssertNoErrors();
                R.Table(s_VoteTable).IndexCreate("UserId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_VoteTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_VoteTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测投票是否存在

        private void AssertNoExistingVote(Vote newVote, Vote exceptForExistingVote = null)
        {
            var existingVote = GetVote(newVote.ParentId, newVote.UserId);
            if (existingVote != null && (exceptForExistingVote == null || existingVote.Id != exceptForExistingVote.Id))
            {
                throw new ArgumentException(string.Format(Resources.ParentWithUserAlreadyExists, newVote.ParentId, newVote.UserId));
            }
        }

        private async Task AssertNoExistingVoteAsync(Vote newVote, Vote exceptForExistingVote = null)
        {
            var existingVote = await GetVoteAsync(newVote.ParentId, newVote.UserId);
            if (existingVote != null && (exceptForExistingVote == null || existingVote.Id != exceptForExistingVote.Id))
            {
                throw new ArgumentException(string.Format(Resources.ParentWithUserAlreadyExists, newVote.ParentId, newVote.UserId));
            }
        }

        #endregion

        #region IVoteRepository 接口实现

        /// <inheritdoc />
        public Vote GetVote(string parentId, int userId)
        {
            return R.Table(s_VoteTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Nth(0).Default_(default(Vote)).RunResult<Vote>(_conn);
        }

        /// <inheritdoc />
        public Task<Vote> GetVoteAsync(string parentId, int userId)
        {
            return R.Table(s_VoteTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Nth(0).Default_(default(Vote)).RunResultAsync<Vote>(_conn);
        }

        /// <inheritdoc />
        public List<Vote> GetVotes(List<Tuple<string, int>> compositeIds)
        {
            if (compositeIds == null)
            {
                return new List<Vote>();
            }
            return R.Table(s_VoteTable).GetAll(R.Args(compositeIds.Select(compositeId => R.Array(compositeId.Item1, compositeId.Item2)).ToArray())).OptArg("index", "ParentId_UserId").RunResult<List<Vote>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Vote>> GetVotesAsync(List<Tuple<string, int>> compositeIds)
        {
            if (compositeIds == null)
            {
                return Task.FromResult(new List<Vote>());
            }
            return R.Table(s_VoteTable).GetAll(R.Args(compositeIds.Select(compositeId => R.Array(compositeId.Item1, compositeId.Item2)).ToArray())).OptArg("index", "ParentId_UserId").RunResultAsync<List<Vote>>(_conn);
        }

        /// <inheritdoc />
        public List<Vote> FindVotesByParent(string parentId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VoteTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Vote>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Vote>> FindVotesByParentAsync(string parentId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VoteTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Vote>>(_conn);
        }

        /// <inheritdoc />
        public List<Vote> FindVotesByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VoteTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Vote>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Vote>> FindVotesByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VoteTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Vote>>(_conn);
        }

        /// <inheritdoc />
        public int GetVotesCountByParent(string parentId, DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_VoteTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetVotesCountByParentAsync(string parentId, DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_VoteTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetVotesCountByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_VoteTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetVotesCountByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_VoteTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public float CalculateUserVotesScore(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince)
        {
            var count = GetVotesCountByUser(userId, parentType, createdSince, modifiedSince);
            return Math.Min(1.0f, count / 50.0f);
        }

        /// <inheritdoc />
        public async Task<float> CalculateUserVotesScoreAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince)
        {
            var count = await GetVotesCountByUserAsync(userId, parentType, createdSince, modifiedSince);
            return Math.Min(1.0f, count / 50.0f);
        }

        /// <inheritdoc />
        public Vote CreateVote(Vote newVote)
        {
            newVote.ThrowIfNull(nameof(newVote));
            AssertNoExistingVote(newVote);
            newVote.Id = string.Format("{0}-{1}", newVote.ParentId, newVote.UserId);
            newVote.CreatedDate = DateTime.UtcNow;
            newVote.ModifiedDate = newVote.CreatedDate;
            var result = R.Table(s_VoteTable).Get(newVote.Id).Replace(newVote).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Vote>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Vote> CreateVoteAsync(Vote newVote)
        {
            newVote.ThrowIfNull(nameof(newVote));
            await AssertNoExistingVoteAsync(newVote);
            newVote.Id = string.Format("{0}-{1}", newVote.ParentId, newVote.UserId);
            newVote.CreatedDate = DateTime.UtcNow;
            newVote.ModifiedDate = newVote.CreatedDate;
            var result = (await R.Table(s_VoteTable).Get(newVote.Id).Replace(newVote).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Vote>()[0].NewValue;
        }

        /// <inheritdoc />
        public Vote UpdateVote(Vote existingVote, Vote newVote)
        {
            existingVote.ThrowIfNull(nameof(existingVote));
            newVote.Id = existingVote.Id;
            newVote.CreatedDate = existingVote.CreatedDate;
            newVote.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_VoteTable).Get(newVote.Id).Replace(newVote).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Vote>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Vote> UpdateVoteAsync(Vote existingVote, Vote newVote)
        {
            existingVote.ThrowIfNull(nameof(existingVote));
            newVote.Id = existingVote.Id;
            newVote.CreatedDate = existingVote.CreatedDate;
            newVote.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_VoteTable).Get(newVote.Id).Replace(newVote).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Vote>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteVote(string parentId, int userId)
        {
            R.Table(s_VoteTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteVoteAsync(string parentId, int userId)
        {
            (await R.Table(s_VoteTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Delete().RunResultAsync(_conn)).AssertNoErrors();
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