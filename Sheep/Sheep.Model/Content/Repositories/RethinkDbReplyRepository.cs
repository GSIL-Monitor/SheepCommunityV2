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
    ///     基于RethinkDb的回复的存储库。
    /// </summary>
    public class RethinkDbReplyRepository : IReplyRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbReplyRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     回复的数据表名。
        /// </summary>
        private static readonly string s_ReplyTable = typeof(Reply).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbReplyRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbReplyRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbReplyRepository).Name));
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
            if (tables.Contains(s_ReplyTable))
            {
                R.TableDrop(s_ReplyTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_ReplyTable))
            {
                R.TableCreate(s_ReplyTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_ReplyTable).IndexCreate("ParentId_UserId", row => R.Array(row.G("ParentId"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_ReplyTable).IndexCreate("ParentId").RunResult(_conn).AssertNoErrors();
                R.Table(s_ReplyTable).IndexCreate("UserId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_ReplyTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_ReplyTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测回复是否存在

        #endregion

        #region IReplyRepository 接口实现

        /// <inheritdoc />
        public Reply GetReply(string replyId)
        {
            return R.Table(s_ReplyTable).Get(replyId).RunResult<Reply>(_conn);
        }

        /// <inheritdoc />
        public Task<Reply> GetReplyAsync(string replyId)
        {
            return R.Table(s_ReplyTable).Get(replyId).RunResultAsync<Reply>(_conn);
        }

        /// <inheritdoc />
        public List<Reply> FindRepliesByParent(string parentId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ReplyTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Reply>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Reply>> FindRepliesByParentAsync(string parentId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ReplyTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Reply>>(_conn);
        }

        /// <inheritdoc />
        public List<Reply> FindRepliesByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ReplyTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Reply>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Reply>> FindRepliesByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ReplyTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Reply>>(_conn);
        }

        /// <inheritdoc />
        public int GetRepliesCountByParent(string parentId, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_ReplyTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetRepliesCountByParentAsync(string parentId, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_ReplyTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetRepliesCountByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_ReplyTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetRepliesCountByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status)
        {
            var query = R.Table(s_ReplyTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            if (!parentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ParentType").Eq(parentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public Reply CreateReply(Reply newReply)
        {
            newReply.ThrowIfNull(nameof(newReply));
            newReply.Id = newReply.Id.IsNullOrEmpty() ? new Base36IdGenerator(11).NewId().ToLower() : newReply.Id;
            newReply.Status = "审核通过";
            newReply.CreatedDate = DateTime.UtcNow;
            newReply.ModifiedDate = newReply.CreatedDate;
            newReply.VotesCount = 0;
            newReply.YesVotesCount = 0;
            newReply.NoVotesCount = 0;
            newReply.ContentQuality = 0;
            var result = R.Table(s_ReplyTable).Get(newReply.Id).Replace(newReply).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Reply> CreateReplyAsync(Reply newReply)
        {
            newReply.ThrowIfNull(nameof(newReply));
            newReply.Id = newReply.Id.IsNullOrEmpty() ? new Base36IdGenerator(11).NewId().ToLower() : newReply.Id;
            newReply.Status = "审核通过";
            newReply.CreatedDate = DateTime.UtcNow;
            newReply.ModifiedDate = newReply.CreatedDate;
            newReply.VotesCount = 0;
            newReply.YesVotesCount = 0;
            newReply.NoVotesCount = 0;
            newReply.ContentQuality = 0;
            var result = (await R.Table(s_ReplyTable).Get(newReply.Id).Replace(newReply).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public Reply UpdateReply(Reply existingReply, Reply newReply)
        {
            existingReply.ThrowIfNull(nameof(existingReply));
            newReply.Id = existingReply.Id;
            newReply.CreatedDate = existingReply.CreatedDate;
            newReply.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_ReplyTable).Get(newReply.Id).Replace(newReply).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Reply> UpdateReplyAsync(Reply existingReply, Reply newReply)
        {
            existingReply.ThrowIfNull(nameof(existingReply));
            newReply.Id = existingReply.Id;
            newReply.CreatedDate = existingReply.CreatedDate;
            newReply.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_ReplyTable).Get(newReply.Id).Replace(newReply).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteReply(string replyId)
        {
            R.Table(s_ReplyTable).Get(replyId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteReplyAsync(string replyId)
        {
            (await R.Table(s_ReplyTable).Get(replyId).Delete().RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public Reply IncrementReplyVotesCount(string replyId, int count)
        {
            var result = R.Table(s_ReplyTable).Get(replyId).Update(row => R.HashMap("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Reply> IncrementReplyVotesCountAsync(string replyId, int count)
        {
            var result = (await R.Table(s_ReplyTable).Get(replyId).Update(row => R.HashMap("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public Reply IncrementReplyYesVotesCount(string replyId, int count)
        {
            var result = R.Table(s_ReplyTable).Get(replyId).Update(row => R.HashMap("YesVotesCount", row.G("YesVotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Reply> IncrementReplyYesVotesCountAsync(string replyId, int count)
        {
            var result = (await R.Table(s_ReplyTable).Get(replyId).Update(row => R.HashMap("YesVotesCount", row.G("YesVotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public Reply IncrementReplyNoVotesCount(string replyId, int count)
        {
            var result = R.Table(s_ReplyTable).Get(replyId).Update(row => R.HashMap("NoVotesCount", row.G("NoVotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Reply> IncrementReplyNoVotesCountAsync(string replyId, int count)
        {
            var result = (await R.Table(s_ReplyTable).Get(replyId).Update(row => R.HashMap("NoVotesCount", row.G("NoVotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public Reply IncrementReplyVotesAndYesVotesCount(string replyId, int count)
        {
            var result = R.Table(s_ReplyTable).Get(replyId).Update(row => R.HashMap("YesVotesCount", row.G("YesVotesCount").Default_(0).Add(count)).With("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Reply> IncrementReplyVotesAndYesVotesCountAsync(string replyId, int count)
        {
            var result = (await R.Table(s_ReplyTable).Get(replyId).Update(row => R.HashMap("YesVotesCount", row.G("YesVotesCount").Default_(0).Add(count)).With("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public Reply IncrementReplyVotesAndNoVotesCount(string replyId, int count)
        {
            var result = R.Table(s_ReplyTable).Get(replyId).Update(row => R.HashMap("NoVotesCount", row.G("NoVotesCount").Default_(0).Add(count)).With("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Reply> IncrementReplyVotesAndNoVotesCountAsync(string replyId, int count)
        {
            var result = (await R.Table(s_ReplyTable).Get(replyId).Update(row => R.HashMap("NoVotesCount", row.G("NoVotesCount").Default_(0).Add(count)).With("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Reply>()[0].NewValue;
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