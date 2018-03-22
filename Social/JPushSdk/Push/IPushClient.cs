using System.Threading.Tasks;

namespace JPush.Push
{
    /// <summary>
    ///     极光推送服务客户端封装库的接口定义。
    /// </summary>
    public interface IPushClient
    {
        #region 推送

        /// <summary>
        ///     获取CID列表。
        /// </summary>
        CidResponse Get(CidRequest request);

        /// <summary>
        ///     异步获取CID列表。
        /// </summary>
        Task<CidResponse> GetAsync(CidRequest request);

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