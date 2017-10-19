using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using Sheep.Model.Security.Entities;
using AsyncContext = Nito.AsyncEx.AsyncContext;

namespace Sheep.Model.Security.Repositories
{
    /// <summary>
    ///     基于OrmLite的安全戳存储库。
    /// </summary>
    public class OrmLiteSecurityStampRepository : ISecurityStampRepository, IRequiresSchema
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(OrmLiteSecurityStampRepository));

        #endregion

        #region 属性

        private readonly IDbConnectionFactory _dbFactory;

        /// <summary>
        ///     是否已初始化数据库结构。
        /// </summary>
        public bool HasInitSchema { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="OrmLiteSecurityStampRepository" />对象。
        /// </summary>
        /// <param name="dbFactory">数据库工厂。</param>
        public OrmLiteSecurityStampRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        #endregion

        #region 重新创建数据表

        /// <summary>
        ///     删除并重新创建数据表。
        /// </summary>
        public void DropAndReCreateTables()
        {
            HasInitSchema = true;
            using (var db = _dbFactory.OpenDbConnection())
            {
                db.DropAndCreateTable<SecurityStamp>();
            }
        }

        #endregion

        #region IRequiresSchema 接口实现

        /// <summary>
        ///     使用统一API创建缺少的表，数据结构或执行在启动时运行所需的任何其他的 任务依赖关系。
        /// </summary>
        public void InitSchema()
        {
            HasInitSchema = true;
            using (var db = _dbFactory.OpenDbConnection())
            {
                db.CreateTableIfNotExists<SecurityStamp>();
            }
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
            using (var db = _dbFactory.OpenDbConnection())
            {
                var securityStamp = await db.SingleAsync<SecurityStamp>(st => st.Identifier.ToLower() == identifier.ToLower());
                if (securityStamp == null)
                {
                    securityStamp = new SecurityStamp
                                    {
                                        Identifier = identifier,
                                        Stamp = Guid.NewGuid().ToString("N")
                                    };
                    await db.SaveAsync(securityStamp);
                }
                return securityStamp;
            }
        }

        #endregion
    }
}