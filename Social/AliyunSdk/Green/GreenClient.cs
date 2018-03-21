using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.Logging;
using ServiceStack.Text;
using AsyncContext = Nito.AsyncEx.AsyncContext;

namespace Aliyun.Green
{
    /// <summary>
    ///     阿里内容安全服务客户端封装库。
    /// </summary>
    public class GreenClient : IGreenClient
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(GreenClient));

        private const string version = "2017-01-12";
        private const string signatureMethod = "HMAC-SHA1";
        private const string signatureVersion = "1.0";

        #endregion

        #region 属性

        /// <summary>
        ///     申请应用时分配的应用程序唯一标识。
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        ///     申请应用时分配的应用程序密钥。
        /// </summary>
        public string AccessKeySecret { get; set; }

        /// <summary>
        ///     获取检测的基本地址。
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        ///     获取检测图片的地址路径。
        /// </summary>
        public string ImageScanPath { get; set; }

        /// <summary>
        ///     获取检测文本的地址路径。
        /// </summary>
        public string TextScanPath { get; set; }

        #endregion

        #region IGreenClient 接口实现

        /// <summary>
        ///     检测图片。
        /// </summary>
        public ImageScanResponse Post(ImageScanRequest request)
        {
            return AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步检测图片。
        /// </summary>
        public async Task<ImageScanResponse> PostAsync(ImageScanRequest request)
        {
            try
            {
                var httpHandler = new HttpClientHandler();
                using (var httpClient = new HttpClient(httpHandler))
                {
                    var requestJson = request.ToJson();
                    var signatureVersionNonce = Guid.NewGuid().ToString("D");
                    var currentDateTime = DateTimeOffset.Now;
                    var requestContent = new StringContent(requestJson, Encoding.UTF8);
                    requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    requestContent.Headers.ContentMD5 = requestJson.ToMd5HashBytes();
                    requestContent.Headers.Add("x-acs-signature-method", signatureMethod);
                    requestContent.Headers.Add("x-acs-signature-nonce", signatureVersionNonce);
                    requestContent.Headers.Add("x-acs-signature-version", signatureVersion);
                    requestContent.Headers.Add("x-acs-version", version);
                    var signatureBuilder = StringBuilderCache.Allocate();
                    signatureBuilder.Append("POST\n");
                    signatureBuilder.Append("application/json\n");
                    signatureBuilder.Append($"{Convert.ToBase64String(requestContent.Headers.ContentMD5)}\n");
                    signatureBuilder.Append("application/json\n");
                    signatureBuilder.Append($"{currentDateTime.ToUniversalTime():R}\n");
                    signatureBuilder.Append($"x-acs-signature-method:{signatureMethod}\n");
                    signatureBuilder.Append($"x-acs-signature-nonce:{signatureVersionNonce}\n");
                    signatureBuilder.Append($"x-acs-signature-version:{signatureVersion}\n");
                    signatureBuilder.Append($"x-acs-version:{version}\n");
                    signatureBuilder.Append(ImageScanPath);
                    var signature = Convert.ToBase64String(StringBuilderCache.ReturnAndFree(signatureBuilder).ToHmacSha1HashBytes(AccessKeySecret));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("acs", string.Format("{0}:{1}", AccessKeyId, signature));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Date = currentDateTime;
                    var responseMessage = await httpClient.PostAsync(string.Format("{0}{1}", BaseUrl, ImageScanPath), requestContent);
                    var responseContent = responseMessage.Content == null ? string.Empty : await responseMessage.Content.ReadAsStringAsync();
                    var response = responseContent.FromJson<ImageScanResponse>();
                    if (response.Code != 200)
                    {
                        Log.ErrorFormat("{0} {1} Error: {2}-{3}", GetType().Name, request.GetType().Name, response.Message, response.RequestId);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new ImageScanResponse
                       {
                           Code = -1,
                           Message = errorMessage
                       };
            }
        }

        /// <summary>
        ///     检测文本。
        /// </summary>
        public TextScanResponse Post(TextScanRequest request)
        {
            return AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步检测文本。
        /// </summary>
        public async Task<TextScanResponse> PostAsync(TextScanRequest request)
        {
            try
            {
                var httpHandler = new HttpClientHandler();
                using (var httpClient = new HttpClient(httpHandler))
                {
                    var requestJson = request.ToJson();
                    var signatureVersionNonce = Guid.NewGuid().ToString("D");
                    var currentDateTime = DateTimeOffset.Now;
                    var requestContent = new StringContent(requestJson, Encoding.UTF8);
                    requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    requestContent.Headers.ContentMD5 = requestJson.ToMd5HashBytes();
                    requestContent.Headers.Add("x-acs-signature-method", signatureMethod);
                    requestContent.Headers.Add("x-acs-signature-nonce", signatureVersionNonce);
                    requestContent.Headers.Add("x-acs-signature-version", signatureVersion);
                    requestContent.Headers.Add("x-acs-version", version);
                    var signatureBuilder = StringBuilderCache.Allocate();
                    signatureBuilder.Append("POST\n");
                    signatureBuilder.Append("application/json\n");
                    signatureBuilder.Append($"{Convert.ToBase64String(requestContent.Headers.ContentMD5)}\n");
                    signatureBuilder.Append("application/json\n");
                    signatureBuilder.Append($"{currentDateTime.ToUniversalTime():R}\n");
                    signatureBuilder.Append($"x-acs-signature-method:{signatureMethod}\n");
                    signatureBuilder.Append($"x-acs-signature-nonce:{signatureVersionNonce}\n");
                    signatureBuilder.Append($"x-acs-signature-version:{signatureVersion}\n");
                    signatureBuilder.Append($"x-acs-version:{version}\n");
                    signatureBuilder.Append(TextScanPath);
                    var signature = Convert.ToBase64String(StringBuilderCache.ReturnAndFree(signatureBuilder).ToHmacSha1HashBytes(AccessKeySecret));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("acs", string.Format("{0}:{1}", AccessKeyId, signature));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Date = currentDateTime;
                    var responseMessage = await httpClient.PostAsync(string.Format("{0}{1}", BaseUrl, TextScanPath), requestContent);
                    var responseContent = responseMessage.Content == null ? string.Empty : await responseMessage.Content.ReadAsStringAsync();
                    var response = responseContent.FromJson<TextScanResponse>();
                    if (response.Code != 200)
                    {
                        Log.ErrorFormat("{0} {1} Error: {2}-{3}", GetType().Name, request.GetType().Name, response.Message, response.RequestId);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TextScanResponse
                       {
                           Code = -1,
                           Message = errorMessage
                       };
            }
        }

        #endregion
    }
}