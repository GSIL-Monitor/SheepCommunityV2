using System;
using Aliyun.OSS;
using Funq;
using Netease.Nim;
using Polly;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Auth;
using ServiceStack.Authentication.RethinkDb;
using ServiceStack.Logging;
using ServiceStack.ProtoBuf;
using ServiceStack.Redis;
using ServiceStack.Validation;
using Sheep.Common.Settings;
using Sheep.Model.Auth.Events;
using Sheep.Model.Auth.Providers;
using Sheep.Model.Content;
using Sheep.Model.Content.Repositories;
using Sheep.Model.Corp;
using Sheep.Model.Corp.Repositories;
using Sheep.Model.Friendship;
using Sheep.Model.Friendship.Repositories;
using Sheep.Model.Geo;
using Sheep.Model.Geo.Repositories;
using Sheep.Model.Read;
using Sheep.Model.Read.Repositories;
using Sheep.Model.Security;
using Sheep.Model.Security.Providers;
using Sheep.Model.Security.Repositories;
using Sheep.ServiceInterface;
using Sheep.ServiceModel;
using Sina.Weibo;
using Tencent.Cos;
using Tencent.Weixin;
using Top.Api;
using CredentialsAuthProvider = Sheep.Model.Auth.Providers.CredentialsAuthProvider;

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
            : base("Sheep", typeof(ServiceInterfaceAssembly).Assembly)
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
            ConfigAliyunTopClient(container);
            // 配置阿里云对象存储客户端。
            ConfigAliyunOssClient(container);
            // 配置新浪微博客户端。
            ConfigSinaWeiboClient(container);
            // 配置腾讯微信客户端。
            ConfigTencentWeixinClient(container);
            // 配置腾讯云对象存储客户端。
            ConfigTencentCosClient(container);
            // 配置网易云通讯客户端。
            ConfigNetneaseNimClient(container);
            // 配置安全验证码提供程序。
            ConfigSecurityTokenProviders(container);
            // 配置身份验证功能。
            ConfigAuth(container);
            // 配置地理位置功能。
            ConfigureGeo(container);
            // 配置团体功能。
            ConfigureCorp(container);
            // 配置好友功能。
            ConfigureFriendship(container);
            // 配置内容功能。
            ConfigureContent(container);
            // 配置书籍功能。
            ConfigureBook(container);
            // 配置校验器。
            ConfigValidation(container);
            // 配置跨域访问功能。
            ConfigureCors();
            // 配置ProtoBuf序列化功能。
            ConfigureProtoBuf();
            // 配置Postman接口生成功能。
            ConfigurePostman();
            // 配置 Swagger 功能。
            ConfigSwagger();
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
                               var conn = R.Connection().Hostname(AppSettings.GetString(AppSettingsDbNames.RethinkDbHostName)).Port(AppSettings.GetString(AppSettingsDbNames.RethinkDbPort).ToInt()).Timeout(AppSettings.GetString(AppSettingsDbNames.RethinkDbTimeout).ToInt()).Db(AppSettings.GetString(AppSettingsDbNames.RethinkDbDatabase)).Connect();
                               conn.ConnectionError += (sender, ex) =>
                                                       {
                                                           Log.WarnFormat("Lost connection to RethinkDB: {0} Reconnecting...", ex.Message);
                                                           Policy.Handle<Exception>().WaitAndRetryForever(retryAttempts => TimeSpan.FromSeconds(Math.Min(retryAttempts, 10)), (exception, span) => Log.WarnFormat("Error occurs when trying to reconnect to RethinkDB: {0} Retrying...", ex.Message)).Execute(() => conn.Reconnect());
                                                       };
                               container.Register<IConnection>(conn);
                           });
        }

        ///// <summary>
        /////     配置 SQL Server 数据库。
        ///// </summary>
        //private void ConfigSqlServerDb(Container container)
        //{
        //    IOrmLiteDialectProvider dialectProvider;
        //    switch (AppSettings.GetString(AppSettingsDbNames.DbProvider))
        //    {
        //        case "SqlServer2012":
        //            dialectProvider = SqlServer2012Dialect.Provider;
        //            break;
        //        case "SqlServer2014":
        //            dialectProvider = SqlServer2014Dialect.Provider;
        //            break;
        //        case "SqlServer2016":
        //            dialectProvider = SqlServer2016Dialect.Provider;
        //            break;
        //        default:
        //            dialectProvider = SqlServerDialect.Provider;
        //            break;
        //    }
        //    container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(AppSettings.GetString(AppSettingsDbNames.DbConnectionString), dialectProvider));
        //}

        /// <summary>
        ///     配置 Redis 客户端管理器。
        /// </summary>
        private void ConfigRedis(Container container)
        {
            container.Register<IRedisClientsManager>(c => new PooledRedisClientManager(1, AppSettings.GetString(AppSettingsDbNames.RedisConnectionString)));
            container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
        }

        /// <summary>
        ///     配置阿里大于客户端。
        /// </summary>
        private void ConfigAliyunTopClient(Container container)
        {
            container.Register<ITopClient>(c => new DefaultTopClient(AppSettings.GetString(AppSettingsTopNames.TopUrl), AppSettings.GetString(AppSettingsTopNames.TopAppKey), AppSettings.GetString(AppSettingsTopNames.TopAppKeySecret)));
        }

        /// <summary>
        ///     配置阿里云对象存储客户端。
        /// </summary>
        private void ConfigAliyunOssClient(Container container)
        {
            container.Register<IOss>(c => new OssClient(AppSettings.GetString(AppSettingsOssNames.OssEndpoint), AppSettings.GetString(AppSettingsOssNames.OssAccessKeyId), AppSettings.GetString(AppSettingsOssNames.OssAccessKeySecret)));
        }

        /// <summary>
        ///     配置新浪微博客户端。
        /// </summary>
        private void ConfigSinaWeiboClient(Container container)
        {
            container.Register<IWeiboClient>(c => new WeiboClient
                                                  {
                                                      AppKey = AppSettings.GetString(AppSettingsWeiboNames.AppKey),
                                                      AppSecret = AppSettings.GetString(AppSettingsWeiboNames.AppSecret),
                                                      AccessTokenUrl = AppSettings.GetString(AppSettingsWeiboNames.AccessTokenUrl),
                                                      GetTokenUrl = AppSettings.GetString(AppSettingsWeiboNames.GetTokenUrl),
                                                      ShowUserUrl = AppSettings.GetString(AppSettingsWeiboNames.ShowUserUrl),
                                                      GetCountryUrl = AppSettings.GetString(AppSettingsWeiboNames.GetCountryUrl),
                                                      GetProvinceUrl = AppSettings.GetString(AppSettingsWeiboNames.GetProvinceUrl),
                                                      GetCityUrl = AppSettings.GetString(AppSettingsWeiboNames.GetCityUrl),
                                                      RedirectUrl = AppSettings.GetString(AppSettingsWeiboNames.RedirectUrl)
                                                  });
        }

        /// <summary>
        ///     配置腾讯微信客户端。
        /// </summary>
        private void ConfigTencentWeixinClient(Container container)
        {
            container.Register<IWeixinClient>(c => new WeixinClient
                                                   {
                                                       AppId = AppSettings.GetString(AppSettingsWeixinNames.AppId),
                                                       AppSecret = AppSettings.GetString(AppSettingsWeixinNames.AppSecret),
                                                       AccessTokenUrl = AppSettings.GetString(AppSettingsWeixinNames.AccessTokenUrl),
                                                       RefreshTokenUrl = AppSettings.GetString(AppSettingsWeixinNames.RefreshTokenUrl),
                                                       AuthenticateTokenUrl = AppSettings.GetString(AppSettingsWeixinNames.AuthenticateTokenUrl),
                                                       UserInfoUrl = AppSettings.GetString(AppSettingsWeixinNames.UserInfoUrl)
                                                   });
        }

        /// <summary>
        ///     配置腾讯云对象存储客户端。
        /// </summary>
        private void ConfigTencentCosClient(Container container)
        {
            container.Register<ICosClient>(c => new CosClient
                                                {
                                                    AppId = AppSettings.Get<int>(AppSettingsCosNames.AppId),
                                                    SecretId = AppSettings.GetString(AppSettingsCosNames.SecretId),
                                                    SecretKey = AppSettings.GetString(AppSettingsCosNames.SecretKey),
                                                    ApiUrl = AppSettings.GetString(AppSettingsCosNames.ApiUrl),
                                                    Bucket = AppSettings.GetString(AppSettingsCosNames.Bucket)
                                                });
        }

        /// <summary>
        ///     配置网易云通讯客户端。
        /// </summary>
        private void ConfigNetneaseNimClient(Container container)
        {
            container.Register<INimClient>(c => new NimClient
                                                {
                                                    AppKey = AppSettings.GetString(AppSettingsNeteaseNimNames.AppKey),
                                                    AppSecret = AppSettings.GetString(AppSettingsNeteaseNimNames.AppSecret),
                                                    UserCreateUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserCreateUrl),
                                                    UserUpdateUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserUpdateUrl),
                                                    UserBlockUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserBlockUrl),
                                                    UserUnBlockUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserUnBlockUrl),
                                                    UserUpdateInfoUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserUpdateInfoUrl),
                                                    UserGetInfosUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserGetInfosUrl),
                                                    UserSetSpecialRelationUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserSetSpecialRelationUrl),
                                                    FriendAddUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.FriendAddUrl),
                                                    FriendUpdateUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.FriendUpdateUrl),
                                                    FriendDeleteUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.FriendDeleteUrl),
                                                    MessageSendUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageSendUrl),
                                                    MessageSendBatchUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageSendBatchUrl),
                                                    MessageSendAttachUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageSendAttachUrl),
                                                    MessageSendBatchAttachUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageSendBatchAttachUrl),
                                                    MessageFileUploadUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageFileUploadUrl),
                                                    MessageRecallUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageRecallUrl),
                                                    TeamCreateUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamCreateUrl),
                                                    TeamAddMemberUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamAddMemberUrl),
                                                    TeamKickMemberUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamKickMemberUrl),
                                                    TeamRemoveUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamRemoveUrl),
                                                    TeamUpdateUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamUpdateUrl),
                                                    TeamQueryUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamQueryUrl),
                                                    TeamChangeOwnerUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamChangeOwnerUrl),
                                                    TeamAddManagerUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamAddManagerUrl),
                                                    TeamRemoveManagerUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamRemoveManagerUrl),
                                                    TeamGetJoinedUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamGetJoinedUrl),
                                                    TeamUpdateMemberNickUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamUpdateMemberNickUrl),
                                                    TeamUpdateMemberMuteUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamUpdateMemberMuteUrl),
                                                    TeamMuteMemberUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamMuteMemberUrl),
                                                    TeamLeaveMemberUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamLeaveMemberUrl),
                                                    TeamMuteUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamMuteUrl),
                                                    TeamGetMutedMembersUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamGetMutedMembersUrl)
                                                });
        }

        /// <summary>
        ///     配置安全验证码提供程序。
        /// </summary>
        private void ConfigSecurityTokenProviders(Container container)
        {
            container.Register<ISecurityStampRepository>(c => new RethinkDbSecurityStampRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<ISecurityTokenProvider>(c => new Rfc6238CodeMobileSecurityTokenProvider(c.Resolve<ITopClient>(), c.Resolve<ISecurityStampRepository>()));
        }

        /// <summary>
        ///     配置身份验证功能。
        /// </summary>
        private void ConfigAuth(Container container)
        {
            container.Register<IUserAuthRepository>(c => new RethinkDbAuthRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            var authEvents = new IAuthEvents[]
                             {
                                 new SystemAuthEvents(),
                                 new NeteaseImAuthEvents(container.Resolve<INimClient>())
                             };
            container.Register<IAuthEvents>(c => new MultiAuthEvents(authEvents));
            var authProviders = new IAuthProvider[]
                                {
                                    new CredentialsAuthProvider(AppSettings),
                                    new MobileAuthProvider(AppSettings, container.Resolve<ISecurityTokenProvider>()),
                                    new WeiboAuthProvider(AppSettings, container.Resolve<IWeiboClient>())
                                };
            var feature = new AuthFeature(() => new AuthUserSession(), authProviders)
                          {
                              IncludeAssignRoleServices = false,
                              IncludeAuthMetadataProvider = true,
                              IncludeRegistrationService = false,
                              ValidateUniqueUserNames = true,
                              ValidateUniqueEmails = true,
                              MaxLoginAttempts = 10,
                              SessionExpiry = TimeSpan.FromDays(7),
                              PermanentSessionExpiry = TimeSpan.FromDays(365),
                              DeleteSessionCookiesOnLogout = true,
                              GenerateNewSessionCookiesOnAuthentication = false,
                              SaveUserNamesInLowerCase = false
                          };
            Plugins.Add(feature);
        }

        /// <summary>
        ///     配置地理位置功能。
        /// </summary>
        private void ConfigureGeo(Container container)
        {
            container.Register<ICountryRepository>(c => new RethinkDbCountryRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<IStateRepository>(c => new RethinkDbStateRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<ICityRepository>(c => new RethinkDbCityRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
        }

        /// <summary>
        ///     配置团体功能。
        /// </summary>
        private void ConfigureCorp(Container container)
        {
            container.Register<IGroupRepository>(c => new RethinkDbGroupRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
        }

        /// <summary>
        ///     配置好友功能。
        /// </summary>
        private void ConfigureFriendship(Container container)
        {
            container.Register<IFollowRepository>(c => new RethinkDbFollowRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
        }

        /// <summary>
        ///     配置内容功能。
        /// </summary>
        private void ConfigureContent(Container container)
        {
            container.Register<IPostRepository>(c => new RethinkDbPostRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<ILikeRepository>(c => new RethinkDbLikeRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<ICommentRepository>(c => new RethinkDbCommentRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<IReplyRepository>(c => new RethinkDbReplyRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<IVoteRepository>(c => new RethinkDbVoteRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
        }

        /// <summary>
        ///     配置书籍功能。
        /// </summary>
        private void ConfigureBook(Container container)
        {
            container.Register<IBookRepository>(c => new RethinkDbBookRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<IVolumeRepository>(c => new RethinkDbVolumeRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<IVolumeAnnotationRepository>(c => new RethinkDbVolumeAnnotationRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<IChapterRepository>(c => new RethinkDbChapterRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<IChapterAnnotationRepository>(c => new RethinkDbChapterAnnotationRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<ISubjectRepository>(c => new RethinkDbSubjectRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
            container.Register<IParagraphRepository>(c => new RethinkDbParagraphRepository(c.Resolve<IConnection>(), AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true));
        }

        /// <summary>
        ///     配置校验器及校验功能。
        /// </summary>
        private void ConfigValidation(Container container)
        {
            container.RegisterValidators(ReuseScope.Default, typeof(ServiceModelAssembly).Assembly);
            var feature = new ValidationFeature
                          {
                              ScanAppHostAssemblies = false
                          };
            Plugins.Add(feature);
        }

        /// <summary>
        ///     配置跨域访问功能。
        /// </summary>
        private void ConfigureCors()
        {
            var corsFeature = new CorsFeature(allowedHeaders: "Content-Type,Accept,X-ss-opt,X-ss-pid,X-ss-id");
            Plugins.Add(corsFeature);
        }

        /// <summary>
        ///     配置ProtoBuf序列化功能。
        /// </summary>
        private void ConfigureProtoBuf()
        {
            var protoBufFormat = new ProtoBufFormat();
            Plugins.Add(protoBufFormat);
        }

        /// <summary>
        ///     配置Postman接口生成功能。
        /// </summary>
        private void ConfigurePostman()
        {
            var postmanFeature = new PostmanFeature();
            Plugins.Add(postmanFeature);
        }

        /// <summary>
        ///     配置请求日志功能。
        /// </summary>
        private void ConfigureRequestLogs()
        {
            var requestLogsFeature = new RequestLogsFeature
                                     {
                                         EnableErrorTracking = false,
                                         EnableResponseTracking = false
                                     };
            Plugins.Add(requestLogsFeature);
        }

        /// <summary>
        ///     配置 Swagger 功能。
        /// </summary>
        private void ConfigSwagger()
        {
            var feature = new SwaggerFeature
                          {
                              UseBootstrapTheme = false,
                              UseLowercaseUnderscoreModelPropertyNames = false
                          };
            Plugins.Add(feature);
        }

        #endregion
    }
}