using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServiceStack.Extensions;
using ServiceStack.Text;

namespace Aliyun.Green
{
    /// <summary>
    ///     阿里云内容安全服务客户端封装库。
    /// </summary>
    public class GreenClient : IGreenClient
    {
        #region 静态变量

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
        public string BaseUrl { get; set; } = "https://green.cn-shanghai.aliyuncs.com";

        /// <summary>
        ///     获取检测图片的地址路径。
        /// </summary>
        public string ImageScanPath { get; set; } = "/green/image/scan";

        /// <summary>
        ///     获取异步检测图片的地址路径。
        /// </summary>
        public string ImageAsyncScanPath { get; set; } = "/green/image/asyncscan";

        /// <summary>
        ///     获取检测文本的地址路径。
        /// </summary>
        public string TextScanPath { get; set; } = "/green/text/scan";

        #endregion

        #region IGreenClient 接口实现

        /// <inheritdoc />
        /// <summary>
        ///     检测图片。
        /// </summary>
        public async Task<ImageScanResponse> PostAsync(ImageScanRequest request, ClientInfo clientInfo, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var httpHandler = new HttpClientHandler
                              {
                                  AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                              };
            using (var httpClient = new HttpClient(httpHandler, true))
            {
                var signatureTime = DateTimeOffset.Now;
                var signatureVersionNonce = Guid.NewGuid().ToString("D");
                var clientInfoJson = clientInfo == null ? string.Empty : JsonConvert.SerializeObject(clientInfo, Formatting.None);
                var requestJson = JsonConvert.SerializeObject(request, Formatting.None);
                var requestContent = new StringContent(requestJson, Encoding.UTF8);
                requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypes.Application.Json);
                requestContent.Headers.ContentMD5 = requestJson.ToMd5HashBytes();
                requestContent.Headers.Add("x-acs-signature-method", signatureMethod);
                requestContent.Headers.Add("x-acs-signature-nonce", signatureVersionNonce);
                requestContent.Headers.Add("x-acs-signature-version", signatureVersion);
                requestContent.Headers.Add("x-acs-version", version);
                var scanPath = clientInfo == null ? ImageScanPath : $"{ImageScanPath}?clientInfo={clientInfoJson}";
                var signature = Signature(Convert.ToBase64String(requestContent.Headers.ContentMD5), signatureTime, signatureVersionNonce, scanPath);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("acs", $"{AccessKeyId}:{signature}");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.Application.Json));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
                httpClient.DefaultRequestHeaders.Date = signatureTime;
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                var scanUrl = clientInfo == null ? $"{BaseUrl}{ImageScanPath}" : $"{BaseUrl}{ImageScanPath}?clientInfo={WebUtility.UrlEncode(clientInfoJson)}";
                var responseMessage = await httpClient.PostAsync(scanUrl, requestContent, cancellationToken);
                var responseContent = responseMessage.Content == null ? string.Empty : await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ImageScanResponse>(responseContent);
            }
        }

        /// <summary>
        ///     异步检测图片。
        /// </summary>
        public async Task<ImageScanResponse> PostAsync(ImageAsyncScanRequest request, ClientInfo clientInfo, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var httpHandler = new HttpClientHandler
                              {
                                  AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                              };
            using (var httpClient = new HttpClient(httpHandler, true))
            {
                var signatureTime = DateTimeOffset.Now;
                var signatureVersionNonce = Guid.NewGuid().ToString("D");
                var clientInfoJson = clientInfo == null ? string.Empty : JsonConvert.SerializeObject(clientInfo, Formatting.None);
                var requestJson = JsonConvert.SerializeObject(request, Formatting.None);
                var requestContent = new StringContent(requestJson, Encoding.UTF8);
                requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypes.Application.Json);
                requestContent.Headers.ContentMD5 = requestJson.ToMd5HashBytes();
                requestContent.Headers.Add("x-acs-signature-method", signatureMethod);
                requestContent.Headers.Add("x-acs-signature-nonce", signatureVersionNonce);
                requestContent.Headers.Add("x-acs-signature-version", signatureVersion);
                requestContent.Headers.Add("x-acs-version", version);
                var scanPath = clientInfo == null ? ImageAsyncScanPath : $"{ImageAsyncScanPath}?clientInfo={clientInfoJson}";
                var signature = Signature(Convert.ToBase64String(requestContent.Headers.ContentMD5), signatureTime, signatureVersionNonce, scanPath);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("acs", $"{AccessKeyId}:{signature}");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.Application.Json));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
                httpClient.DefaultRequestHeaders.Date = signatureTime;
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                var scanUrl = clientInfo == null ? $"{BaseUrl}{ImageAsyncScanPath}" : $"{BaseUrl}{ImageAsyncScanPath}?clientInfo={WebUtility.UrlEncode(clientInfoJson)}";
                var responseMessage = await httpClient.PostAsync(scanUrl, requestContent, cancellationToken);
                var responseContent = responseMessage.Content == null ? string.Empty : await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ImageScanResponse>(responseContent);
            }
        }

        /// <summary>
        ///     检测文本。
        /// </summary>
        public async Task<TextScanResponse> PostAsync(TextScanRequest request, ClientInfo clientInfo, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var httpHandler = new HttpClientHandler
                              {
                                  AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                              };
            using (var httpClient = new HttpClient(httpHandler, true))
            {
                var signatureTime = DateTimeOffset.Now;
                var signatureVersionNonce = Guid.NewGuid().ToString("D");
                var clientInfoJson = clientInfo == null ? string.Empty : JsonConvert.SerializeObject(clientInfo, Formatting.None);
                var requestJson = JsonConvert.SerializeObject(request, Formatting.None);
                var requestContent = new StringContent(requestJson, Encoding.UTF8);
                requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypes.Application.Json);
                requestContent.Headers.ContentMD5 = requestJson.ToMd5HashBytes();
                requestContent.Headers.Add("x-acs-signature-method", signatureMethod);
                requestContent.Headers.Add("x-acs-signature-nonce", signatureVersionNonce);
                requestContent.Headers.Add("x-acs-signature-version", signatureVersion);
                requestContent.Headers.Add("x-acs-version", version);
                var scanPath = clientInfo == null ? TextScanPath : $"{TextScanPath}?clientInfo={clientInfoJson}";
                var signature = Signature(Convert.ToBase64String(requestContent.Headers.ContentMD5), signatureTime, signatureVersionNonce, scanPath);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("acs", $"{AccessKeyId}:{signature}");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.Application.Json));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
                httpClient.DefaultRequestHeaders.Date = signatureTime;
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                var scanUrl = clientInfo == null ? $"{BaseUrl}{TextScanPath}" : $"{BaseUrl}{TextScanPath}?clientInfo={WebUtility.UrlEncode(clientInfoJson)}";
                var responseMessage = await httpClient.PostAsync(scanUrl, requestContent, cancellationToken);
                var responseContent = responseMessage.Content == null ? string.Empty : await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TextScanResponse>(responseContent);
            }
        }

        #endregion

        #region 签名

        private string Signature(string contentMd5, DateTimeOffset signatureTime, string signatureVersionNonce, string scanPath)
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("POST\n");
            builder.Append("application/json\n");
            builder.Append($"{contentMd5}\n");
            builder.Append("application/json\n");
            builder.Append($"{signatureTime.ToUniversalTime():R}\n");
            builder.Append($"x-acs-signature-method:{signatureMethod}\n");
            builder.Append($"x-acs-signature-nonce:{signatureVersionNonce}\n");
            builder.Append($"x-acs-signature-version:{signatureVersion}\n");
            builder.Append($"x-acs-version:{version}\n");
            builder.Append(scanPath);
            var signatureString = StringBuilderCache.ReturnAndFree(builder);
            var signature = signatureString.ToHmacSha1HashBytes(AccessKeySecret);
            return Convert.ToBase64String(signature);
        }

        #endregion
    }
}