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
    ///     基于RethinkDb的帖子屏蔽的存储库。
    /// </summary>
    public class RethinkDbPostBlockRepository : IPostBlockRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbPostBlockRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     帖子屏蔽的数据表名。
        /// </summary>
        private static readonly string s_PostBlockTable = typeof(PostBlock).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbPostBlockRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbPostBlockRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.",
                                                                  typeof(RethinkDbPostBlockRepository).Name));
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
            if (tables.Contains(s_PostBlockTable))
            {
                R.TableDrop(s_PostBlockTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_PostBlockTable))
            {
                R.TableCreate(s_PostBlockTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_PostBlockTable).IndexCreate("PostId_BlockerId", row => R.Array(row.G("PostId"), row.G("BlockerId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_PostBlockTable).IndexCreate("PostId").RunResult(_conn).AssertNoErrors();
                R.Table(s_PostBlockTable).IndexCreate("BlockerId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_PostBlockTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_PostBlockTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测帖子屏蔽是否存在

        private void AssertNoExistingBlock(PostBlock newPostBlock, PostBlock exceptForExistingPostBlock = null)
        {
            var existingBlock = GetPostBlock(newPostBlock.PostId, newPostBlock.BlockerId);
            if (existingBlock != null && (exceptForExistingPostBlock == null || existingBlock.Id != exceptForExistingPostBlock.Id))
            {
                throw new ArgumentException(string.Format(Resources.PostWithBlockerAlreadyExists, newPostBlock.PostId, newPostBlock.BlockerId));
            }
        }

        private async Task AssertNoExistingBlockAsync(PostBlock newPostBlock, PostBlock exceptForExistingPostBlock = null)
        {
            var existingBlock = await GetPostBlockAsync(newPostBlock.PostId, newPostBlock.BlockerId);
            if (existingBlock != null && (exceptForExistingPostBlock == null || existingBlock.Id != exceptForExistingPostBlock.Id))
            {
                throw new ArgumentException(string.Format(Resources.PostWithBlockerAlreadyExists, newPostBlock.PostId, newPostBlock.BlockerId));
            }
        }

        #endregion

        #region IPostBlockRepository 接口实现

        /// <inheritdoc />
        public PostBlock GetPostBlock(string blockId)
        {
            return R.Table(s_PostBlockTable).Get(blockId).RunResult<PostBlock>(_conn);
        }

        /// <inheritdoc />
        public Task<PostBlock> GetPostBlockAsync(string blockId)
        {
            return R.Table(s_PostBlockTable).Get(blockId).RunResultAsync<PostBlock>(_conn);
        }

        /// <inheritdoc />
        public PostBlock GetPostBlock(string postId, int blockerId)
        {
            return R.Table(s_PostBlockTable).GetAll(R.Array(postId, blockerId)).OptArg("index", "PostId_BlockerId").Nth(0).Default_(default(PostBlock)).RunResult<PostBlock>(_conn);
        }

        /// <inheritdoc />
        public Task<PostBlock> GetPostBlockAsync(string postId, int blockerId)
        {
            return R.Table(s_PostBlockTable).GetAll(R.Array(postId, blockerId)).OptArg("index", "PostId_BlockerId").Nth(0).Default_(default(PostBlock)).RunResultAsync<PostBlock>(_conn);
        }

        /// <inheritdoc />
        public List<PostBlock> FindPostBlocks(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostBlockTable).Filter(true);
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
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<PostBlock>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<PostBlock>> FindPostBlocksAsync(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostBlockTable).Filter(true);
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
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<PostBlock>>(_conn);
        }

        /// <inheritdoc />
        public List<PostBlock> FindPostBlocksByPost(string postId, int? blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostBlockTable).GetAll(postId).OptArg("index", "PostId").Filter(true);
            if (blockerId.HasValue)
            {
                query = R.Table(s_PostBlockTable).GetAll(R.Array(postId, blockerId)).OptArg("index", "PostId_BlockerId").Filter(true);
            }
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
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<PostBlock>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<PostBlock>> FindPostBlocksByPostAsync(string postId, int? blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostBlockTable).GetAll(postId).OptArg("index", "PostId").Filter(true);
            if (blockerId.HasValue)
            {
                query = R.Table(s_PostBlockTable).GetAll(R.Array(postId, blockerId)).OptArg("index", "PostId_BlockerId").Filter(true);
            }
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
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<PostBlock>>(_conn);
        }

        /// <inheritdoc />
        public List<PostBlock> FindPostBlocksByBlocker(int blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostBlockTable).GetAll(blockerId).OptArg("index", "BlockerId").Filter(true);
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
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<PostBlock>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<PostBlock>> FindPostBlocksByBlockerAsync(int blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostBlockTable).GetAll(blockerId).OptArg("index", "BlockerId").Filter(true);
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
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<PostBlock>>(_conn);
        }

        /// <inheritdoc />
        public int GetPostBlocksCount(DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_PostBlockTable).Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetPostBlocksCountAsync(DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_PostBlockTable).Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetPostBlocksCountByPost(string postId, int? blockerId, DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_PostBlockTable).GetAll(postId).OptArg("index", "PostId").Filter(true);
            if (blockerId.HasValue)
            {
                query = R.Table(s_PostBlockTable).GetAll(R.Array(postId, blockerId)).OptArg("index", "PostId_BlockerId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetPostBlocksCountByPostAsync(string postId, int? blockerId, DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_PostBlockTable).GetAll(postId).OptArg("index", "PostId").Filter(true);
            if (blockerId.HasValue)
            {
                query = R.Table(s_PostBlockTable).GetAll(R.Array(postId, blockerId)).OptArg("index", "PostId_BlockerId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public List<KeyValuePair<string, int>> GetPostBlocksCountByPosts(List<string> postIds, int? blockerId, DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_PostBlockTable).GetAll(R.Args(postIds.ToArray())).OptArg("index", "PostId").Filter(true);
            if (blockerId.HasValue)
            {
                query = R.Table(s_PostBlockTable).GetAll(R.Args(postIds.Select(postId => R.Array(postId, blockerId)).ToArray())).OptArg("index", "PostId_BlockerId").Filter(true);
            }
            if (blockerId.HasValue)
            {
                query = query.Filter(row => row.G("BlockerId").Eq(blockerId.Value));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Group(row => row.G("PostId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResult<List<KeyValuePair<string, int>>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<KeyValuePair<string, int>>> GetPostBlocksCountByPostsAsync(List<string> postIds, int? blockerId, DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_PostBlockTable).GetAll(R.Args(postIds.ToArray())).OptArg("index", "PostId").Filter(true);
            if (blockerId.HasValue)
            {
                query = R.Table(s_PostBlockTable).GetAll(R.Args(postIds.Select(postId => R.Array(postId, blockerId)).ToArray())).OptArg("index", "PostId_BlockerId").Filter(true);
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Group(row => row.G("PostId")).Count().Ungroup().Map(row => R.HashMap("Key", row.G("group")).With("Value", row.G("reduction"))).RunResultAsync<List<KeyValuePair<string, int>>>(_conn);
        }

        /// <inheritdoc />
        public int GetPostBlocksCountByBlocker(int blockerId, DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_PostBlockTable).GetAll(blockerId).OptArg("index", "BlockerId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetPostBlocksCountByBlockerAsync(int blockerId, DateTime? createdSince, DateTime? modifiedSince)
        {
            var query = R.Table(s_PostBlockTable).GetAll(blockerId).OptArg("index", "BlockerId").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public float CalculateBlockerPostBlocksScore(int blockerId, DateTime? createdSince, DateTime? modifiedSince)
        {
            var count = GetPostBlocksCountByBlocker(blockerId, createdSince, modifiedSince);
            return Math.Min(1.0f, count / 20.0f);
        }

        /// <inheritdoc />
        public async Task<float> CalculateBlockerPostBlocksScoreAsync(int blockerId, DateTime? createdSince, DateTime? modifiedSince)
        {
            var count = await GetPostBlocksCountByBlockerAsync(blockerId, createdSince, modifiedSince);
            return Math.Min(1.0f, count / 20.0f);
        }

        /// <inheritdoc />
        public PostBlock CreatePostBlock(PostBlock newPostBlock)
        {
            newPostBlock.ThrowIfNull(nameof(newPostBlock));
            AssertNoExistingBlock(newPostBlock);
            newPostBlock.Id = string.Format("{0}-{1}", newPostBlock.PostId, newPostBlock.BlockerId);
            newPostBlock.CreatedDate = DateTime.UtcNow;
            newPostBlock.ModifiedDate = newPostBlock.CreatedDate;
            var result = R.Table(s_PostBlockTable).Get(newPostBlock.Id).Replace(newPostBlock).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<PostBlock>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<PostBlock> CreatePostBlockAsync(PostBlock newPostBlock)
        {
            newPostBlock.ThrowIfNull(nameof(newPostBlock));
            await AssertNoExistingBlockAsync(newPostBlock);
            newPostBlock.Id = string.Format("{0}-{1}", newPostBlock.PostId, newPostBlock.BlockerId);
            newPostBlock.CreatedDate = DateTime.UtcNow;
            newPostBlock.ModifiedDate = newPostBlock.CreatedDate;
            var result = (await R.Table(s_PostBlockTable).Get(newPostBlock.Id).Replace(newPostBlock).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<PostBlock>()[0].NewValue;
        }

        /// <inheritdoc />
        public PostBlock UpdatePostBlock(PostBlock existingPostBlock, PostBlock newPostBlock)
        {
            existingPostBlock.ThrowIfNull(nameof(existingPostBlock));
            AssertNoExistingBlock(newPostBlock, existingPostBlock);
            newPostBlock.Id = existingPostBlock.Id;
            newPostBlock.CreatedDate = existingPostBlock.CreatedDate;
            newPostBlock.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_PostBlockTable).Get(newPostBlock.Id).Replace(newPostBlock).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<PostBlock>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<PostBlock> UpdatePostBlockAsync(PostBlock existingPostBlock, PostBlock newPostBlock)
        {
            existingPostBlock.ThrowIfNull(nameof(existingPostBlock));
            await AssertNoExistingBlockAsync(newPostBlock, existingPostBlock);
            newPostBlock.Id = existingPostBlock.Id;
            newPostBlock.CreatedDate = existingPostBlock.CreatedDate;
            newPostBlock.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_PostBlockTable).Get(newPostBlock.Id).Replace(newPostBlock).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<PostBlock>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeletePostBlock(string blockId)
        {
            R.Table(s_PostBlockTable).Get(blockId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeletePostBlockAsync(string blockId)
        {
            (await R.Table(s_PostBlockTable).Get(blockId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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