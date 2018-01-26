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
using Sheep.Model.Bookstore.Entities;

namespace Sheep.Model.Bookstore.Repositories
{
    /// <summary>
    ///     基于RethinkDb的章阅读的存储库。
    /// </summary>
    public class RethinkDbChapterReadRepository : IChapterReadRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbChapterReadRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     章阅读的数据表名。
        /// </summary>
        private static readonly string s_ChapterReadTable = typeof(ChapterRead).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbChapterReadRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbChapterReadRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbChapterReadRepository).Name));
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
            if (tables.Contains(s_ChapterReadTable))
            {
                R.TableDrop(s_ChapterReadTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_ChapterReadTable))
            {
                R.TableCreate(s_ChapterReadTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_ChapterReadTable).IndexCreate("ChapterId_UserId", row => R.Array(row.G("ChapterId"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_ChapterReadTable).IndexCreate("BookId_UserId", row => R.Array(row.G("BookId"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_ChapterReadTable).IndexCreate("ChapterId").RunResult(_conn).AssertNoErrors();
                R.Table(s_ChapterReadTable).IndexCreate("UserId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_ChapterReadTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_ChapterReadTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测章阅读是否存在

        #endregion

        #region IChapterReadRepository 接口实现

        /// <inheritdoc />
        public ChapterRead GetChapterRead(string readId)
        {
            return R.Table(s_ChapterReadTable).Get(readId).RunResult<ChapterRead>(_conn);
        }

        /// <inheritdoc />
        public Task<ChapterRead> GetChapterReadAsync(string readId)
        {
            return R.Table(s_ChapterReadTable).Get(readId).RunResultAsync<ChapterRead>(_conn);
        }

        /// <inheritdoc />
        public List<ChapterRead> FindChapterReadsByChapter(string chapterId, int? userId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterReadTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_ChapterReadTable).GetAll(R.Array(chapterId, userId)).OptArg("index", "ChapterId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<ChapterRead>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<ChapterRead>> FindChapterReadsByChapterAsync(string chapterId, int? userId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterReadTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_ChapterReadTable).GetAll(R.Array(chapterId, userId)).OptArg("index", "ChapterId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<ChapterRead>>(_conn);
        }

        /// <inheritdoc />
        public List<ChapterRead> FindChapterReadsByUser(int userId, string bookId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterReadTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!bookId.IsNullOrEmpty())
            {
                query = R.Table(s_ChapterReadTable).GetAll(R.Array(bookId, userId)).OptArg("index", "BookId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<ChapterRead>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<ChapterRead>> FindChapterReadsByUserAsync(int userId, string bookId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterReadTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!bookId.IsNullOrEmpty())
            {
                query = R.Table(s_ChapterReadTable).GetAll(R.Array(bookId, userId)).OptArg("index", "BookId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<ChapterRead>>(_conn);
        }

        /// <inheritdoc />
        public int GetChapterReadsCountByChapter(string chapterId, int? userId, DateTime? createdSince)
        {
            var query = R.Table(s_ChapterReadTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_ChapterReadTable).GetAll(R.Array(chapterId, userId)).OptArg("index", "ChapterId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetChapterReadsCountByChapterAsync(string chapterId, int? userId, DateTime? createdSince)
        {
            var query = R.Table(s_ChapterReadTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_ChapterReadTable).GetAll(R.Array(chapterId, userId)).OptArg("index", "ChapterId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetChapterReadsCountByUser(int userId, string bookId, DateTime? createdSince)
        {
            var query = R.Table(s_ChapterReadTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!bookId.IsNullOrEmpty())
            {
                query = R.Table(s_ChapterReadTable).GetAll(R.Array(bookId, userId)).OptArg("index", "BookId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetChapterReadsCountByUserAsync(int userId, string bookId, DateTime? createdSince)
        {
            var query = R.Table(s_ChapterReadTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!bookId.IsNullOrEmpty())
            {
                query = R.Table(s_ChapterReadTable).GetAll(R.Array(bookId, userId)).OptArg("index", "BookId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public ChapterRead CreateChapterRead(ChapterRead newChapterRead)
        {
            newChapterRead.ThrowIfNull(nameof(newChapterRead));
            newChapterRead.Id = newChapterRead.Id.IsNullOrEmpty() ? new Base36IdGenerator(12, 5, 7).NewId().ToLower() : newChapterRead.Id;
            newChapterRead.CreatedDate = DateTime.UtcNow;
            var result = R.Table(s_ChapterReadTable).Get(newChapterRead.Id).Replace(newChapterRead).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<ChapterRead>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<ChapterRead> CreateChapterReadAsync(ChapterRead newChapterRead)
        {
            newChapterRead.ThrowIfNull(nameof(newChapterRead));
            newChapterRead.Id = newChapterRead.Id.IsNullOrEmpty() ? new Base36IdGenerator(12, 5, 7).NewId().ToLower() : newChapterRead.Id;
            newChapterRead.CreatedDate = DateTime.UtcNow;
            var result = (await R.Table(s_ChapterReadTable).Get(newChapterRead.Id).Replace(newChapterRead).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<ChapterRead>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteChapterRead(string readId)
        {
            R.Table(s_ChapterReadTable).Get(readId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteChapterReadAsync(string readId)
        {
            (await R.Table(s_ChapterReadTable).Get(readId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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