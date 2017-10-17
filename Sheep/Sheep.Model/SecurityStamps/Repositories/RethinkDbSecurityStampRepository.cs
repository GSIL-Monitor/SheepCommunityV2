using System;
using System.Threading.Tasks;
using RethinkDb.Driver;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ServiceStack;
using ServiceStack.Logging;
using Sheep.Model.SecurityStamps.Entities;
using AsyncContext = Nito.AsyncEx.AsyncContext;

namespace Sheep.Model.SecurityStamps.Repositories
{
    /// <summary>
    ///     基于RethinkDb的安全戳存储库。
    /// </summary>
    public class RethinkDbSecurityStampRepository : ISecurityStampRepository
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbSecurityStampRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     安全戳的数据表名。
        /// </summary>
        private static readonly string s_SecurityStampTable = typeof(SecurityStamp).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbSecurityStampRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbSecurityStampRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbSecurityStampRepository).Name));
            }
        }

        #endregion

        #region 数据表检测及创建

        /// <summary>
        ///     删除并重新创建数据表。
        /// </summary>
        public void DropAndReCreateTables()
        {
            if (R.TableList().Contains(s_SecurityStampTable).RunResult<bool>(_conn))
            {
                R.TableDrop(s_SecurityStampTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            if (!R.TableList().Contains(s_SecurityStampTable).RunResult<bool>(_conn))
            {
                R.TableCreate(s_SecurityStampTable).OptArg("primaryKey", "Identifier").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            return R.TableList().Contains(s_SecurityStampTable).RunResult<bool>(_conn);
        }

        #endregion

        #region ISecurityStampRepository 接口实现

        /// <summary>
        ///     获取或创建一个安全戳。
        /// </summary>
        /// <param name="identifier">安全戳标识，表示手机号码或电子邮件地址。</param>
        /// <returns>安全戳对象。</returns>
        public SecurityStamp GetSecurityStampStamp(string identifier)
        {
            return AsyncContext.Run(() => GetSecurityStampAsync(identifier));
        }

        /// <summary>
        ///     获取或创建一个安全戳。
        /// </summary>
        /// <param name="identifier">安全戳标识，表示手机号码或电子邮件地址。</param>
        /// <returns>安全戳对象。</returns>
        public async Task<SecurityStamp> GetSecurityStampAsync(string identifier)
        {
            identifier.ThrowIfNullOrEmpty(nameof(identifier));
            var securityStamp = await R.Table(s_SecurityStampTable).Get(identifier).RunResultAsync<SecurityStamp>(_conn);
            if (securityStamp == null)
            {
                securityStamp = new SecurityStamp
                                {
                                    Identifier = identifier,
                                    Stamp = Guid.NewGuid().ToString("N")
                                };
                var insertResult = await R.Table(s_SecurityStampTable).Insert(securityStamp).RunResultAsync(_conn);
                insertResult.AssertNoErrors().AssertInserted(1);
            }
            return securityStamp;
        }

        #endregion
    }
}