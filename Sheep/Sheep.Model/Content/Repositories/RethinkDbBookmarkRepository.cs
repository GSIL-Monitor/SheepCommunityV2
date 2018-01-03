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
    ///     基于RethinkDb的收藏的存储库。
    /// </summary>
    public class RethinkDbBookmarkRepository : IBookmarkRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbBookmarkRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     收藏的数据表名。
        /// </summary>
        private static readonly string s_BookmarkTable = typeof(Bookmark).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbBookmarkRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbBookmarkRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbBookmarkRepository).Name));
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
            if (tables.Contains(s_BookmarkTable))
            {
                R.TableDrop(s_BookmarkTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_BookmarkTable))
            {
                R.TableCreate(s_BookmarkTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_BookmarkTable).IndexCreate("ParentId_UserId", row => R.Array(row.G("ParentId"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_BookmarkTable).IndexCreate("ParentId").RunResult(_conn).AssertNoErrors();
                R.Table(s_BookmarkTable).IndexCreate("UserId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_BookmarkTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_BookmarkTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测收藏是否存在

        private void AssertNoExistingBookmark(Bookmark newBookmark, Bookmark exceptForExistingBookmark = null)
        {
            var existingBookmark = GetBookmark(newBookmark.ParentId, newBookmark.UserId);
            if (existingBookmark != null && (exceptForExistingBookmark == null || existingBookmark.Id != exceptForExistingBookmark.Id))
            {
                throw new ArgumentException(string.Format(Resources.ParentWithUserAlreadyExists, newBookmark.ParentId, newBookmark.UserId));
            }
        }

        private async Task AssertNoExistingBookmarkAsync(Bookmark newBookmark, Bookmark exceptForExistingBookmark = null)
        {
            var existingBookmark = await GetBookmarkAsync(newBookmark.ParentId, newBookmark.UserId);
            if (existingBookmark != null && (exceptForExistingBookmark == null || existingBookmark.Id != exceptForExistingBookmark.Id))
            {
                throw new ArgumentException(string.Format(Resources.ParentWithUserAlreadyExists, newBookmark.ParentId, newBookmark.UserId));
            }
        }

        #endregion

        #region IBookmarkRepository 接口实现

        /// <inheritdoc />
        public Bookmark GetBookmark(string parentId, int userId)
        {
            return R.Table(s_BookmarkTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Nth(0).Default_(default(Bookmark)).RunResult<Bookmark>(_conn);
        }

        /// <inheritdoc />
        public Task<Bookmark> GetBookmarkAsync(string parentId, int userId)
        {
            return R.Table(s_BookmarkTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Nth(0).Default_(default(Bookmark)).RunResultAsync<Bookmark>(_conn);
        }

        /// <inheritdoc />
        public List<Bookmark> FindBookmarksByParent(string parentId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_BookmarkTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Bookmark>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Bookmark>> FindBookmarksByParentAsync(string parentId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_BookmarkTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Bookmark>>(_conn);
        }

        /// <inheritdoc />
        public List<Bookmark> FindBookmarksByUser(int userId, string parentType, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_BookmarkTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Bookmark>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Bookmark>> FindBookmarksByUserAsync(int userId, string parentType, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_BookmarkTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Bookmark>>(_conn);
        }

        /// <inheritdoc />
        public int GetBookmarksCountByParent(string parentId, DateTime? createdSince)
        {
            var query = R.Table(s_BookmarkTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetBookmarksCountByParentAsync(string parentId, DateTime? createdSince)
        {
            var query = R.Table(s_BookmarkTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetBookmarksCountByUser(int userId, string parentType, DateTime? createdSince)
        {
            var query = R.Table(s_BookmarkTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetBookmarksCountByUserAsync(int userId, string parentType, DateTime? createdSince)
        {
            var query = R.Table(s_BookmarkTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public Bookmark CreateBookmark(Bookmark newBookmark)
        {
            newBookmark.ThrowIfNull(nameof(newBookmark));
            AssertNoExistingBookmark(newBookmark);
            newBookmark.Id = string.Format("{0}-{1}", newBookmark.ParentId, newBookmark.UserId);
            newBookmark.CreatedDate = DateTime.UtcNow;
            var result = R.Table(s_BookmarkTable).Get(newBookmark.Id).Replace(newBookmark).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Bookmark>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Bookmark> CreateBookmarkAsync(Bookmark newBookmark)
        {
            newBookmark.ThrowIfNull(nameof(newBookmark));
            await AssertNoExistingBookmarkAsync(newBookmark);
            newBookmark.Id = string.Format("{0}-{1}", newBookmark.ParentId, newBookmark.UserId);
            newBookmark.CreatedDate = DateTime.UtcNow;
            var result = (await R.Table(s_BookmarkTable).Get(newBookmark.Id).Replace(newBookmark).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Bookmark>()[0].NewValue;
        }

        /// <inheritdoc />
        public void CreateBookmarks(List<Bookmark> newBookmarks)
        {
            foreach (var newBookmark in newBookmarks)
            {
                newBookmark.ThrowIfNull(nameof(newBookmark));
                newBookmark.Id = string.Format("{0}-{1}", newBookmark.ParentId, newBookmark.UserId);
                newBookmark.CreatedDate = DateTime.UtcNow;
            }
            R.Table(s_BookmarkTable).GetAll(R.Args(newBookmarks.Select(bookmark => bookmark.Id).ToArray())).Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_BookmarkTable).Insert(R.Array(newBookmarks.Select(newBookmark => (object) newBookmark).ToArray())).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task CreateBookmarksAsync(List<Bookmark> newBookmarks)
        {
            foreach (var newBookmark in newBookmarks)
            {
                newBookmark.ThrowIfNull(nameof(newBookmark));
                newBookmark.Id = string.Format("{0}-{1}", newBookmark.ParentId, newBookmark.UserId);
                newBookmark.CreatedDate = DateTime.UtcNow;
            }
            (await R.Table(s_BookmarkTable).GetAll(R.Args(newBookmarks.Select(bookmark => bookmark.Id).ToArray())).Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_BookmarkTable).Insert(R.Array(newBookmarks.Select(newBookmark => (object) newBookmark).ToArray())).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void DeleteBookmark(string parentId, int userId)
        {
            R.Table(s_BookmarkTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteBookmarkAsync(string parentId, int userId)
        {
            (await R.Table(s_BookmarkTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Delete().RunResultAsync(_conn)).AssertNoErrors();
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