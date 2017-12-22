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
    ///     基于RethinkDb的评论的存储库。
    /// </summary>
    public class RethinkDbCommentRepository : ICommentRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbCommentRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     评论的数据表名。
        /// </summary>
        private static readonly string s_CommentTable = typeof(Comment).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbCommentRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbCommentRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbCommentRepository).Name));
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
            if (tables.Contains(s_CommentTable))
            {
                R.TableDrop(s_CommentTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_CommentTable))
            {
                R.TableCreate(s_CommentTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_CommentTable).IndexCreate("ParentId_UserId", row => R.Array(row.G("ParentId"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_CommentTable).IndexCreate("ParentId").RunResult(_conn).AssertNoErrors();
                R.Table(s_CommentTable).IndexCreate("UserId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_CommentTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_CommentTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测评论是否存在

        #endregion

        #region ICommentRepository 接口实现

        /// <inheritdoc />
        public Comment GetComment(string commentId)
        {
            return R.Table(s_CommentTable).Get(commentId).RunResult<Comment>(_conn);
        }

        /// <inheritdoc />
        public Task<Comment> GetCommentAsync(string commentId)
        {
            return R.Table(s_CommentTable).Get(commentId).RunResultAsync<Comment>(_conn);
        }

        /// <inheritdoc />
        public List<Comment> FindCommentsByParent(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_CommentTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_CommentTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Comment>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Comment>> FindCommentsByParentAsync(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_CommentTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_CommentTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Comment>>(_conn);
        }

        /// <inheritdoc />
        public List<Comment> FindCommentsByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_CommentTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
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
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Comment>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Comment>> FindCommentsByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_CommentTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
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
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Comment>>(_conn);
        }

        /// <inheritdoc />
        public int GetCommentsCountByParent(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status)
        {
            var query = R.Table(s_CommentTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_CommentTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetCommentsCountByParentAsync(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status)
        {
            var query = R.Table(s_CommentTable).GetAll(parentId).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_CommentTable).GetAll(R.Array(parentId, userId)).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public List<KeyValuePair<string, int>> GetCommentsCountByParents(List<string> parentIds, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status)
        {
            var query = R.Table(s_CommentTable).GetAll(R.Args(parentIds.ToArray())).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_CommentTable).GetAll(R.Args(parentIds.Select(parentId => R.Array(parentId, userId)).ToArray())).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (userId.HasValue)
            {
                query = query.Filter(row => row.G("UserId").Eq(userId.Value));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Group(row => row.G("ParentId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResult<List<KeyValuePair<string, int>>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<KeyValuePair<string, int>>> GetCommentsCountByParentsAsync(List<string> parentIds, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status)
        {
            var query = R.Table(s_CommentTable).GetAll(R.Args(parentIds.ToArray())).OptArg("index", "ParentId").Filter(true);
            if (userId.HasValue)
            {
                query = R.Table(s_CommentTable).GetAll(R.Args(parentIds.Select(parentId => R.Array(parentId, userId)).ToArray())).OptArg("index", "ParentId_UserId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Group(row => row.G("ParentId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResultAsync<List<KeyValuePair<string, int>>>(_conn);
        }

        /// <inheritdoc />
        public int GetCommentsCountByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status)
        {
            var query = R.Table(s_CommentTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
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
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetCommentsCountByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status)
        {
            var query = R.Table(s_CommentTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
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
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public Comment CreateComment(Comment newComment)
        {
            newComment.ThrowIfNull(nameof(newComment));
            newComment.Id = newComment.Id.IsNullOrEmpty() ? new Base36IdGenerator(11).NewId().ToLower() : newComment.Id;
            newComment.Status = "审核通过";
            newComment.CreatedDate = DateTime.UtcNow;
            newComment.ModifiedDate = newComment.CreatedDate;
            newComment.IsFeatured = false;
            newComment.RepliesCount = 0;
            newComment.VotesCount = 0;
            newComment.YesVotesCount = 0;
            newComment.NoVotesCount = 0;
            newComment.ContentQuality = 0;
            var result = R.Table(s_CommentTable).Get(newComment.Id).Replace(newComment).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Comment> CreateCommentAsync(Comment newComment)
        {
            newComment.ThrowIfNull(nameof(newComment));
            newComment.Id = newComment.Id.IsNullOrEmpty() ? new Base36IdGenerator(11).NewId().ToLower() : newComment.Id;
            newComment.Status = "审核通过";
            newComment.CreatedDate = DateTime.UtcNow;
            newComment.ModifiedDate = newComment.CreatedDate;
            newComment.IsFeatured = false;
            newComment.RepliesCount = 0;
            newComment.VotesCount = 0;
            newComment.YesVotesCount = 0;
            newComment.NoVotesCount = 0;
            newComment.ContentQuality = 0;
            var result = (await R.Table(s_CommentTable).Get(newComment.Id).Replace(newComment).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public Comment UpdateComment(Comment existingComment, Comment newComment)
        {
            existingComment.ThrowIfNull(nameof(existingComment));
            newComment.Id = existingComment.Id;
            newComment.CreatedDate = existingComment.CreatedDate;
            newComment.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_CommentTable).Get(newComment.Id).Replace(newComment).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Comment> UpdateCommentAsync(Comment existingComment, Comment newComment)
        {
            existingComment.ThrowIfNull(nameof(existingComment));
            newComment.Id = existingComment.Id;
            newComment.CreatedDate = existingComment.CreatedDate;
            newComment.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_CommentTable).Get(newComment.Id).Replace(newComment).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteComment(string commentId)
        {
            R.Table(s_CommentTable).Get(commentId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteCommentAsync(string commentId)
        {
            (await R.Table(s_CommentTable).Get(commentId).Delete().RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public Comment IncrementCommentRepliesCount(string commentId, int count)
        {
            var result = R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("RepliesCount", row.G("RepliesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Comment> IncrementCommentRepliesCountAsync(string commentId, int count)
        {
            var result = (await R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("RepliesCount", row.G("RepliesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public Comment IncrementCommentVotesCount(string commentId, int count)
        {
            var result = R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Comment> IncrementCommentVotesCountAsync(string commentId, int count)
        {
            var result = (await R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public Comment IncrementCommentYesVotesCount(string commentId, int count)
        {
            var result = R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("YesVotesCount", row.G("YesVotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Comment> IncrementCommentYesVotesCountAsync(string commentId, int count)
        {
            var result = (await R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("YesVotesCount", row.G("YesVotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public Comment IncrementCommentNoVotesCount(string commentId, int count)
        {
            var result = R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("NoVotesCount", row.G("NoVotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Comment> IncrementCommentNoVotesCountAsync(string commentId, int count)
        {
            var result = (await R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("NoVotesCount", row.G("NoVotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public Comment IncrementCommentVotesAndYesVotesCount(string commentId, int count)
        {
            var result = R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("YesVotesCount", row.G("YesVotesCount").Default_(0).Add(count)).With("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Comment> IncrementCommentVotesAndYesVotesCountAsync(string commentId, int count)
        {
            var result = (await R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("YesVotesCount", row.G("YesVotesCount").Default_(0).Add(count)).With("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public Comment IncrementCommentVotesAndNoVotesCount(string commentId, int count)
        {
            var result = R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("NoVotesCount", row.G("NoVotesCount").Default_(0).Add(count)).With("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Comment> IncrementCommentVotesAndNoVotesCountAsync(string commentId, int count)
        {
            var result = (await R.Table(s_CommentTable).Get(commentId).Update(row => R.HashMap("NoVotesCount", row.G("NoVotesCount").Default_(0).Add(count)).With("VotesCount", row.G("VotesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Comment>()[0].NewValue;
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