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
using Sheep.Model.Membership.Entities;

namespace Sheep.Model.Membership.Repositories
{
    /// <summary>
    ///     基于RethinkDb的群组排行的存储库。
    /// </summary>
    public class RethinkDbGroupRankRepository : IGroupRankRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbGroupRankRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     群组排行的数据表名。
        /// </summary>
        private static readonly string s_GroupRankTable = typeof(GroupRank).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbGroupRankRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbGroupRankRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbGroupRankRepository).Name));
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
            if (tables.Contains(s_GroupRankTable))
            {
                R.TableDrop(s_GroupRankTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_GroupRankTable))
            {
                R.TableCreate(s_GroupRankTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                //R.Table(s_GroupRankTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_GroupRankTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测群组排行是否存在

        #endregion

        #region IGroupRankRepository 接口实现

        /// <inheritdoc />
        public GroupRank GetGroupRank(string groupId)
        {
            return R.Table(s_GroupRankTable).Get(groupId).RunResult<GroupRank>(_conn);
        }

        /// <inheritdoc />
        public Task<GroupRank> GetGroupRankAsync(string groupId)
        {
            return R.Table(s_GroupRankTable).Get(groupId).RunResultAsync<GroupRank>(_conn);
        }

        /// <inheritdoc />
        public List<GroupRank> FindGroupRanks(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_GroupRankTable).Filter(true);
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
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("ParagraphViewsCount")) : query.OrderBy("ParagraphViewsCount");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<GroupRank>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<GroupRank>> FindGroupRanksAsync(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_GroupRankTable).Filter(true);
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
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("ParagraphViewsCount")) : query.OrderBy("ParagraphViewsCount");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<GroupRank>>(_conn);
        }

        /// <inheritdoc />
        public List<GroupRank> FindGroupRanks(List<string> groupIds, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_GroupRankTable).GetAll(R.Args(groupIds.ToArray())).OptArg("index", "Id").Filter(true);
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
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("ParagraphViewsCount")) : query.OrderBy("ParagraphViewsCount");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<GroupRank>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<GroupRank>> FindGroupRanksAsync(List<string> groupIds, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_GroupRankTable).GetAll(R.Args(groupIds.ToArray())).OptArg("index", "Id").Filter(true);
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
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("ParagraphViewsCount")) : query.OrderBy("ParagraphViewsCount");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<GroupRank>>(_conn);
        }

        /// <inheritdoc />
        public GroupRank CreateGroupRank(GroupRank newGroupRank)
        {
            newGroupRank.ThrowIfNull(nameof(newGroupRank));
            newGroupRank.CreatedDate = DateTime.UtcNow;
            newGroupRank.ModifiedDate = newGroupRank.CreatedDate;
            var result = R.Table(s_GroupRankTable).Get(newGroupRank.Id).Replace(newGroupRank).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            var changes = result.ChangesAs<GroupRank>();
            return changes.Length != 0 ? changes[0].NewValue : null;
        }

        /// <inheritdoc />
        public async Task<GroupRank> CreateGroupRankAsync(GroupRank newGroupRank)
        {
            newGroupRank.ThrowIfNull(nameof(newGroupRank));
            newGroupRank.CreatedDate = DateTime.UtcNow;
            newGroupRank.ModifiedDate = newGroupRank.CreatedDate;
            var result = (await R.Table(s_GroupRankTable).Get(newGroupRank.Id).Replace(newGroupRank).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            var changes = result.ChangesAs<GroupRank>();
            return changes.Length != 0 ? changes[0].NewValue : null;
        }

        /// <inheritdoc />
        public GroupRank UpdateGroupRank(GroupRank existingGroupRank, GroupRank newGroupRank)
        {
            existingGroupRank.ThrowIfNull(nameof(existingGroupRank));
            newGroupRank.ThrowIfNull(nameof(newGroupRank));
            newGroupRank.Id = existingGroupRank.Id;
            newGroupRank.CreatedDate = existingGroupRank.CreatedDate;
            newGroupRank.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_GroupRankTable).Get(newGroupRank.Id).Replace(newGroupRank).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            var changes = result.ChangesAs<GroupRank>();
            return changes.Length != 0 ? changes[0].NewValue : null;
        }

        /// <inheritdoc />
        public async Task<GroupRank> UpdateGroupRankAsync(GroupRank existingGroupRank, GroupRank newGroupRank)
        {
            existingGroupRank.ThrowIfNull(nameof(existingGroupRank));
            newGroupRank.ThrowIfNull(nameof(newGroupRank));
            newGroupRank.Id = existingGroupRank.Id;
            newGroupRank.CreatedDate = existingGroupRank.CreatedDate;
            newGroupRank.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_GroupRankTable).Get(newGroupRank.Id).Replace(newGroupRank).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            var changes = result.ChangesAs<GroupRank>();
            return changes.Length != 0 ? changes[0].NewValue : null;
        }

        /// <inheritdoc />
        public void DeleteGroupRank(string groupId)
        {
            R.Table(s_GroupRankTable).Get(groupId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteGroupRankAsync(string groupId)
        {
            (await R.Table(s_GroupRankTable).Get(groupId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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