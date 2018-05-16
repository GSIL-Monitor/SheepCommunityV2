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
using Sheep.Model.Friendship.Entities;
using Sheep.Model.Properties;

namespace Sheep.Model.Friendship.Repositories
{
    /// <summary>
    ///     基于RethinkDb的屏蔽的存储库。
    /// </summary>
    public class RethinkDbBlockRepository : IBlockRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbBlockRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     屏蔽的数据表名。
        /// </summary>
        private static readonly string s_BlockTable = typeof(Block).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbBlockRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbBlockRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                                                                  typeof(RethinkDbBlockRepository).Name));
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
            if (tables.Contains(s_BlockTable))
            {
                R.TableDrop(s_BlockTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_BlockTable))
            {
                R.TableCreate(s_BlockTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_BlockTable).IndexCreate("BlockeeId_BlockerId", row => R.Array(row.G("BlockeeId"), row.G("BlockerId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_BlockTable).IndexCreate("BlockeeId").RunResult(_conn).AssertNoErrors();
                R.Table(s_BlockTable).IndexCreate("BlockerId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_BlockTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_BlockTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测屏蔽是否存在

        private void AssertNoExistingBlock(Block newBlock, Block exceptForExistingBlock = null)
        {
            var existingBlock = GetBlock(newBlock.BlockeeId, newBlock.BlockerId);
            if (existingBlock != null && (exceptForExistingBlock == null || existingBlock.Id != exceptForExistingBlock.Id))
            {
                throw new ArgumentException(string.Format(Resources.BlockeeWithBlockerAlreadyExists, newBlock.BlockeeId, newBlock.BlockerId));
            }
        }

        private async Task AssertNoExistingBlockAsync(Block newBlock, Block exceptForExistingBlock = null)
        {
            var existingBlock = await GetBlockAsync(newBlock.BlockeeId, newBlock.BlockerId);
            if (existingBlock != null && (exceptForExistingBlock == null || existingBlock.Id != exceptForExistingBlock.Id))
            {
                throw new ArgumentException(string.Format(Resources.BlockeeWithBlockerAlreadyExists, newBlock.BlockeeId, newBlock.BlockerId));
            }
        }

        #endregion

        #region IBlockRepository 接口实现

        /// <inheritdoc />
        public Block GetBlock(int blockeeId, int blockerId)
        {
            return R.Table(s_BlockTable).GetAll(R.Array(blockeeId, blockerId)).OptArg("index", "BlockeeId_BlockerId").Nth(0).Default_(default(Block)).RunResult<Block>(_conn);
        }

        /// <inheritdoc />
        public Task<Block> GetBlockAsync(int blockeeId, int blockerId)
        {
            return R.Table(s_BlockTable).GetAll(R.Array(blockeeId, blockerId)).OptArg("index", "BlockeeId_BlockerId").Nth(0).Default_(default(Block)).RunResultAsync<Block>(_conn);
        }

        /// <inheritdoc />
        public List<Block> FindBlocksByBlockee(int blockeeId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_BlockTable).GetAll(blockeeId).OptArg("index", "BlockeeId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Block>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Block>> FindBlocksByBlockeeAsync(int blockeeId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_BlockTable).GetAll(blockeeId).OptArg("index", "BlockeeId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Block>>(_conn);
        }

        /// <inheritdoc />
        public List<Block> FindBlocksByBlocker(int blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_BlockTable).GetAll(blockerId).OptArg("index", "BlockerId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Block>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Block>> FindBlocksByBlockerAsync(int blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_BlockTable).GetAll(blockerId).OptArg("index", "BlockerId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Block>>(_conn);
        }

        /// <inheritdoc />
        public Block CreateBlock(Block newBlock)
        {
            newBlock.ThrowIfNull(nameof(newBlock));
            AssertNoExistingBlock(newBlock);
            newBlock.Id = string.Format("{0}-{1}", newBlock.BlockeeId, newBlock.BlockerId);
            newBlock.CreatedDate = DateTime.UtcNow;
            newBlock.ModifiedDate = newBlock.CreatedDate;
            var result = R.Table(s_BlockTable).Get(newBlock.Id).Replace(newBlock).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Block>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Block> CreateBlockAsync(Block newBlock)
        {
            newBlock.ThrowIfNull(nameof(newBlock));
            await AssertNoExistingBlockAsync(newBlock);
            newBlock.Id = string.Format("{0}-{1}", newBlock.BlockeeId, newBlock.BlockerId);
            newBlock.CreatedDate = DateTime.UtcNow;
            newBlock.ModifiedDate = newBlock.CreatedDate;
            var result = (await R.Table(s_BlockTable).Get(newBlock.Id).Replace(newBlock).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Block>()[0].NewValue;
        }

        /// <inheritdoc />
        public Block UpdateBlock(Block existingBlock, Block newBlock)
        {
            existingBlock.ThrowIfNull(nameof(existingBlock));
            AssertNoExistingBlock(newBlock, existingBlock);
            newBlock.Id = existingBlock.Id;
            newBlock.CreatedDate = existingBlock.CreatedDate;
            newBlock.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_BlockTable).Get(newBlock.Id).Replace(newBlock).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Block>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Block> UpdateBlockAsync(Block existingBlock, Block newBlock)
        {
            existingBlock.ThrowIfNull(nameof(existingBlock));
            await AssertNoExistingBlockAsync(newBlock, existingBlock);
            newBlock.Id = existingBlock.Id;
            newBlock.CreatedDate = existingBlock.CreatedDate;
            newBlock.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_BlockTable).Get(newBlock.Id).Replace(newBlock).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Block>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteBlock(int blockeeId, int blockerId)
        {
            R.Table(s_BlockTable).GetAll(R.Array(blockeeId, blockerId)).OptArg("index", "BlockeeId_BlockerId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteBlockAsync(int blockeeId, int blockerId)
        {
            (await R.Table(s_BlockTable).GetAll(R.Array(blockeeId, blockerId)).OptArg("index", "BlockeeId_BlockerId").Delete().RunResultAsync(_conn)).AssertNoErrors();
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