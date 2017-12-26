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
using Sheep.Model.Properties;
using Group = Sheep.Model.Friendship.Entities.Group;

namespace Sheep.Model.Friendship.Repositories
{
    /// <summary>
    ///     基于RethinkDb的群组的存储库。
    /// </summary>
    public class RethinkDbGroupRepository : IGroupRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbGroupRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     群组的数据表名。
        /// </summary>
        private static readonly string s_GroupTable = typeof(Group).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbGroupRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbGroupRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbGroupRepository).Name));
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
            if (tables.Contains(s_GroupTable))
            {
                R.TableDrop(s_GroupTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_GroupTable))
            {
                R.TableCreate(s_GroupTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_GroupTable).IndexCreate("DisplayName").RunResult(_conn).AssertNoErrors();
                //R.Table(s_GroupTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_GroupTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测群组是否存在

        private void AssertNoExistingGroup(Group newGroup, Group exceptForExistingGroup = null)
        {
            if (newGroup.DisplayName != null)
            {
                var existingGroup = GetGroupByDisplayName(newGroup.DisplayName);
                if (existingGroup != null && (exceptForExistingGroup == null || existingGroup.Id != exceptForExistingGroup.Id))
                {
                    throw new ArgumentException(string.Format(Resources.DisplayNameAlreadyExists, newGroup.DisplayName.SafeInput()));
                }
            }
        }

        private async Task AssertNoExistingGroupAsync(Group newGroup, Group exceptForExistingGroup = null)
        {
            if (newGroup.DisplayName != null)
            {
                var existingGroup = await GetGroupByDisplayNameAsync(newGroup.DisplayName);
                if (existingGroup != null && (exceptForExistingGroup == null || existingGroup.Id != exceptForExistingGroup.Id))
                {
                    throw new ArgumentException(string.Format(Resources.DisplayNameAlreadyExists, newGroup.DisplayName.SafeInput()));
                }
            }
        }

        #endregion

        #region IGroupRepository 接口实现

        /// <inheritdoc />
        public Group GetGroup(string groupId)
        {
            return R.Table(s_GroupTable).Get(groupId).RunResult<Group>(_conn);
        }

        /// <inheritdoc />
        public Task<Group> GetGroupAsync(string groupId)
        {
            return R.Table(s_GroupTable).Get(groupId).RunResultAsync<Group>(_conn);
        }

        /// <inheritdoc />
        public Group GetGroupByDisplayName(string displayName)
        {
            return R.Table(s_GroupTable).GetAll(displayName).OptArg("index", "DisplayName").Nth(0).Default_(default(Group)).RunResult<Group>(_conn);
        }

        /// <inheritdoc />
        public Task<Group> GetGroupByDisplayNameAsync(string displayName)
        {
            return R.Table(s_GroupTable).GetAll(displayName).OptArg("index", "DisplayName").Nth(0).Default_(default(Group)).RunResultAsync<Group>(_conn);
        }

        /// <inheritdoc />
        public List<Group> FindGroups(string nameFilter, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_GroupTable).Filter(true);
            if (!nameFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("DisplayName").Match(nameFilter));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Group>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Group>> FindGroupsAsync(string nameFilter, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_GroupTable).Filter(true);
            if (!nameFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("DisplayName").Match(nameFilter));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Group>>(_conn);
        }

        /// <inheritdoc />
        public Group CreateGroup(Group newGroup)
        {
            newGroup.ThrowIfNull(nameof(newGroup));
            newGroup.DisplayName.ThrowIfNullOrEmpty(nameof(newGroup.DisplayName));
            AssertNoExistingGroup(newGroup);
            newGroup.Id = newGroup.Id.IsNullOrEmpty() ? new Base36IdGenerator(10, 4, 4).NewId().ToLower() : newGroup.Id;
            newGroup.CreatedDate = DateTime.UtcNow;
            newGroup.ModifiedDate = newGroup.CreatedDate;
            var result = R.Table(s_GroupTable).Get(newGroup.Id).Replace(newGroup).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Group>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Group> CreateGroupAsync(Group newGroup)
        {
            newGroup.ThrowIfNull(nameof(newGroup));
            newGroup.DisplayName.ThrowIfNullOrEmpty(nameof(newGroup.DisplayName));
            await AssertNoExistingGroupAsync(newGroup);
            newGroup.Id = newGroup.Id.IsNullOrEmpty() ? new Base36IdGenerator(10, 4, 4).NewId().ToLower() : newGroup.Id;
            newGroup.CreatedDate = DateTime.UtcNow;
            newGroup.ModifiedDate = newGroup.CreatedDate;
            var result = (await R.Table(s_GroupTable).Get(newGroup.Id).Replace(newGroup).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Group>()[0].NewValue;
        }

        /// <inheritdoc />
        public Group UpdateGroup(Group existingGroup, Group newGroup)
        {
            existingGroup.ThrowIfNull(nameof(existingGroup));
            newGroup.ThrowIfNull(nameof(newGroup));
            newGroup.DisplayName.ThrowIfNullOrEmpty(nameof(newGroup.DisplayName));
            AssertNoExistingGroup(newGroup, existingGroup);
            newGroup.Id = existingGroup.Id;
            newGroup.CreatedDate = existingGroup.CreatedDate;
            newGroup.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_GroupTable).Get(newGroup.Id).Replace(newGroup).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Group>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Group> UpdateGroupAsync(Group existingGroup, Group newGroup)
        {
            existingGroup.ThrowIfNull(nameof(existingGroup));
            newGroup.ThrowIfNull(nameof(newGroup));
            newGroup.DisplayName.ThrowIfNullOrEmpty(nameof(newGroup.DisplayName));
            await AssertNoExistingGroupAsync(newGroup, existingGroup);
            newGroup.Id = existingGroup.Id;
            newGroup.CreatedDate = existingGroup.CreatedDate;
            newGroup.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_GroupTable).Get(newGroup.Id).Replace(newGroup).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Group>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteGroup(string groupId)
        {
            R.Table(s_GroupTable).Get(groupId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteGroupAsync(string groupId)
        {
            (await R.Table(s_GroupTable).Get(groupId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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