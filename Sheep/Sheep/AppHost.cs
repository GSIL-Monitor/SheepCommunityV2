using Funq;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.ProtoBuf;
using ServiceStack.Validation;
using Sheep.ServiceInterface.Groups;

namespace Sheep
{
    public class AppHost : AppHostBase
    {
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

            // 配置日志。
            ConfigureLog(container);

            // 配置数据库及缓存。
            ConfigDatabase(container);

            // 配置消息队列服务。
            ConfigureMqService(container);

            // 配置第三方服务客户端。
            ConfigSdkClients(container);

            // 配置验证码相关功能。
            ConfigSecurity(container);

            // 配置用户身份功能。
            ConfigureMembership(container);

            // 配置身份验证和注册功能。
            ConfigureAuth(container);

            // 配置好友功能。
            ConfigureFriendship(container);

            // 配置商店功能。
            ConfigureShop(container);

            // 配置博客功能。
            ConfigureBlog(container);

            // 配置圣经 功能。
            ConfigureBible(container);

            // 配置插件功能。
            ConfigurePlugin(container);
        }

        #endregion

        #region 配置

        private void ConfigureLog(Container container)
        {
        }

        private void ConfigDatabase(Container container)
        {
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

        private void ConfigSdkClients(Container container)
        {
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