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
    ///     基于RethinkDb的举报的存储库。
    /// </summary>
    public class RethinkDbAbuseReportRepository : IAbuseReportRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbAbuseReportRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     举报的数据表名。
        /// </summary>
        private static readonly string s_AbuseReportTable = typeof(AbuseReport).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbAbuseReportRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbAbuseReportRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbAbuseReportRepository).Name));
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
            if (tables.Contains(s_AbuseReportTable))
            {
                R.TableDrop(s_AbuseReportTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_AbuseReportTable))
            {
                R.TableCreate(s_AbuseReportTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_AbuseReportTable).IndexCreate("ParentId_UserId", row => R.Array(row.G("ParentId"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_AbuseReportTable).IndexCreate("ParentId").RunResult(_conn).AssertNoErrors();
                R.Table(s_AbuseReportTable).IndexCreate("UserId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_AbuseReportTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_AbuseReportTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测举报是否存在

        #endregion

        #region IAbuseReportRepository 接口实现

        /// <inheritdoc />
        public AbuseReport GetAbuseReport(string reportId)
        {
            return R.Table(s_AbuseReportTable).Get(reportId).RunResult<AbuseReport>(_conn);
        }

        /// <inheritdoc />
        public Task<AbuseReport> GetAbuseReportAsync(string reportId)
        {
            return R.Table(s_AbuseReportTable).Get(reportId).RunResultAsync<AbuseReport>(_conn);
        }

        /// <inheritdoc />
        public List<AbuseReport> FindAbuseReports(string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_AbuseReportTable).Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<AbuseReport>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<AbuseReport>> FindAbuseReportsAsync(string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_AbuseReportTable).Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<AbuseReport>>(_conn);
        }

        /// <inheritdoc />
        public List<AbuseReport> FindAbuseReportsByParent(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_AbuseReportTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_AbuseReportTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<AbuseReport>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<AbuseReport>> FindAbuseReportsByParentAsync(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_AbuseReportTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_AbuseReportTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<AbuseReport>>(_conn);
        }

        /// <inheritdoc />
        public List<AbuseReport> FindAbuseReportsByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_AbuseReportTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<AbuseReport>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<AbuseReport>> FindAbuseReportsByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_AbuseReportTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<AbuseReport>>(_conn);
        }

        /// <inheritdoc />
        public int GetAbuseReportsCount(string parentType, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_AbuseReportTable).Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetAbuseReportsCountAsync(string parentType, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_AbuseReportTable).Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetAbuseReportsCountByParent(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_AbuseReportTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_AbuseReportTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetAbuseReportsCountByParentAsync(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_AbuseReportTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_AbuseReportTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public List<KeyValuePair<string, int>> GetAbuseReportsCountByParents(List<string> parentIds, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_AbuseReportTable).GetAll(R.Args(parentIds.ToArray())).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_AbuseReportTable).GetAll(R.Args(parentIds.Select(parentId => R.Array(parentId, userId)).ToArray())).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (userId.HasValue)
            {
                query = query.Filter(row => row.G("UserId").Eq(userId.Value));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Group(row => row.G("ParentId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResult<List<KeyValuePair<string, int>>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<KeyValuePair<string, int>>> GetAbuseReportsCountByParentsAsync(List<string> parentIds, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_AbuseReportTable).GetAll(R.Args(parentIds.ToArray())).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_AbuseReportTable).GetAll(R.Args(parentIds.Select(parentId => R.Array(parentId, userId)).ToArray())).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Group(row => row.G("ParentId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResultAsync<List<KeyValuePair<string, int>>>(_conn);
        }

        /// <inheritdoc />
        public int GetAbuseReportsCountByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_AbuseReportTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetAbuseReportsCountByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_AbuseReportTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public float CalculateUserAbuseReportsScore(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var count = GetAbuseReportsCountByUser(userId, parentType, createdSince, modifiedSince, status);
            return Math.Min(1.0f, count / 20.0f);
        }

        /// <inheritdoc />
        public async Task<float> CalculateUserAbuseReportsScoreAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var count = await GetAbuseReportsCountByUserAsync(userId, parentType, createdSince, modifiedSince, status);
            return Math.Min(1.0f, count / 20.0f);
        }

        /// <inheritdoc />
        public AbuseReport CreateAbuseReport(AbuseReport newAbuseReport)
        {
            newAbuseReport.ThrowIfNull(nameof(newAbuseReport));
            newAbuseReport.Id = newAbuseReport.Id.IsNullOrEmpty() ? new Base36IdGenerator().NewId().ToLower() : newAbuseReport.Id;
            newAbuseReport.Status = "待处理";
            newAbuseReport.CreatedDate = DateTime.UtcNow;
            newAbuseReport.ModifiedDate = newAbuseReport.CreatedDate;
            var result = R.Table(s_AbuseReportTable).Get(newAbuseReport.Id).Replace(newAbuseReport).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<AbuseReport>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<AbuseReport> CreateAbuseReportAsync(AbuseReport newAbuseReport)
        {
            newAbuseReport.ThrowIfNull(nameof(newAbuseReport));
            newAbuseReport.Id = newAbuseReport.Id.IsNullOrEmpty() ? new Base36IdGenerator().NewId().ToLower() : newAbuseReport.Id;
            newAbuseReport.Status = "待处理";
            newAbuseReport.CreatedDate = DateTime.UtcNow;
            newAbuseReport.ModifiedDate = newAbuseReport.CreatedDate;
            var result = (await R.Table(s_AbuseReportTable).Get(newAbuseReport.Id).Replace(newAbuseReport).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<AbuseReport>()[0].NewValue;
        }

        /// <inheritdoc />
        public AbuseReport UpdateAbuseReport(AbuseReport existingAbuseReport, AbuseReport newAbuseReport)
        {
            existingAbuseReport.ThrowIfNull(nameof(existingAbuseReport));
            newAbuseReport.Id = existingAbuseReport.Id;
            newAbuseReport.CreatedDate = existingAbuseReport.CreatedDate;
            newAbuseReport.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_AbuseReportTable).Get(newAbuseReport.Id).Replace(newAbuseReport).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<AbuseReport>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<AbuseReport> UpdateAbuseReportAsync(AbuseReport existingAbuseReport, AbuseReport newAbuseReport)
        {
            existingAbuseReport.ThrowIfNull(nameof(existingAbuseReport));
            newAbuseReport.Id = existingAbuseReport.Id;
            newAbuseReport.CreatedDate = existingAbuseReport.CreatedDate;
            newAbuseReport.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_AbuseReportTable).Get(newAbuseReport.Id).Replace(newAbuseReport).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<AbuseReport>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteAbuseReport(string reportId)
        {
            R.Table(s_AbuseReportTable).Get(reportId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteAbuseReportAsync(string reportId)
        {
            (await R.Table(s_AbuseReportTable).Get(reportId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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