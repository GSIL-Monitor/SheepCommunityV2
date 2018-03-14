using System.Threading.Tasks;

namespace JPush.Push
{
    /// <summary>
    ///     极光推送服务客户端封装库的接口定义。
    /// </summary>
    public interface IPushClient
    {
        #region 授权

        /// <summary>
        ///     推送消息。
        /// </summary>
        PushResponse Post(PushRequest request);

        /// <summary>
        ///     异步推送消息。
        /// </summary>
        Task<PushResponse> PostAsync(PushRequest request);

        #endregion
    }
}