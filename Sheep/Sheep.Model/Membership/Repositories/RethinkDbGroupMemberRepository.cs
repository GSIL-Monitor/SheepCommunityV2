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
using Sheep.Model.Membership.Entities;
using Sheep.Model.Properties;

namespace Sheep.Model.Membership.Repositories
{
    /// <summary>
    ///     基于RethinkDb的群主成员的存储库。
    /// </summary>
    public class RethinkDbGroupMemberRepository : IGroupMemberRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbGroupMemberRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     群主成员的数据表名。
        /// </summary>
        private static readonly string s_GroupMemberTable = typeof(GroupMember).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbGroupMemberRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbGroupMemberRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbGroupMemberRepository).Name));
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
            if (tables.Contains(s_GroupMemberTable))
            {
                R.TableDrop(s_GroupMemberTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_GroupMemberTable))
            {
                R.TableCreate(s_GroupMemberTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_GroupMemberTable).IndexCreate("GroupId_UserId", row => R.Array(row.G("GroupId"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_GroupMemberTable).IndexCreate("GroupId").RunResult(_conn).AssertNoErrors();
                R.Table(s_GroupMemberTable).IndexCreate("UserId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_GroupMemberTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_GroupMemberTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测群主成员是否存在

        private void AssertNoExistingGroupMember(GroupMember newGroupMember, GroupMember exceptForExistingGroupMember = null)
        {
            var existingGroupMember = GetGroupMember(newGroupMember.GroupId, newGroupMember.UserId);
            if (existingGroupMember != null && (exceptForExistingGroupMember == null || existingGroupMember.Id != exceptForExistingGroupMember.Id))
            {
                throw new ArgumentException(string.Format(Resources.GroupWithUserAlreadyExists, newGroupMember.GroupId, newGroupMember.UserId));
            }
        }

        private async Task AssertNoExistingGroupMemberAsync(GroupMember newGroupMember, GroupMember exceptForExistingGroupMember = null)
        {
            var existingGroupMember = await GetGroupMemberAsync(newGroupMember.GroupId, newGroupMember.UserId);
            if (existingGroupMember != null && (exceptForExistingGroupMember == null || existingGroupMember.Id != exceptForExistingGroupMember.Id))
            {
                throw new ArgumentException(string.Format(Resources.GroupWithUserAlreadyExists, newGroupMember.GroupId, newGroupMember.UserId));
            }
        }

        #endregion

        #region IGroupMemberRepository 接口实现

        /// <inheritdoc />
        public GroupMember GetGroupMember(string groupId, int userId)
        {
            return R.Table(s_GroupMemberTable).GetAll(R.Array(groupId, userId)).OptArg("index", "GroupId_UserId").Nth(0).Default_(default(GroupMember)).RunResult<GroupMember>(_conn);
        }

        /// <inheritdoc />
        public Task<GroupMember> GetGroupMemberAsync(string groupId, int userId)
        {
            return R.Table(s_GroupMemberTable).GetAll(R.Array(groupId, userId)).OptArg("index", "GroupId_UserId").Nth(0).Default_(default(GroupMember)).RunResultAsync<GroupMember>(_conn);
        }

        /// <inheritdoc />
        public List<GroupMember> FindGroupMembersByGroup(string groupId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_GroupMemberTable).GetAll(groupId).OptArg("index", "GroupId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("UserId")) : query.OrderBy("UserId");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<GroupMember>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<GroupMember>> FindGroupMembersByGroupAsync(string groupId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_GroupMemberTable).GetAll(groupId).OptArg("index", "GroupId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("UserId")) : query.OrderBy("UserId");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<GroupMember>>(_conn);
        }

        /// <inheritdoc />
        public List<GroupMember> FindGroupMembersByUser(int userId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_GroupMemberTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("UserId")) : query.OrderBy("UserId");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<GroupMember>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<GroupMember>> FindGroupMembersByUserAsync(int userId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_GroupMemberTable).GetAll(userId).OptArg("index", "UserId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("UserId")) : query.OrderBy("UserId");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<GroupMember>>(_conn);
        }

        /// <inheritdoc />
        public GroupMember CreateGroupMember(GroupMember newGroupMember)
        {
            newGroupMember.ThrowIfNull(nameof(newGroupMember));
            AssertNoExistingGroupMember(newGroupMember);
            newGroupMember.Id = string.Format("{0}-{1}", newGroupMember.GroupId, newGroupMember.UserId);
            var result = R.Table(s_GroupMemberTable).Get(newGroupMember.Id).Replace(newGroupMember).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<GroupMember>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<GroupMember> CreateGroupMemberAsync(GroupMember newGroupMember)
        {
            newGroupMember.ThrowIfNull(nameof(newGroupMember));
            await AssertNoExistingGroupMemberAsync(newGroupMember);
            newGroupMember.Id = string.Format("{0}-{1}", newGroupMember.GroupId, newGroupMember.UserId);
            var result = (await R.Table(s_GroupMemberTable).Get(newGroupMember.Id).Replace(newGroupMember).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<GroupMember>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteGroupMember(string groupId, int userId)
        {
            R.Table(s_GroupMemberTable).GetAll(R.Array(groupId, userId)).OptArg("index", "GroupId_UserId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteGroupMemberAsync(string groupId, int userId)
        {
            (await R.Table(s_GroupMemberTable).GetAll(R.Array(groupId, userId)).OptArg("index", "GroupId_UserId").Delete().RunResultAsync(_conn)).AssertNoErrors();
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