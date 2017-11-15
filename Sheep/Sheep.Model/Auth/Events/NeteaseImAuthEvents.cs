using System.Collections.Generic;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Extensions;
using ServiceStack.Logging;
using ServiceStack.Web;

namespace Sheep.Model.Auth.Events
{
    /// <summary>
    ///     与网易云通讯集成的注册登录注销的身份验证事件。
    /// </summary>
    public class NeteaseImAuthEvents : AuthEvents
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(NeteaseImAuthEvents));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        #endregion

        #region 重写事件调用函数

        /// <summary>
        ///     注册成功后调用。
        /// </summary>
        public override void OnRegistered(IRequest httpReq, IAuthSession session, IServiceBase registrationService)
        {
            var userCreateResponse = NimClient.Post(new UserCreateRequest
                                                    {
                                                        AccountId = session.UserAuthId,
                                                        Name = session.DisplayName,
                                                        Token = session.UserAuthId.ToMd5Hash()
                                                    });
            if (userCreateResponse.Code != 200)
            {
                Log.WarnFormat("NimClient user create error: {0}, AccountId={1}", userCreateResponse.Code, session.UserAuthId);
            }
        }

        /// <summary>
        ///     身份验证成功后调用。
        /// </summary>
        public override void OnAuthenticated(IRequest httpReq, IAuthSession session, IServiceBase authService, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
        }

        #endregion
    }
}