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
    ///     基于RethinkDb的推荐的存储库。
    /// </summary>
    public class RethinkDbRecommendationRepository : IRecommendationRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbRecommendationRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     推荐的数据表名。
        /// </summary>
        private static readonly string s_RecommendationTable = typeof(Recommendation).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbRecommendationRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbRecommendationRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbRecommendationRepository).Name));
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
            if (tables.Contains(s_RecommendationTable))
            {
                R.TableDrop(s_RecommendationTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_RecommendationTable))
            {
                R.TableCreate(s_RecommendationTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                //R.Table(s_RecommendationTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_RecommendationTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测推荐是否存在

        #endregion

        #region IRecommendationRepository 接口实现

        /// <inheritdoc />
        public Recommendation GetRecommendation(string recommendationId)
        {
            return R.Table(s_RecommendationTable).Get(recommendationId).RunResult<Recommendation>(_conn);
        }

        /// <inheritdoc />
        public Task<Recommendation> GetRecommendationAsync(string recommendationId)
        {
            return R.Table(s_RecommendationTable).Get(recommendationId).RunResultAsync<Recommendation>(_conn);
        }

        /// <inheritdoc />
        public List<Recommendation> FindRecommendations(string contentType, DateTime? createdSince, DateTime? modifiedSince, int? position, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_RecommendationTable).Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (position.HasValue)
            {
                query = query.Filter(row => row.G("Position").Eq(position.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Recommendation>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Recommendation>> FindRecommendationsAsync(string contentType, DateTime? createdSince, DateTime? modifiedSince, int? position, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_RecommendationTable).Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (position.HasValue)
            {
                query = query.Filter(row => row.G("Position").Eq(position.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Recommendation>>(_conn);
        }

        /// <inheritdoc />
        public int GetRecommendationsCount(string contentType, DateTime? createdSince, DateTime? modifiedSince, int? position)
        {
            var query = R.Table(s_RecommendationTable).Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (position.HasValue)
            {
                query = query.Filter(row => row.G("Position").Eq(position.Value));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetRecommendationsCountAsync(string contentType, DateTime? createdSince, DateTime? modifiedSince, int? position)
        {
            var query = R.Table(s_RecommendationTable).Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (position.HasValue)
            {
                query = query.Filter(row => row.G("Position").Eq(position.Value));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public Recommendation CreateRecommendation(Recommendation newRecommendation)
        {
            newRecommendation.ThrowIfNull(nameof(newRecommendation));
            newRecommendation.Id = newRecommendation.Id.IsNullOrEmpty() ? new Base36IdGenerator().NewId().ToLower() : newRecommendation.Id;
            newRecommendation.CreatedDate = DateTime.UtcNow;
            newRecommendation.ModifiedDate = newRecommendation.CreatedDate;
            var result = R.Table(s_RecommendationTable).Get(newRecommendation.Id).Replace(newRecommendation).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Recommendation>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Recommendation> CreateRecommendationAsync(Recommendation newRecommendation)
        {
            newRecommendation.ThrowIfNull(nameof(newRecommendation));
            newRecommendation.Id = newRecommendation.Id.IsNullOrEmpty() ? new Base36IdGenerator().NewId().ToLower() : newRecommendation.Id;
            newRecommendation.CreatedDate = DateTime.UtcNow;
            newRecommendation.ModifiedDate = newRecommendation.CreatedDate;
            var result = (await R.Table(s_RecommendationTable).Get(newRecommendation.Id).Replace(newRecommendation).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Recommendation>()[0].NewValue;
        }

        /// <inheritdoc />
        public Recommendation UpdateRecommendation(Recommendation existingRecommendation, Recommendation newRecommendation)
        {
            existingRecommendation.ThrowIfNull(nameof(existingRecommendation));
            newRecommendation.Id = existingRecommendation.Id;
            newRecommendation.CreatedDate = existingRecommendation.CreatedDate;
            newRecommendation.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_RecommendationTable).Get(newRecommendation.Id).Replace(newRecommendation).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Recommendation>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Recommendation> UpdateRecommendationAsync(Recommendation existingRecommendation, Recommendation newRecommendation)
        {
            existingRecommendation.ThrowIfNull(nameof(existingRecommendation));
            newRecommendation.Id = existingRecommendation.Id;
            newRecommendation.CreatedDate = existingRecommendation.CreatedDate;
            newRecommendation.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_RecommendationTable).Get(newRecommendation.Id).Replace(newRecommendation).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Recommendation>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteRecommendation(string recommendationId)
        {
            R.Table(s_RecommendationTable).Get(recommendationId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteRecommendationAsync(string recommendationId)
        {
            (await R.Table(s_RecommendationTable).Get(recommendationId).Delete().RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void DeleteRecommendations(List<string> recommendationIds)
        {
            R.Table(s_RecommendationTable).GetAll(R.Args(recommendationIds.ToArray())).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteRecommendationsAsync(List<string> recommendationIds)
        {
            (await R.Table(s_RecommendationTable).GetAll(R.Args(recommendationIds.ToArray())).Delete().RunResultAsync(_conn)).AssertNoErrors();
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