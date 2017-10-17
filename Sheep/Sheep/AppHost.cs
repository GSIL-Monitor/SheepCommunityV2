using System;
using Funq;
using Polly;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.ProtoBuf;
using ServiceStack.Redis;
using ServiceStack.Validation;
using Sheep.ServiceInterface.Groups;
using Top.Api;

namespace Sheep
{
    public class AppHost : AppHostBase
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(AppHost));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="AppHost" />对象。
        ///     需要传入名称和Web服务实现所在的程序集。
        /// </summary>
        public AppHost()
            : base("Sheep", typeof(CreateGroupService).Assembly)
        {
        }

        #endregion

        #region 重写 ServiceStackHost

        /// <summary>
        ///     使用Funqlet提供的注册来配置指定的容器。
        ///     此方法应初始化您的Web服务类所使用的任何IoC资源。
        /// </summary>
        /// <param name="container">要注册的容器。</param>
        public override void Configure(Container container)
        {
            // 设置服务器主机的通用配置。
            var hostConfig = new HostConfig
                             {
                                 DebugMode = AppSettings.Get(AppSettingsHostNames.DebugMode, false),
                                 AdminAuthSecret = AppSettings.Get(AppSettingsHostNames.AdminAuthSecret, "Bosshong2012"),
                                 ApiVersion = "2.0.0"
                             };
            SetConfig(hostConfig);

            // 配置 RethinkDB 数据库。
            ConfigRethinkDb(container);
            //配置 Redis 客户端管理器。
            ConfigRedis(container);
            // 配置阿里大于客户端。
            ConfigAlibabaTopClient(container);
        }

        #endregion

        #region 配置

        /// <summary>
        ///     配置 RethinkDB 数据库。
        /// </summary>
        private void ConfigRethinkDb(Container container)
        {
            Policy.Handle<Exception>()
                  .WaitAndRetryForever(retryAttempts => TimeSpan.FromSeconds(Math.Min(retryAttempts, 10)), (exception, span) => Log.WarnFormat("Error occurs when trying to connect to RethinkDB: {0} Retrying...", exception.Message))
                  .Execute(() =>
                           {
                               var conn = R.Connection().Hostname(AppSettings.GetString(AppSettingsDbNames.RethinkDbHostName)).Port(AppSettingsDbNames.RethinkDbPort.ToInt()).Timeout(AppSettingsDbNames.RethinkDbTimeout.ToInt()).Db(AppSettingsDbNames.RethinkDbDatabase).Connect();
                               conn.ConnectionError += (sender, ex) =>
                                                       {
                                                           Log.WarnFormat("Lost connection to RethinkDB: {0} Reconnecting...", ex.Message);
                                                           Policy.Handle<Exception>().WaitAndRetryForever(retryAttempts => TimeSpan.FromSeconds(Math.Min(retryAttempts, 10)), (exception, span) => Log.WarnFormat("Error occurs when trying to reconnect to RethinkDB: {0} Retrying...", ex.Message)).Execute(() => conn.Reconnect());
                                                       };
                               container.Register<IConnection>(conn);
                           });
        }

        /// <summary>
        ///     配置 SQL Server 数据库。
        /// </summary>
        private void ConfigSqlServerDb(Container container)
        {
            IOrmLiteDialectProvider dialectProvider;
            switch (AppSettings.GetString(AppSettingsDbNames.DbProvider))
            {
                case "SqlServer2012":
                    dialectProvider = SqlServer2012Dialect.Provider;
                    break;
                case "SqlServer2014":
                    dialectProvider = SqlServer2014Dialect.Provider;
                    break;
                case "SqlServer2016":
                    dialectProvider = SqlServer2016Dialect.Provider;
                    break;
                default:
                    dialectProvider = SqlServerDialect.Provider;
                    break;
            }
            container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(AppSettings.GetString(AppSettingsDbNames.DbConnectionString), dialectProvider));
        }

        /// <summary>
        ///     配置 Redis 客户端管理器。
        /// </summary>
        private void ConfigRedis(Container container)
        {
            container.Register<IRedisClientsManager>(c => new PooledRedisClientManager(AppSettings.GetString(AppSettingsDbNames.RedisConnectionString)));
        }

        private void ConfigureMqService(Container container)
        {
            //var mqService = new RedisMqServer(container.Resolve<IRedisClientsManager>(), 2);
            //container.Register<IMessageService>(mqService);
            //container.Register<IMessageFactory>(mqService.MessageFactory);

            //mqService.RegisterHandler<CheckClassRoom>(ServiceController.ExecuteMessage);
            //mqService.RegisterHandler<StartCheckClassRoom>(ServiceController.ExecuteMessage);
            //mqService.RegisterHandler<StopCheckClassRoom>(ServiceController.ExecuteMessage);

            //mqService.Start();
        }

        /// <summary>
        ///     配置阿里大于客户端。
        /// </summary>
        private void ConfigAlibabaTopClient(Container container)
        {
            container.Register<ITopClient>(c => new DefaultTopClient(AppSettings.GetString(AppSettingsTopNames.TopUrl), AppSettings.GetString(AppSettingsTopNames.TopAppKey), AppSettings.GetString(AppSettingsTopNames.TopAppKeySecret)));
        }

        private void ConfigSecurity(Container container)
        {
        }

        private void ConfigureMembership(Container container)
        {
        }

        private void ConfigureAuth(Container container)
        {
        }

        private void ConfigureFriendship(Container container)
        {
        }

        private void ConfigureShop(Container container)
        {
        }

        private void ConfigureBlog(Container container)
        {
        }

        private void ConfigureBible(Container container)
        {
        }

        private void ConfigurePlugin(Container container)
        {
            //Plugins.Add(new CorsFeature("*", "GET,POST", "Content-Type", true));
            Plugins.Add(new ProtoBufFormat());
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new PostmanFeature());
            Plugins.Add(new RequestLogsFeature
                        {
                            EnableErrorTracking = false,
                            EnableResponseTracking = false
                        });
            Plugins.Add(new ValidationFeature());
        }

        #endregion
    }
}