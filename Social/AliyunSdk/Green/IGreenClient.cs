using System.Threading.Tasks;

namespace Aliyun.Green
{
    /// <summary>
    ///     阿里内容安全服务客户端封装库的接口定义。
    /// </summary>
    public interface IGreenClient
    {
        #region 图片

        /// <summary>
        ///     检测图片。
        /// </summary>
        ImageScanResponse Post(ImageScanRequest request);

        /// <summary>
        ///     异步检测图片。
        /// </summary>
        Task<ImageScanResponse> PostAsync(ImageScanRequest request);

        #endregion

        #region 文本

        /// <summary>
        ///     检测文本。
        /// </summary>
        TextScanResponse Post(TextScanRequest request);

        /// <summary>
        ///     异步检测文本。
        /// </summary>
        Task<TextScanResponse> PostAsync(TextScanRequest request);

        #endregion
    }
}