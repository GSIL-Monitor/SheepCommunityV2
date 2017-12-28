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
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content.Repositories
{
    /// <summary>
    ///     基于RethinkDb的阅读的存储库。
    /// </summary>
    public class RethinkDbViewRepository : IViewRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbViewRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     阅读的数据表名。
        /// </summary>
        private static readonly string s_ViewTable = typeof(View).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbViewRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbViewRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbViewRepository).Name));
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
            if (tables.Contains(s_ViewTable))
            {
                R.TableDrop(s_ViewTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_ViewTable))
            {
                R.TableCreate(s_ViewTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_ViewTable).IndexCreate("ParentId_UserId", row => R.Array(row.G("ParentId"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_ViewTable).IndexCreate("ParentId").RunResult(_conn).AssertNoErrors();
                R.Table(s_ViewTable).IndexCreate("UserId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_ViewTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_ViewTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测阅读是否存在

        #endregion

        #region IViewRepository 接口实现

        /// <inheritdoc />
        public View GetView(string viewId)
        {
            return R.Table(s_ViewTable).Get(viewId).RunResult<View>(_conn);
        }

        /// <inheritdoc />
        public Task<View> GetViewAsync(string viewId)
        {
            return R.Table(s_ViewTable).Get(viewId).RunResultAsync<View>(_conn);
        }

        /// <inheritdoc />
        public List<View> FindViewsByParent(string parentId, int? userId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ViewTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_ViewTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<View>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<View>> FindViewsByParentAsync(string parentId, int? userId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ViewTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_ViewTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<View>>(_conn);
        }

        /// <inheritdoc />
        public List<View> FindViewsByUser(int userId, string parentType, string parentIdPrefix, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ViewTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<View>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<View>> FindViewsByUserAsync(int userId, string parentType, string parentIdPrefix, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ViewTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<View>>(_conn);
        }

        /// <inheritdoc />
        public int GetViewsCountByParent(string parentId, int? userId, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_ViewTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetViewsCountByParentAsync(string parentId, int? userId, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_ViewTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetDaysCountByParent(string parentId, int? userId, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_ViewTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Map(row => row.G("CreatedDate").Date()).Distinct().Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetDaysCountByParentAsync(string parentId, int? userId, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_ViewTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Map(row => row.G("CreatedDate").Date()).Distinct().Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetViewsCountByUser(int userId, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetViewsCountByUserAsync(int userId, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetParentsCountByUser(int userId, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.WithFields("ParentId").Distinct().Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetParentsCountByUserAsync(int userId, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.WithFields("ParentId").Distinct().Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetDaysCountByUser(int userId, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Map(row => row.G("CreatedDate").Date()).Distinct().Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetDaysCountByUserAsync(int userId, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Map(row => row.G("CreatedDate").Date()).Distinct().Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public List<KeyValuePair<int, int>> GetViewsCountByUsers(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(R.Args(userIds.ToArray())).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Group(row => row.G("UserId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResult<List<KeyValuePair<int, int>>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<KeyValuePair<int, int>>> GetViewsCountByUsersAsync(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(R.Args(userIds.ToArray())).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Group(row => row.G("UserId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResultAsync<List<KeyValuePair<int, int>>>(_conn);
        }

        /// <inheritdoc />
        public List<KeyValuePair<int, int>> GetParentsCountByUsers(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(R.Args(userIds.ToArray())).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.WithFields("UserId", "ParentId").Distinct().Group(row => row.G("UserId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResult<List<KeyValuePair<int, int>>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<KeyValuePair<int, int>>> GetParentsCountByUsersAsync(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(R.Args(userIds.ToArray())).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.WithFields("UserId", "ParentId").Distinct().Group(row => row.G("UserId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResultAsync<List<KeyValuePair<int, int>>>(_conn);
        }

        /// <inheritdoc />
        public List<KeyValuePair<int, int>> GetDaysCountByUsers(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(R.Args(userIds.ToArray())).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Map(row => R.HashMap("UserId", row.G("UserId")).With("CreatedDate", row.G("CreatedDate").Date())).Distinct().Group(row => row.G("UserId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResult<List<KeyValuePair<int, int>>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<KeyValuePair<int, int>>> GetDaysCountByUsersAsync(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince)
        {
            var query = R.Table(s_ViewTable).GetAll(R.Args(userIds.ToArray())).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (!parentIdPrefix.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentId").Match($"^{parentIdPrefix}"));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Map(row => R.HashMap("UserId", row.G("UserId")).With("CreatedDate", row.G("CreatedDate").Date())).Distinct().Group(row => row.G("UserId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResultAsync<List<KeyValuePair<int, int>>>(_conn);
        }

        /// <inheritdoc />
        public View CreateView(View newView)
        {
            newView.ThrowIfNull(nameof(newView));
            newView.Id = newView.Id.IsNullOrEmpty() ? new Base36IdGenerator(12, 5, 7).NewId().ToLower() : newView.Id;
            newView.CreatedDate = DateTime.UtcNow;
            var result = R.Table(s_ViewTable).Get(newView.Id).Replace(newView).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<View>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<View> CreateViewAsync(View newView)
        {
            newView.ThrowIfNull(nameof(newView));
            newView.Id = newView.Id.IsNullOrEmpty() ? new Base36IdGenerator(12, 5, 7).NewId().ToLower() : newView.Id;
            newView.CreatedDate = DateTime.UtcNow;
            var result = (await R.Table(s_ViewTable).Get(newView.Id).Replace(newView).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<View>()[0].NewValue;
        }

        /// <inheritdoc />
        public void CreateViews(List<View> newViews)
        {
            foreach (var newView in newViews)
            {
                newView.ThrowIfNull(nameof(newView));
                newView.Id = newView.Id.IsNullOrEmpty() ? new Base36IdGenerator(12, 5, 7).NewId().ToLower() : newView.Id;
                newView.CreatedDate = DateTime.UtcNow;
            }
            R.Table(s_ViewTable).Insert(R.Array(newViews.Select(newView => (object) newView).ToArray())).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task CreateViewsAsync(List<View> newViews)
        {
            foreach (var newView in newViews)
            {
                newView.ThrowIfNull(nameof(newView));
                newView.Id = newView.Id.IsNullOrEmpty() ? new Base36IdGenerator(12, 5, 7).NewId().ToLower() : newView.Id;
                newView.CreatedDate = DateTime.UtcNow;
            }
            (await R.Table(s_ViewTable).Insert(R.Array(newViews.Select(newView => (object) newView).ToArray())).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void DeleteView(string viewId)
        {
            R.Table(s_ViewTable).Get(viewId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteViewAsync(string viewId)
        {
            (await R.Table(s_ViewTable).Get(viewId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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