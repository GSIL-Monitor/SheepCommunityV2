using System.Threading;
using System.Threading.Tasks;

namespace Aliyun.Green
{
    /// <summary>
    ///     阿里云内容安全服务客户端封装库的接口定义。
    /// </summary>
    public interface IGreenClient
    {
        #region 图片

        /// <summary>
        ///     检测图片。
        /// </summary>
        Task<ImageScanResponse> PostAsync(ImageScanRequest request, ClientInfo clientInfo, CancellationToken cancellationToken);

        /// <summary>
        ///     异步检测图片。
        /// </summary>
        Task<ImageScanResponse> PostAsync(ImageAsyncScanRequest request, ClientInfo clientInfo, CancellationToken cancellationToken);

        #endregion

        #region 文本

        /// <summary>
        ///     检测文本。
        /// </summary>
        Task<TextScanResponse> PostAsync(TextScanRequest request, ClientInfo clientInfo, CancellationToken cancellationToken);

        #endregion

        #region 文件

        #endregion

        #region 声音

        #endregion
    }
}