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
    ///     基于RethinkDb的点赞的存储库。
    /// </summary>
    public class RethinkDbLikeRepository : ILikeRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbLikeRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     点赞的数据表名。
        /// </summary>
        private static readonly string s_LikeTable = typeof(Like).Name;

        /// <summary>
        ///     帖子的数据表名。
        /// </summary>
        private static readonly string s_PostTable = typeof(Post).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbLikeRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbLikeRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbLikeRepository).Name));
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
            if (tables.Contains(s_LikeTable))
            {
                R.TableDrop(s_LikeTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_LikeTable))
            {
                R.TableCreate(s_LikeTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_LikeTable).IndexCreate("ParentId_UserId", row => R.Array(row.G("ParentId"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_LikeTable).IndexCreate("ParentId").RunResult(_conn).AssertNoErrors();
                R.Table(s_LikeTable).IndexCreate("UserId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_LikeTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_LikeTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测点赞是否存在

        private void AssertNoExistingLike(Like newLike, Like exceptForExistingLike = null)
        {
            var existingLike = GetLike(newLike.ParentId, newLike.UserId);
            if (existingLike != null && (exceptForExistingLike == null || existingLike.Id != exceptForExistingLike.Id))
            {
                throw new ArgumentException(string.Format(Resources.ParentWithUserAlreadyExists, newLike.ParentId, newLike.UserId));
            }
        }

        private async Task AssertNoExistingLikeAsync(Like newLike, Like exceptForExistingLike = null)
        {
            var existingLike = await GetLikeAsync(newLike.ParentId, newLike.UserId);
            if (existingLike != null && (exceptForExistingLike == null || existingLike.Id != exceptForExistingLike.Id))
            {
                throw new ArgumentException(string.Format(Resources.ParentWithUserAlreadyExists, newLike.ParentId, newLike.UserId));
            }
        }

        #endregion

        #region ILikeRepository 接口实现

        /// <inheritdoc />
        public Like GetLike(string parentId, int userId)
        {
            return R.Table(s_LikeTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Nth(0).Default_(default(Like)).RunResult<Like>(_conn);
        }

        /// <inheritdoc />
        public Task<Like> GetLikeAsync(string parentId, int userId)
        {
            return R.Table(s_LikeTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Nth(0).Default_(default(Like)).RunResultAsync<Like>(_conn);
        }

        /// <inheritdoc />
        public List<Like> FindLikesByParent(string parentId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_LikeTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Like>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Like>> FindLikesByParentAsync(string parentId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_LikeTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Like>>(_conn);
        }

        /// <inheritdoc />
        public List<Like> FindLikesByPostsOfAuthor(int postAuthorId, List<int> userIds, int? skip, int? limit)
        {
            if (userIds != null)
            {
                return R.Do_(R.Table(s_PostTable).GetAll(postAuthorId).OptArg("index", "AuthorId").G("Id").CoerceTo("array"), postIds => R.Table(s_LikeTable).GetAll(R.Args(postIds)).OptArg("index", "ParentId").Filter(row => R.Expr(userIds.ToArray()).Contains(row.G("UserId"))).OrderBy(R.Desc("CreatedDate")).Skip(skip ?? 0).Limit(limit ?? 100000)).RunResult<List<Like>>(_conn);
            }
            return R.Do_(R.Table(s_PostTable).GetAll(postAuthorId).OptArg("index", "AuthorId").G("Id").CoerceTo("array"), postIds => R.Table(s_LikeTable).GetAll(R.Args(postIds)).OptArg("index", "ParentId").OrderBy(R.Desc("CreatedDate")).Skip(skip ?? 0).Limit(limit ?? 100000)).RunResult<List<Like>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Like>> FindLikesByPostsOfAuthorAsync(int postAuthorId, List<int> userIds, int? skip, int? limit)
        {
            if (userIds != null)
            {
                return R.Do_(R.Table(s_PostTable).GetAll(postAuthorId).OptArg("index", "AuthorId").G("Id").CoerceTo("array"), postIds => R.Table(s_LikeTable).GetAll(R.Args(postIds)).OptArg("index", "ParentId").Filter(row => R.Expr(userIds.ToArray()).Contains(row.G("UserId"))).OrderBy(R.Desc("CreatedDate")).Skip(skip ?? 0).Limit(limit ?? 100000)).RunResultAsync<List<Like>>(_conn);
            }
            return R.Do_(R.Table(s_PostTable).GetAll(postAuthorId).OptArg("index", "AuthorId").G("Id").CoerceTo("array"), postIds => R.Table(s_LikeTable).GetAll(R.Args(postIds)).OptArg("index", "ParentId").OrderBy(R.Desc("CreatedDate")).Skip(skip ?? 0).Limit(limit ?? 100000)).RunResultAsync<List<Like>>(_conn);
        }

        /// <inheritdoc />
        public List<Like> FindLikesByUser(int userId, string parentType, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_LikeTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Like>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Like>> FindLikesByUserAsync(int userId, string parentType, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_LikeTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Like>>(_conn);
        }

        /// <inheritdoc />
        public int GetLikesCountByParent(string parentId, DateTime? createdSince)
        {
            var query = R.Table(s_LikeTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetLikesCountByParentAsync(string parentId, DateTime? createdSince)
        {
            var query = R.Table(s_LikeTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetLikesCountByUser(int userId, string parentType, DateTime? createdSince)
        {
            var query = R.Table(s_LikeTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetLikesCountByUserAsync(int userId, string parentType, DateTime? createdSince)
        {
            var query = R.Table(s_LikeTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public float CalculateUserLikesScore(int userId, string parentType, DateTime? createdSince)
        {
            var count = GetLikesCountByUser(userId, parentType, createdSince);
            return Math.Min(1.0f, count / 50.0f);
        }

        /// <inheritdoc />
        public async Task<float> CalculateUserLikesScoreAsync(int userId, string parentType, DateTime? createdSince)
        {
            var count = await GetLikesCountByUserAsync(userId, parentType, createdSince);
            return Math.Min(1.0f, count / 50.0f);
        }

        /// <inheritdoc />
        public Like CreateLike(Like newLike)
        {
            newLike.ThrowIfNull(nameof(newLike));
            AssertNoExistingLike(newLike);
            newLike.Id = string.Format("{0}-{1}", newLike.ParentId, newLike.UserId);
            newLike.CreatedDate = DateTime.UtcNow;
            var result = R.Table(s_LikeTable).Get(newLike.Id).Replace(newLike).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Like>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Like> CreateLikeAsync(Like newLike)
        {
            newLike.ThrowIfNull(nameof(newLike));
            await AssertNoExistingLikeAsync(newLike);
            newLike.Id = string.Format("{0}-{1}", newLike.ParentId, newLike.UserId);
            newLike.CreatedDate = DateTime.UtcNow;
            var result = (await R.Table(s_LikeTable).Get(newLike.Id).Replace(newLike).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Like>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteLike(string parentId, int userId)
        {
            R.Table(s_LikeTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteLikeAsync(string parentId, int userId)
        {
            (await R.Table(s_LikeTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Delete().RunResultAsync(_conn)).AssertNoErrors();
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