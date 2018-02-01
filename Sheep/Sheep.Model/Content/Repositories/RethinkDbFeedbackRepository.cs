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
    ///     基于RethinkDb的反馈的存储库。
    /// </summary>
    public class RethinkDbFeedbackRepository : IFeedbackRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbFeedbackRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     反馈的数据表名。
        /// </summary>
        private static readonly string s_FeedbackTable = typeof(Feedback).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbFeedbackRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbFeedbackRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbFeedbackRepository).Name));
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
            if (tables.Contains(s_FeedbackTable))
            {
                R.TableDrop(s_FeedbackTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_FeedbackTable))
            {
                R.TableCreate(s_FeedbackTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_FeedbackTable).IndexCreate("UserId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_FeedbackTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_FeedbackTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测反馈是否存在

        #endregion

        #region IFeedbackRepository 接口实现

        /// <inheritdoc />
        public Feedback GetFeedback(string feedbackId)
        {
            return R.Table(s_FeedbackTable).Get(feedbackId).RunResult<Feedback>(_conn);
        }

        /// <inheritdoc />
        public Task<Feedback> GetFeedbackAsync(string feedbackId)
        {
            return R.Table(s_FeedbackTable).Get(feedbackId).RunResultAsync<Feedback>(_conn);
        }

        /// <inheritdoc />
        public List<Feedback> FindFeedbacks(string contentFilter, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_FeedbackTable).Filter(true);
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Feedback>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Feedback>> FindFeedbacksAsync(string contentFilter, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_FeedbackTable).Filter(true);
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Feedback>>(_conn);
        }

        /// <inheritdoc />
        public List<Feedback> FindFeedbacksByUser(int userId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_FeedbackTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Feedback>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Feedback>> FindFeedbacksByUserAsync(int userId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_FeedbackTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Feedback>>(_conn);
        }

        /// <inheritdoc />
        public int GetFeedbacksCount(string contentFilter, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_FeedbackTable).Filter(true);
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetFeedbacksCountAsync(string contentFilter, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_FeedbackTable).Filter(true);
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetFeedbacksCountByUser(int userId, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_FeedbackTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetFeedbacksCountByUserAsync(int userId, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_FeedbackTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public float CalculateUserFeedbacksScore(int userId, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var count = GetFeedbacksCountByUser(userId, createdSince, modifiedSince, status);
            return Math.Min(1.0f, count / 10.0f);
        }

        /// <inheritdoc />
        public async Task<float> CalculateUserFeedbacksScoreAsync(int userId, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var count = await GetFeedbacksCountByUserAsync(userId, createdSince, modifiedSince, status);
            return Math.Min(1.0f, count / 10.0f);
        }

        /// <inheritdoc />
        public Feedback CreateFeedback(Feedback newFeedback)
        {
            newFeedback.ThrowIfNull(nameof(newFeedback));
            newFeedback.Id = newFeedback.Id.IsNullOrEmpty() ? new Base36IdGenerator().NewId().ToLower() : newFeedback.Id;
            newFeedback.Status = "待处理";
            newFeedback.CreatedDate = DateTime.UtcNow;
            newFeedback.ModifiedDate = newFeedback.CreatedDate;
            var result = R.Table(s_FeedbackTable).Get(newFeedback.Id).Replace(newFeedback).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Feedback>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Feedback> CreateFeedbackAsync(Feedback newFeedback)
        {
            newFeedback.ThrowIfNull(nameof(newFeedback));
            newFeedback.Id = newFeedback.Id.IsNullOrEmpty() ? new Base36IdGenerator().NewId().ToLower() : newFeedback.Id;
            newFeedback.Status = "待处理";
            newFeedback.CreatedDate = DateTime.UtcNow;
            newFeedback.ModifiedDate = newFeedback.CreatedDate;
            var result = (await R.Table(s_FeedbackTable).Get(newFeedback.Id).Replace(newFeedback).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Feedback>()[0].NewValue;
        }

        /// <inheritdoc />
        public Feedback UpdateFeedback(Feedback existingFeedback, Feedback newFeedback)
        {
            existingFeedback.ThrowIfNull(nameof(existingFeedback));
            newFeedback.Id = existingFeedback.Id;
            newFeedback.CreatedDate = existingFeedback.CreatedDate;
            newFeedback.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_FeedbackTable).Get(newFeedback.Id).Replace(newFeedback).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Feedback>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Feedback> UpdateFeedbackAsync(Feedback existingFeedback, Feedback newFeedback)
        {
            existingFeedback.ThrowIfNull(nameof(existingFeedback));
            newFeedback.Id = existingFeedback.Id;
            newFeedback.CreatedDate = existingFeedback.CreatedDate;
            newFeedback.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_FeedbackTable).Get(newFeedback.Id).Replace(newFeedback).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Feedback>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteFeedback(string feedbackId)
        {
            R.Table(s_FeedbackTable).Get(feedbackId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteFeedbackAsync(string feedbackId)
        {
            (await R.Table(s_FeedbackTable).Get(feedbackId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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