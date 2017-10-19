using ServiceStack.Auth;
using ServiceStack.Logging;

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
    }
}